using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointScript : MonoBehaviour {

	public int myNumber;
	public GameObject gameMaster;
	CheckRunScript checkRunScript;
	TimeScript timeScript;

	// Use this for initialization
	void Start () {
		checkRunScript = gameMaster.GetComponent<CheckRunScript>();
		timeScript = gameMaster.GetComponent<TimeScript>();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Player") {
			checkRunScript.newCheckedPoint(timeScript.GetTime(), myNumber);
		}	
	}
}
