using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {

	InstancePlayer instancePlayer;
	TimeScript timeScript;
	[HideInInspector]
	public GameObject[] startPoints;

	public bool playGame;

	// Use this for initialization
	void Awake () {
		instancePlayer = GetComponent<InstancePlayer>();
		timeScript = GetComponent<TimeScript>();
		
		
	}

	void Start() {
		startPoints = GameObject.FindGameObjectsWithTag("Start");
		InstancePlayer.instancePlayer.StartsLength();
	}
	
	public void StartButton() {
		if(!playGame) {
			timeScript.TimeReset();
			timeScript.StartTime();
			instancePlayer.StartGame();
			foreach(GameObject start in startPoints) {
				start.GetComponent<StartPointScript>().SetStart();
			}
			
		} else {
			timeScript.TimeReset();
			instancePlayer.StopGame();
			timeScript.StartTime();
			instancePlayer.StartGame();
			foreach(GameObject start in startPoints) {
				start.GetComponent<StartPointScript>().SetStart();
			}
		}
		playGame = true;
	}

	public void StopButton() {
		if(playGame) {
			timeScript.StopTime();
			instancePlayer.StopGame();
		}
	}
}
