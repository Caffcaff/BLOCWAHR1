using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resourceUnit : MonoBehaviour {

	public int resourceValue = 100;
	private int resourceReset = 100;
	public float mini = 0.05f;

	void OnEnable(){
		eventManager.onDamage += miningEvent;
	}

	void OnDisable(){
		eventManager.onDamage -= miningEvent;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void miningEvent(int hitpoints, GameObject sender, GameObject reciever){
		if (reciever == this.gameObject) {
			resourceValue = (resourceValue - hitpoints);
			if (resourceValue > 50) {
				transform.localScale -= new Vector3 (0.02f, 0.02f, 0.02f);
				Debug.Log ("Damage Recieved");
			}
				if (resourceValue <50 && resourceValue >2) {
					//float mini = (float)hitpoints / resourceReset;
					transform.localScale -= new Vector3(0.03f,0.03f,0.03f);
					Debug.Log ("Damage Recieved");
			}
			if (resourceValue <= 0) {
				eventManager.UnitDestroy (this.gameObject, true);
				Destroy(gameObject, 1);
				}
		}
	}
}




