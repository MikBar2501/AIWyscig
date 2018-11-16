using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InstancePlayer : MonoBehaviour {

	//public Transform carPrefab;
	public List<Transform> carPrefabs;
	public Vector3 startPos;
	

	public void StartGame() {
		int i = 0;
		foreach(Transform car in carPrefabs) {
			Instantiate(car,startPos + new Vector3((float) i,0,0),Quaternion.identity);
			i++;
		}
		
	}

	public void StopGame() {
		GameObject [] destObj = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject car in destObj) {
			Destroy(car);
		}
		

	}
	
	
}
