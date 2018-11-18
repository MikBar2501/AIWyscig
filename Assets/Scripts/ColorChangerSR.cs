using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangerSR : MonoBehaviour {

	public void ChangeColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
        foreach (SpriteRenderer rend in GetComponentsInChildren<SpriteRenderer>())
            rend.color = color;
    }
}
