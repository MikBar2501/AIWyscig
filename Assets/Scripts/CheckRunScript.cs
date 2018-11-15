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

	public List<CheckedPoint> checkedPoints;// = new List<CheckedPoint>();

	void Start() {
		CheckedPoint startPoint = new CheckedPoint();
		startPoint.SetValue(0f,0);
		checkedPoints[0] = startPoint;
	}

	public void newCheckedPoint(float time, int number) {
		CheckedPoint checkedPoint = new CheckedPoint();
		checkedPoint.SetValue(time,number);
		checkedPoints.Add(checkedPoint);
	}
}
