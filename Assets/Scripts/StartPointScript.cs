using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPointScript : MonoBehaviour {

	public List<CheckPointScript> checkPointsList;

	public int laps;

	void Start() {
		SetStart();	
	}

	private void OnTriggerExit2D(Collider2D other) {
		foreach(CheckPointScript point in checkPointsList) {
			point.setActive();
		}
		laps++;
	}

	public void SetStart() {
		laps = 0;
	}

}
