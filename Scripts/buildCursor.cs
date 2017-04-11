using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildCursor : MonoBehaviour {

	public buildManager bManager;
	public bool hasCollision = false;

	// Use this for initialization

	void Start() {
		GameObject temp = GameObject.FindGameObjectWithTag ("buildManager");
		bManager = temp.GetComponent<buildManager> ();
	}

	void FixedUpdate () {
		bManager.hasCollision = hasCollision;
	}
	
	void OnTriggerEnter(Collider col){
		Debug.Log ("Collision");
			if (col.gameObject.tag == "Friendly" | col.gameObject.tag == "Structure") {
				hasCollision = true;
		}
	}
	void OnTriggerExit(Collider col){
		Debug.Log ("onExit");
		if (col.gameObject.tag == "Friendly" | col.gameObject.tag == "Structure") {
			hasCollision = false;
		}
	}
}