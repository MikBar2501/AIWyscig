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

	void Awake() {
		instancePlayer = this;

        learnMode = GenerationsManager.Training();

        if(learnMode)
        {
            carCount = GetCarsCount();
        }
	}

    int GetCarsCount()
    {
        carCount = 0;
        for(int i = 0; i < GenerationsManager.generationsSettings.Count; i++)
        {
            carCount += GenerationsManager.generationsSettings[i].count;
            print(carCount);
        }
        return carCount;
    }

	public void StartGame() {
		if(learnMode) {
			for(int i = 0; i < carCount; i++) {
				//Debug.Log("Wykonaj Autko: " + i);
				(Instantiate(carPrefab,startPos,Quaternion.identity)).transform.parent = carsHolder;
			}
		} else {
			(Instantiate(carPrefab,startPos,Quaternion.identity)).transform.parent = carsHolder;
			(Instantiate(playerPrefab,startPos,Quaternion.identity)).transform.parent = carsHolder;

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
