using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InstancePlayer : MonoBehaviour {

	public Transform carPrefab;
	public Transform playerPrefab;
	public int carCount;
	//public List<Transform> carPrefabs;
	public Vector3 startPos;

    public Transform carsHolder;
	public static InstancePlayer instancePlayer;
	public bool learnMode;

	void Start() {
		instancePlayer = this;
		
	}

	public void StartGame() {
		//int i = 0;
		/*foreach(Transform car in carPrefabs) {
			Instantiate(car,startPos + new Vector3((float) i,0,0),Quaternion.identity);
			i++;
		}*/

		if(learnMode) {
			for(int i = 0; i < carCount; i++) {
				Debug.Log("Wykonaj Autko: " + i);
				(Instantiate(carPrefab,startPos + new Vector3((float)(0.1f * (float)i),0,0),Quaternion.identity)).transform.parent = carsHolder;
			}
		} else {
			(Instantiate(carPrefab,startPos + new Vector3(0f,0,0),Quaternion.identity)).transform.parent = carsHolder;
			(Instantiate(playerPrefab,startPos + new Vector3(1f,0,0),Quaternion.identity)).transform.parent = carsHolder;

		}
		
		
	}

	public void StopGame() {
		GameObject [] destObj = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject car in destObj) {
			Destroy(car);
		}	
	}

	public void StartsLength() {
		int i = GetComponent<ButtonScript>().startPoints.Length;
		Debug.Log("Ilość startów: " + i);
		i = i / 2;
		startPos = GetComponent<ButtonScript>().startPoints[i].transform.position;
	}
	
	
}
