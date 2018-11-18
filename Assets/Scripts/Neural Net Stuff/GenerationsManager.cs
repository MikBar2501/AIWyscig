using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerationsManager : MonoBehaviour {

    public static GenerationsManager main;

    ButtonScript gameControl;
    TimeScript timeControl;

    public float generationTime;

    public Text genText;
    int genCounter;

    private void Awake()
    {
        main = this;
        gameControl = GetComponent<ButtonScript>();
        timeControl = GetComponent<TimeScript>();
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
    }
	
	// Update is called once per frame
	void Update () {
		if(timeControl.GetTime() >= generationTime)
        {
            gameControl.StopButton();
            gameControl.StartButton();
            IterGenCounter();
        }
	}
}
