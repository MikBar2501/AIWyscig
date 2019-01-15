using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Control : MonoBehaviour {

	public virtual float GetValue()
    {
        return 0.0f;
    }

    public void SetName(string name)
    {
        transform.GetChild(0).GetComponent<Text>().text = name;
    }

    public virtual void SetBaseValue(float value)
    {

    }
}
