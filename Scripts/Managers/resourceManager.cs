using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resourceManager : MonoBehaviour {

	public int playerResource;
	public int playerGold;
	public int NPCResource;
	public int NPCGold;


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
		if (type == 3) {
			NPCResource -= amount;
		}
		if (type == 5) {
			NPCGold -= amount;
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
		if (type == 3) {
			NPCResource += amount;
		}
		if (type == 5) {
			NPCGold += amount;
		}
	}
}
