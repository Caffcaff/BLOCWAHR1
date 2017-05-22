using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resourceManager : MonoBehaviour {

	public int playerResource = 10000;
	public int initBalance = 3000;

	public List<int> resource = new List<int> ();

	void OnEnable(){
		eventManager.onCollect += collect;
		eventManager.onSpend += spend;
	}

	void OnDisable(){
		eventManager.onCollect -= collect;
		eventManager.onSpend -= spend;
	}

	void Start(){

		resource.Add (initBalance);
		resource.Add (initBalance);
		resource.Add (initBalance);
		resource.Add (initBalance);
	}

	// Use this for initialization
	void spend (int amount, int player) {

	//	playerResource -= amount;

		resource [player] -= amount;
	}
	
	// Update is called once per frame
	void collect (int amount, int player) {

	//	playerResource += amount;
		resource [player] += amount;
	}
}
