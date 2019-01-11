using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random=UnityEngine.Random;
using System;
using System.IO;

public class GenerationsManager : MonoBehaviour {

    public static GenerationsManager main;

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

    private void Awake()
    {
        main = this;
        gameControl = GetComponent<ButtonScript>();
        timeControl = GetComponent<TimeScript>();
        display = GetComponent<GenerationDisplay>();
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
    }

    // Use this for initialization
    void Start () {
        if(!menuMode)
            gameControl.StartButton();

        if (Training() && !menuMode)
        {
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

        foreach (AICarControl car in GameObject.FindObjectsOfType<AICarControl>())
        {
            Color m_NewColor = SwitchColor(Random.Range(0,5));
            car.myColor = m_NewColor;
            car.GetComponent<SpriteRenderer>().color = car.myColor;
            

            car.brain = new NeuralNetwork(startNet);

            if (!first)
            {
                car.brain.Mutate(true, 1);
            }
            else
            {
                display.bestCar = car.gameObject;
                NeuralNetwork.best = car.brain;
            }
            first = false;
        }
        NeuralList.list.CarList();
        

    }

    void CreateNextGeneration(List<NeuralNetwork> oldBrains)
    {
        gameControl.StopButton();
        gameControl.StartButton();

        ///==Wyrzucenie zbędnych mozgów===///
        int keepCount = (int)(oldBrains.Count * keepPerc);
        if (keepCount == 0)
            keepCount = 1;

        if (oldBrains[0].GetFitness() == 0)
            keepCount = (int)((float)oldBrains.Count / 2);

        if (keepCount == 0)
            keepCount = 1;

        int cutCount = oldBrains.Count - keepCount;
        for (int i = 0; i < cutCount; i++)
        {
            oldBrains.RemoveAt(oldBrains.Count - 1);
        }

        ///==Rozmnazanie Mózgów==///
        int netIterator = 0;
        bool bestOne = true;
        foreach (AICarControl car in GameObject.FindObjectsOfType<AICarControl>())
        {
            car.brain = new NeuralNetwork(oldBrains[netIterator]); //netIterator względem tego pokolorować
            
            Color m_NewColor = SwitchColor(Random.Range(0,5));
            car.myColor = m_NewColor;
            car.GetComponent<SpriteRenderer>().color = car.myColor;
            //print("new brain - " + oldBrains[netIterator].GetFitness());

            if (!bestOne)
                car.brain.Mutate(true, 0.1f);
            else
            {
                //car.GetComponent<ColorChangerSR>().ChangeColor(Color.yellow);
                display.bestCar = car.gameObject;
                NeuralNetwork.best = car.brain;
                bestOne = false;
            }

            netIterator++;
            if (netIterator >= oldBrains.Count)
                netIterator = 0;
        }
        NeuralList.list.CarList();
        display.UpdateDisplay();
        IterGenCounter();
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
