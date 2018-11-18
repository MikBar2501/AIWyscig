using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void Awake()
    {
        main = this;
        gameControl = GetComponent<ButtonScript>();
        timeControl = GetComponent<TimeScript>();
        display = GetComponent<GenerationDisplay>();
    }

    void IterGenCounter()
    {
        genCounter++;
        genText.text = "Generation: " + genCounter;
    }

    // Use this for initialization
    void Start () {
        gameControl.StartButton();
        IterGenCounter();
        CreateFirstGeneration();
        display.UpdateDisplay();
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

            print("new brain - " + oldBrains[netIterator].GetFitness());

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
		if(timeControl.GetTime() >= generationTime)
        {
            CalculateFitness();
            List<NeuralNetwork> oldBrains = GetSortedNets();

            foreach (NeuralNetwork net in oldBrains)
                print("brain - " + net.GetFitness());

            gameControl.StopButton();
            gameControl.StartButton();

            CreateNextGeneration(oldBrains);
            display.UpdateDisplay();

            IterGenCounter();
        }
	}
}
