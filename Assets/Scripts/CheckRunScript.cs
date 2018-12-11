using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public struct CheckedPoint {
	public float time;
	public int checkNumber;
	

	public void SetValue(float time, int checkNumber) {
		this.time = time;
		this.checkNumber = checkNumber;
	}
}

public class CheckRunScript : MonoBehaviour {
	public TimeScript timeScript;

	public List<CheckedPoint> checkedPoints;// = new List<CheckedPoint>();

    int checkPointsCount;

	void Awake() {
		timeScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<TimeScript>();
	}

    void Start()
    {
        CheckedPoint startPoint = new CheckedPoint();
        startPoint.SetValue(0f, 0);
        checkedPoints[0] = startPoint;
        checkPointsCount = FindObjectsOfType<CheckPointScript>().Length;
    }

    bool Contains(int index)
    {
        foreach(CheckedPoint checkedPoint in checkedPoints)
        {
            if (checkedPoint.checkNumber == index)
                return true;
        }

        return false;
    }

	public void newCheckedPoint(float time, int number) {
        if (Contains(number))
            return;

        int lastCheckPoint = checkedPoints[checkedPoints.Count - 1].checkNumber;

        if (number != lastCheckPoint + 1)
        {
            if(lastCheckPoint != checkPointsCount || number != 0)
            return;
        }

		CheckedPoint checkedPoint = new CheckedPoint();
		checkedPoint.SetValue(time,number);
		checkedPoints.Add(checkedPoint);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Checkpoint") {
				newCheckedPoint(timeScript.GetTime(), other.GetComponent<CheckPointScript>().MyNumber());
		}	
	}
}
