using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resourceManager : MonoBehaviour {

	public int playerResource;
	public int playerGold;

	void OnEnable(){
		eventManager.onCollect += collect;
		eventManager.onSpend += spend;
	}

	void OnDisable(){
		eventManager.onCollect -= collect;
		eventManager.onSpend -= spend;
	}

	// Use this for initialization
	void spend (int amount, int type) {
		if (type == 1) {
			playerResource -= amount;
		}
		if (type == 2) {
			playerGold -= amount;
		}
	}
	
	// Update is called once per frame
	void collect (int amount, int type) {
		if (type == 1) {
			playerResource += amount;
		}
		if (type == 2) {
			playerGold += amount;
		}
	}
	void buildConfirm(Vector3 point, int type){
		// Something
	}
}
