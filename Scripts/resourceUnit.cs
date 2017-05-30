using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class resourceUnit : MonoBehaviour {

	public int resourceValue = 100;
	private int resourceReset = 100;
	public float mini = 0.05f;
	public GameObject debris;
	public float debrisRate;
	public float debrisScale = 100;
	private Vector3 randScale;
	public int repeat = 2;
	public int damageRate = 5;
	public bool cutOff = true;
	public float cutOffHeight = 3;

	void OnEnable(){
		eventManager.onDamage += miningEvent;
	}

	void OnDisable(){
		eventManager.onDamage -= miningEvent;
	}

	// Use this for initialization
	void Start () {
		if (Terrain.activeTerrain.SampleHeight (transform.position) > transform.position.y) {
			GameObject.Destroy (this.gameObject);
		}

		
		
	}
	
	// Update is called once per frame
	void Update () {
		if (cutOff == true){
			if (transform.position.y < cutOffHeight) {
				Destroy (this.gameObject, 1);
			}
		}
	}
	void miningEvent(int hitpoints, GameObject sender, GameObject reciever){
		cutOff = false;
		
		if (reciever == this.gameObject) {
			unitAgent tempUnit = sender.GetComponent<unitAgent> ();
			if (tempUnit._type == unitAgent.Type.miner) {
				float rand = Random.Range (0, 10);
				if (rand < debrisRate) {
					GameObject temp = Instantiate (debris, transform.position, Random.rotation);
					debrisLogic tempLogic = temp.GetComponent<debrisLogic> ();
					tempLogic.Target = sender;
					temp.transform.parent = transform;
					Vector3 randScale = new Vector3 (rand / debrisScale, rand / debrisScale, rand / debrisScale);
					temp.transform.localScale = temp.transform.localScale + randScale;
				}
				resourceValue = (resourceValue - hitpoints);
				if (resourceValue > 50) {
					transform.localScale -= new Vector3 (0.02f, 0.02f, 0.02f);
					Debug.Log ("Damage Recieved");
				}
				if (resourceValue < 50 && resourceValue > 2) {
					//float mini = (float)hitpoints / resourceReset;
					int i = 1;
					transform.localScale -= new Vector3 (0.03f, 0.03f, 0.03f);
					if (i == 1) {
						var myBounds = new GraphUpdateObject (GetComponent<Collider> ().bounds); 
						AstarPath.active.UpdateGraphs (myBounds);
						i++;
					}
					
				}
				if (resourceValue <= 0) {
					onDamage ();
					onDeplete ();

				}
			}
		}
	}
	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Floor") {
			var myBounds = new GraphUpdateObject (GetComponent<Collider> ().bounds); 
			AstarPath.active.UpdateGraphs (myBounds);
		}
		if (col.gameObject.tag == "missile" | col.gameObject.tag == "tracer") {
			onDamage ();
			emitDebris ();
		}
	}
	void OnCollisionExit(Collision col){
		if (col.gameObject.tag == "Floor") {
			var myBounds = new GraphUpdateObject (GetComponent<Collider> ().bounds); 
			AstarPath.active.UpdateGraphs (myBounds);
		}
	}
	void emitDebris(){
		int i = 0;
		float rand = Random.Range (0, 10);
		repeat =  (int)rand;
		while (i < repeat) {
			GameObject temp = Instantiate (debris, transform.position, Random.rotation);
			debrisLogic tempLogic = temp.GetComponent<debrisLogic> ();
			tempLogic.speed = 10 * ((rand / 5) + 1);
			tempLogic.timer = 1 /((rand/3)+1);
			Vector3 randScale = new Vector3 (rand / 500, rand / 500, rand / 500);
			temp.transform.localScale = temp.transform.localScale + randScale;
			i++;
		}
		if (resourceValue <= 0) {
			onDeplete ();
		}
	}
	void onDeplete(){
		gameObject.layer = 1;
		var myBounds = new GraphUpdateObject (GetComponent<Collider> ().bounds); 
		AstarPath.active.UpdateGraphs (myBounds);
		eventManager.UnitDestroy (this.gameObject, true);
		Destroy (gameObject, 1);
	}
	void onDamage(){
		resourceValue = (resourceValue - damageRate);
		if (resourceValue > 50) {
			transform.localScale -= new Vector3 (0.02f, 0.02f, 0.02f);
			Debug.Log ("Damage Recieved");
		}
		if (resourceValue < 50 && resourceValue > 2) {
			//float mini = (float)hitpoints / resourceReset;
			int i = 1;
			transform.localScale -= new Vector3 (0.03f, 0.03f, 0.03f);
			if (i == 1) {
				var myBounds = new GraphUpdateObject (GetComponent<Collider> ().bounds); 
				AstarPath.active.UpdateGraphs (myBounds);
				i++;
			}
		}
	}
}
