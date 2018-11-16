using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointScript : MonoBehaviour {

	public int myNumber;
	public GameObject gameMaster;
	TimeScript timeScript;
	bool checkActive;

	// Use this for initialization
	void Start () {
		setActive();
		timeScript = gameMaster.GetComponent<TimeScript>();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Player") {
			if(checkActive) {
				other.GetComponent<CheckRunScript>().newCheckedPoint(timeScript.GetTime(), myNumber);
				checkActive = false;
			}
		}	
	}

	public void setActive() {
		checkActive = true;
	}
}
