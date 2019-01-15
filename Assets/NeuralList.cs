using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralList : MonoBehaviour {

	//public List<GameObject> carsControls;
	public List<AICarControl> carsControls;
	public static NeuralList list;
	int checkedCar;

	// Use this for initialization
	void Start () {
		
		
	}

	void Awake() {
		carsControls = new List<AICarControl>();
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



		/* CarsControls.Clear();

		foreach (GameObject carCon in GameObject.FindGameObjectsWithTag("Player"))
        {
            carsControls.Add(carCon);
        }
		carsControls[checkedCar].GetComponent<SpriteRenderer>().color = Color.cyan;*/

		//foreach(AICarControl car in GameObject.FindObjectsOfType<AICarControl>()){

		//}

	}

	public void NextCarInList(int direct) {
		carsControls[checkedCar].GetComponent<SpriteRenderer>().color = carsControls[checkedCar].GetComponent<AICarControl>().myColor;
		checkedCar += direct;
		if(checkedCar < 0) {
			checkedCar = carsControls.Count-1;
		}
		if(checkedCar >= carsControls.Count) {
			checkedCar = 0;
		}
		carsControls[checkedCar].GetComponent<SpriteRenderer>().color = Color.cyan;
		carsControls[checkedCar].transform.SetAsLastSibling();
		
	}

	public void ClearList() {
		carsControls.Clear();
	}

	public void AddInList(AICarControl car) {
		carsControls.Add(car);
	}

	public void CheckedList() {
		Debug.Log("Czyszczenie");
		for(int i = carsControls.Count-1; i >= 0; i--) {
			if (carsControls[i] == null) {
				carsControls.RemoveAt(i);
			}
		}
		checkedCar = 0;
	}


}
