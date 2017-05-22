using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallLogic : MonoBehaviour {

	void OnEnable(){
		
		eventManager.onConfirmWall += bLogic;

	//	Debug.Log("OnEnabled", gameObject);

	}

	void OnDisable(){

		eventManager.onConfirmWall -= bLogic;

	//	Debug.Log("OFF", gameObject);

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void bLogic (GameObject build) {
		if (build == this.gameObject) {
			buildLogic logic = GetComponent<buildLogic> ();
			logic.enabled = true;
		}
	}
}
