using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainInserter : MonoBehaviour {

    public string brainFlieName;

	// Use this for initialization
	void Start () {
        GetComponent<AICarControl>().brain =
        GenerationsManager.main.Load(brainFlieName);
	}

}
