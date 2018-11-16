using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {

	InstancePlayer instancePlayer;
	TimeScript timeScript;
	public StartPointScript startPoint;

	public bool playGame;

	// Use this for initialization
	void Start () {
		instancePlayer = GetComponent<InstancePlayer>();
		timeScript = GetComponent<TimeScript>();
		
	}
	
	public void StartButton() {
		if(!playGame) {
			timeScript.TimeReset();
			timeScript.StartTime();
			instancePlayer.StartGame();
			startPoint.SetStart();
		} else {
			timeScript.TimeReset();
			instancePlayer.StopGame();
			timeScript.StartTime();
			instancePlayer.StartGame();
			startPoint.SetStart();
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
