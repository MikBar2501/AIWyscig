using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationDisplay : MonoBehaviour {

    bool bestOne;

    public Transform carsHolder;
    [HideInInspector]
    public GameObject bestCar;

    public GameObject saveButton;
    public GameObject toggle;
    public GameObject slider;
    public GameObject leaveButton;

    private void Awake()
    {
        if (!GenerationsManager.Training())
        {
            saveButton.SetActive(false);
            toggle.SetActive(false);
            slider.SetActive(false);
        }
        else
        {
            leaveButton.SetActive(false);
            enabled = false;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Application.LoadLevel(1);
        }
    }

    public void Toggle()
    {
        bestOne = !bestOne;
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        if (!bestCar)
            return;

        if (bestCar)
            bestCar.transform.parent = null;

        foreach (Transform car in carsHolder)
        {
            car.GetComponent<AICarControl>().Show(!bestOne);
        }
        if (bestCar)
        {
            bestCar.transform.parent = null;
            //bestCar.transform.SetAsFirstSibling();
        }
    }

    public void Leave()
    {
        GenerationsManager.main.SaveData();

        Application.LoadLevel(0);
    }
}
