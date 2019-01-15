using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random=UnityEngine.Random;
using System;
using System.IO;

public struct GenerationSettings
{
    public Color color;
    public int ID;
    public int count;
    public int startIndex;
    public int endIndex;
    public float keepPerc;
    public float rangeOfMutation;
    public int softMutationChanse;
    public int hardMutationChanse;
}

public struct GenerationInfo
{
    public int checkPointReached;
    public float bestFitness;
}

public class GenerationsManager : MonoBehaviour {

    public static GenerationsManager main;
    public static List<GenerationSettings> generationsSettings;
    public List<GenerationInfo> generationsInfo;

    public bool menuMode;

    public int[] layers;

    ButtonScript gameControl;
    TimeScript timeControl;
    GenerationDisplay display;

    public float generationTime;
    [Range(0.0f,1.0f)]
    public float keepPerc;

    public Text genText;
    int genCounter;

    public static string saveFileName = "brain.nc";

    public Text genTimeText;

    List<int> groupsCount;

    List<string> data;

    public void AddLineToData(string line)
    {
        data.Add(line);
    }

    public void AddLineToData(string line, int ID)
    {
        string groupName = "nieznany: ";
        switch(ID)
        {
            case 0:
                groupName = "blue: ";
                break;

            case 1:
                groupName = "red: ";
                break;

            case 2:
                groupName = "green: ";
                break;
        }

        data.Add(groupName + line);
    }

    public void SaveData()
    {
        int counter = 0;
        while (File.Exists("test" + counter + ".txt"))
            counter++;

        File.WriteAllLines(@"test" +counter + ".txt", data.ToArray());
    }

    private void Awake()
    {
        main = this;
        gameControl = GetComponent<ButtonScript>();
        timeControl = GetComponent<TimeScript>();
        display = GetComponent<GenerationDisplay>();

        data = new List<string>();
        generationsInfo = new List<GenerationInfo>();

        if(generationsSettings != null)
        for (int i = 0; i < generationsSettings.Count; i++)
            generationsInfo.Add(new GenerationInfo());
    }

    public void SetGenTime(Slider slider)
    {
        generationTime = slider.value;

        genTimeText.text = "generation time: " + ("00:") + (generationTime<10? "0": "") + (int)generationTime;
        if((int)generationTime >= 60)
        {
            genTimeText.text = "generation time: 01:00";
        }
    }

    public static bool Training()
    {
        if (!PlayerPrefs.HasKey("training") || PlayerPrefs.GetInt("training") == 1)
        {
            return true;
        }
        return false;
    }

    void IterGenCounter()
    {
        genCounter++;
        genText.text = "Generation: " + genCounter;
        AddLineToData("===GENERATION " + genCounter + "===");

    }

    // Use this for initialization
    void Start () {


        //string[] lines = { "First line", "Second line", "Third line" };

        if (!menuMode)
        {
            groupsCount = new List<int>();
            for (int i = 0; i < generationsSettings.Count; i++)
            {
                groupsCount.Add(generationsSettings[i].count);
            }
            gameControl.StartButton();
        }

        if (Training() && !menuMode)
        {
            for (int i = 0; i < generationsSettings.Count; i++)
            {
                GenerationSettings setting = generationsSettings[i];
                AddLineToData("SETTINGS", i);
                AddLineToData("gen count - " + setting.count, i);
                AddLineToData("keep perc - " + setting.keepPerc + "%", i);
                AddLineToData("soft mutation chance - " + setting.softMutationChanse + "%", i);
                AddLineToData("hard mutation chance - " + setting.hardMutationChanse + "%", i);
                AddLineToData("range of change - " + setting.rangeOfMutation, i);
                AddLineToData("");
            }

            IterGenCounter();
            CreateFirstGeneration();
            display.UpdateDisplay();
            return;
        }

        NeuralNetwork brain = Load();
        AICarControl car = FindObjectOfType<AICarControl>();
        car.brain = brain;
        NeuralNetwork.best = car.brain;
        foreach (Eye eye in car.GetComponentsInChildren<Eye>())
            eye.HideRays();
        
        enabled = false;

        if (!menuMode)
        {
            foreach (CarController carCon in FindObjectsOfType<CarController>())
            {
                carCon.enabled = false;
            }

            StartCountDown.main.StartCount();
        }
    }
	
    public GenerationSettings GetSettingsOfID(int ID)
    {
        for(int i = 0; i < generationsSettings.Count; i++)
        {
            if (ID > generationsSettings[i].endIndex)
                continue;

            return generationsSettings[i];
        }

        return new GenerationSettings();
    }

    public void ActivateCars()
    {
        foreach (CarController carCon in FindObjectsOfType<CarController>())
        {
            carCon.enabled = true;
        }
    }

    void CreateFirstGeneration()
    {
        NeuralNetwork startNet;
        if (!File.Exists(saveFileName))
        {
            startNet = new NeuralNetwork(layers);
        }
        else
        {
            startNet = Load();
        }

        bool first = true;
        NeuralList.list.ClearList();

        int iterator = 0;
        foreach (AICarControl car in GameObject.FindObjectsOfType<AICarControl>())
        {
            //Color m_NewColor = SwitchColor(Random.Range(0,5));
            //Color m_NewColor = SwitchColor(Random.Range(0,5));start
            //car.myColor = m_NewColor;
            car.myColor = GetSettingsOfID(iterator).color;
            car.GetComponent<SpriteRenderer>().color = car.myColor;
            NeuralList.list.AddInList(car);
            

            car.brain = new NeuralNetwork(startNet);
            car.brain.ID = iterator;
            iterator++;

            if (!first)
            {
                car.brain.Mutate(true, 1, 100,100,100, true);
            }
            else
            {
                display.bestCar = car.gameObject;
                NeuralNetwork.best = car.brain;
            }
            first = false;
        }
        NeuralList.list.Invoke("CheckedList",0.5f);
        //NeuralList.list.CarList();
        

    }

    void CreateNextGeneration(List<NeuralNetwork> oldBrains)
    {    

        gameControl.StopButton();
        gameControl.StartButton();


        NeuralNetwork.best = oldBrains[0];
        for(int i = 0; i < generationsSettings.Count; i++)
        {
            List<NeuralNetwork> groupBrains = new List<NeuralNetwork>();
            for(int n = 0; n < oldBrains.Count; n++)
            {
                NeuralNetwork brain = oldBrains[n];
                if (GetSettingsOfID(brain.ID).ID == i)
                {
                    groupBrains.Add(brain);
                }
            }

            CreateNextGenerationGroup(i, groupBrains);
        }
        
        NeuralList.list.ClearList();
        ///==Rozmnazanie Mózgów==///
        //int netIterator = 0;
        //bool bestOne = true;
        //int iterator = 0;

        //foreach (AICarControl car in GameObject.FindObjectsOfType<AICarControl>())
        //{
        //    car.brain = new NeuralNetwork(oldBrains[netIterator]); //netIterator względem tego pokolorować
            
        //    //Color m_NewColor = SwitchColor(Random.Range(0,5));
        //    //car.myColor = m_NewColor;
        //    car.myColor = GetSettingsOfID(iterator).color;
        //    car.GetComponent<SpriteRenderer>().color = car.myColor;
        //    //print("new brain - " + oldBrains[netIterator].GetFitness());

        //     NeuralList.list.AddInList(car);
        //    if (!bestOne)
        //        car.brain.Mutate(true, 0.1f);
        //    else
        //    {
        //        //car.GetComponent<ColorChangerSR>().ChangeColor(Color.yellow);
        //        display.bestCar = car.gameObject;
        //        NeuralNetwork.best = car.brain;
        //        bestOne = false;
        //    }

        //    iterator++;
        //    netIterator++;
        //    if (netIterator >= oldBrains.Count)
        //        netIterator = 0;
        //}
        //NeuralList.list.CarList();
        NeuralList.list.Invoke("CheckedList",0.5f);
        //Debug.Log("ListaReady");
        display.UpdateDisplay();
        IterGenCounter();
    }

    void CreateNextGenerationGroup(int ID, List<NeuralNetwork> oldBrains)
    {
        GenerationSettings settings = generationsSettings[ID];

        int keepCount = (int)(oldBrains.Count * settings.keepPerc);
        if (keepCount == 0)
            keepCount = 1;

        float bestFittnes = oldBrains[0].GetFitness();
        GenerationInfo info = generationsInfo[ID];

        if (bestFittnes > info.bestFitness)
        {
            info.bestFitness = bestFittnes;
            AddLineToData("best fit - " + bestFittnes, ID);
            generationsInfo[ID] = info;
        }

        if (bestFittnes == 0)
            keepCount = (int)((float)oldBrains.Count / 2);

        if (keepCount == 0)
            keepCount = 1;

        int cutCount = oldBrains.Count - keepCount;
        for (int i = 0; i < cutCount; i++)
        {
            oldBrains.RemoveAt(oldBrains.Count - 1);
        }


        int netIterator = 0;
        bool bestOne = true;
        //int iterator = 0;
        bool mutate = false;

        AICarControl[] cars = GameObject.FindObjectsOfType<AICarControl>();

        for (int i = settings.startIndex; i <= settings.endIndex; i++)
        {
            AICarControl car = cars[i];

            car.brain = new NeuralNetwork(oldBrains[netIterator]); //netIterator względem tego pokolorować
            car.brain.ID = i;

            //Color m_NewColor = SwitchColor(Random.Range(0,5));
            //car.myColor = m_NewColor;
            car.myColor = GetSettingsOfID(i).color;
            car.GetComponent<SpriteRenderer>().color = car.myColor;
            //print("new brain - " + oldBrains[netIterator].GetFitness());

            NeuralList.list.AddInList(car);
            int hard = (int)((float)settings.hardMutationChanse / 2);
            if (hard == 0 && settings.hardMutationChanse > 0)
                hard = 1;

            if (mutate)
                car.brain.Mutate(true, settings.rangeOfMutation, hard, hard, settings.softMutationChanse);
            else
            {
                if(NeuralNetwork.best == oldBrains[netIterator])
                {
                    NeuralNetwork.best = car.brain;
                    display.bestCar = car.gameObject;
                }

            }
            mutate = true;
            //iterator++;
            netIterator++;
            if (netIterator >= oldBrains.Count)
            {
                netIterator = 0;
            }
        }
    }

    void ApplyColors()
    {

    }

    List<NeuralNetwork> GetSortedNets()
    {
        List<NeuralNetwork> nets = new List<NeuralNetwork>();

        foreach (AICarControl car in GameObject.FindObjectsOfType<AICarControl>())
        {
            bool inserted = false;
            for(int i = 0; i < nets.Count; i++)
            {
                if(car.brain.GetFitness() > nets[i].GetFitness())
                {
                    nets.Insert(i, car.brain);
                    inserted = true;
                    break;
                }
            }
            if (!inserted)
                nets.Add(car.brain);

        }

        return nets;
    }

    void CalculateFitness()
    {
        foreach (AICarControl car in GameObject.FindObjectsOfType<AICarControl>())
        {
            car.CalculateFitness();
        }
    }

	// Update is called once per frame
	void Update () {

        if(Input.GetKey(KeyCode.P))
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            NeuralNetwork net = Load();
            List<NeuralNetwork> genSample = new List<NeuralNetwork>();
            genSample.Add(net);
            CreateNextGeneration(genSample);
        }

            if (timeControl.GetTime() >= generationTime)
        {
            CalculateFitness();
            List<NeuralNetwork> oldBrains = GetSortedNets();

            foreach (NeuralNetwork net in oldBrains)
                print("brain - " + net.GetFitness());

            CreateNextGeneration(oldBrains);
        }
	}



    public void Save()
    {
        if (!display.bestCar)
            return;

        NeuralNetwork net = display.bestCar.GetComponent<AICarControl>().brain;

        string saveText = "";

        for (int x = 0; x < net.weights.Length; x++)
            for (int y = 0; y < net.weights[x].Length; y++)
                for (int z = 0; z < net.weights[x][y].Length; z++)
                    saveText += net.weights[x][y][z] + Environment.NewLine;

        File.WriteAllText(saveFileName, saveText);
    }

    public NeuralNetwork Load()
    {
        return Load(saveFileName);
    }

    public NeuralNetwork Load(string fileName)
    {
        NeuralNetwork net = new NeuralNetwork(layers);

        if (!File.Exists(fileName))
            return net;

        string[] lines = File.ReadAllLines(fileName);
        int iterator = 0;

        for (int x = 0; x < net.weights.Length; x++)
            for (int y = 0; y < net.weights[x].Length; y++)
                for (int z = 0; z < net.weights[x][y].Length; z++)
                    net.weights[x][y][z] = float.Parse(lines[iterator++]);

        return net;
    }

    Color SwitchColor(int chooseColor) {
        switch(chooseColor){
            case 0:
                return Color.blue;
                break;
            case 1:
                return Color.green;
                break;
            case 2:
                return Color.red;
                break;
            case 3:
                return Color.yellow;
                break;
            case 4:
                return Color.magenta;
                break;
            default:
                return Color.white;
                break;
        }
    }
}
