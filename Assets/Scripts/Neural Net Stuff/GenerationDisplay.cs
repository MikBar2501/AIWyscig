using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationDisplay : MonoBehaviour {

    bool bestOne;

    public Transform carsHolder;
    [HideInInspector]
    public GameObject bestCar;

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
}
