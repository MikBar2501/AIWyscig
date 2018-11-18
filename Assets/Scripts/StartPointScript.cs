using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPointScript : MonoBehaviour {


	public int laps;

	void Start() {
		SetStart();	
	}

	private void OnTriggerExit2D(Collider2D other) {
		laps++;
	}

	public void SetStart() {
		laps = 0;
	}

}
