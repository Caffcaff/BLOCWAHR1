using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class factoryManager : MonoBehaviour {

	public Structures bFactory;
	public buildQueue[] bList;
	private resourceManager rManager;


	// Use this for initialization
	void Start () {
		bFactory = new Structures (1, 1, 8, transform.position, transform.rotation, 1, bList);
		GameObject temp = GameObject.FindGameObjectWithTag ("resourceManager");
		rManager = temp.GetComponent<> (resourceManager);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
