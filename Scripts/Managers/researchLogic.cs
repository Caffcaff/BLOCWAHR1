using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class researchLogic : MonoBehaviour {

	public int playerID;
	public int techLevel = 1;

	// Use this for initialization
	void Start () {
		playerID = GetComponent<buildLogic> ().playerID;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
