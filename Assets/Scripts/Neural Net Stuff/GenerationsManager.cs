using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class GenerationsManager : MonoBehaviour {

    public static GenerationsManager main;

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

    private void Awake()
    {
        main = this;
        gameControl = GetComponent<ButtonScript>();
        timeControl = GetComponent<TimeScript>();
        display = GetComponent<GenerationDisplay>();
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
        gameControl.StartButton();

        if (Training())
        {
            IterGenCounter();
            CreateFirstGeneration();
            display.UpdateDisplay();
            return;
        }

        NeuralNetwork brain = Load();
        AICarControl car = FindObjectOfType<AICarControl>();
        car.brain = brain;
        foreach (Eye eye in car.GetComponentsInChildren<Eye>())
            eye.HideRays();
        
        enabled = false;

        foreach(CarController carCon in FindObjectsOfType<CarController>())
        {
            carCon.enabled = false;
        }

        StartCountDown.main.StartCount();
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
        NeuralNetwork startNet = new NeuralNetwork(layers);
        foreach(AICarControl car in GameObject.FindObjectsOfType<AICarControl>())
        {
            car.brain = new NeuralNetwork(startNet);
            car.brain.Mutate(true, 1);
        }
    }

    void CreateNextGeneration(List<NeuralNetwork> oldBrains)
    {
        gameControl.StopButton();
        gameControl.StartButton();

        ///==Wyrzucenie zbędnych mozgów===///
        int keepCount = (int)(oldBrains.Count * keepPerc);
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
            car.brain = new NeuralNetwork(oldBrains[netIterator]);

            //print("new brain - " + oldBrains[netIterator].GetFitness());

            if (!bestOne)
                car.brain.Mutate(true, 0.1f);
            else
            {
                car.GetComponent<ColorChangerSR>().ChangeColor(Color.yellow);
                display.bestCar = car.gameObject;
                bestOne = false;
            }

            netIterator++;
            if (netIterator >= oldBrains.Count)
                netIterator = 0;
        }

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
        NeuralNetwork net = new NeuralNetwork(layers);

        if (!File.Exists(saveFileName))
            return net;

        string[] lines = File.ReadAllLines(saveFileName);
        int iterator = 0;

        for (int x = 0; x < net.weights.Length; x++)
            for (int y = 0; y < net.weights[x].Length; y++)
                for (int z = 0; z < net.weights[x][y].Length; z++)
                    net.weights[x][y][z] = float.Parse(lines[iterator++]);

        return net;
    }
}
