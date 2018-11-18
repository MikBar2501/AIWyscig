using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class TimeScript : MonoBehaviour {

	public Text counterText;
	public float seconds, minutes;
	public bool timeRun;
	float time;

	void Update() {
		time += Time.deltaTime;
		if(timeRun) {
			minutes = (int)(time/60f);
			seconds = (int)(time % 60f);
			counterText.text = minutes.ToString("00") + ":" + seconds.ToString("00");	
		}
	}

	public void StartTime() {
		timeRun = true;
	}

	public void StopTime() {
		timeRun = false;
	}

	public void TimeReset() {
		time = 0f;
		minutes = 0;
		seconds = 0;
		counterText.text = "00:00";
	}

	public float GetTime() {
		return time;
	}
}
