using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarControl : CarController {

    public NeuralNetwork brain;
    Sight sight;

    CheckRunScript checkRun;
    public Color myColor;

    private void Awake()
    {
        sight = GetComponentInChildren<Sight>();
        checkRun = GetComponent<CheckRunScript>();
    }

    public void CalculateFitness()
    {
        float totalFitness = 0.0f;

        GenerationSettings settings = GenerationsManager.main.GetSettingsOfID(brain.ID); //to jest takie glupie omg stop
        GenerationInfo info = GenerationsManager.main.generationsInfo[settings.ID];
        int bestCheks = info.checkPointReached;
        if (checkRun.checkedPoints.Count > bestCheks)
        {
            info.checkPointReached = checkRun.checkedPoints.Count;
            GenerationsManager.main.AddLineToData("reached check point " + (checkRun.checkedPoints.Count - 1), settings.ID);
            GenerationsManager.main.generationsInfo[settings.ID] = info;
        }

        CheckedPoint lastCheckPoint = checkRun.checkedPoints[0];
        for (int i = 1; i < checkRun.checkedPoints.Count; i++) 
        {
            CheckedPoint checkPoint = checkRun.checkedPoints[i];

            totalFitness += 1 / (checkPoint.time - lastCheckPoint.time) + 3;

            lastCheckPoint = checkPoint;
        }

        brain.SetFitness(totalFitness);
    }

    public void Show(bool show)
    {
        GetComponent<SpriteRenderer>().enabled = show;

        foreach(SpriteRenderer render in GetComponentsInChildren<SpriteRenderer>())
        {
            render.enabled = show;
        }

        foreach (LineRenderer render in GetComponentsInChildren<LineRenderer>())
        {
            render.enabled = show;
        }
    }

    public override void Control()
    {
        float[] input = sight.GetSight();
        float[] output = brain.FeedForward(input);

        move = output[0];
        rotation = output[1];
    }
}
