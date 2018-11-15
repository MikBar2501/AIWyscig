using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InstancePlayer : MonoBehaviour {

	public Transform carPrefab;
	public Vector3 startPos;
	

	public void StartGame() {
		Instantiate(carPrefab,startPos,Quaternion.identity);
	}

	public void StopGame() {
		GameObject destObj = GameObject.FindGameObjectWithTag("Player");
		Destroy(destObj);

	}
	
	
}
