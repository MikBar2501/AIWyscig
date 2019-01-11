using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralList : MonoBehaviour {

	public List<GameObject> carsControls;
	public static NeuralList list;
	int checkedCar;

	// Use this for initialization
	void Start () {
		list = this;
		checkedCar = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			NextCarInList(1);
		}

		if(Input.GetKeyDown(KeyCode.Alpha3)) {
			NextCarInList(-1);
		}
	}

	public void CarList() {
		/* if (carsControls == null) {
			carsControls = GameObject.FindGameObjectsWithTag("Player");
		}*/
		//carsControls[0].GetComponent<SpriteRenderer>().color = Color.cyan;
		carsControls.Clear();

		foreach (GameObject carCon in GameObject.FindGameObjectsWithTag("Player"))
        {
            carsControls.Add(carCon);
        }
		carsControls[checkedCar].GetComponent<SpriteRenderer>().color = Color.cyan;

	}

	public void NextCarInList(int direct) {
		carsControls[checkedCar].GetComponent<SpriteRenderer>().color = carsControls[checkedCar].GetComponent<AICarControl>().myColor;
		checkedCar += direct;
		if(checkedCar < 0) {
			checkedCar = carsControls.Count;
		}
		carsControls[checkedCar].GetComponent<SpriteRenderer>().color = Color.cyan;
		
	}


}
