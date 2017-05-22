using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class buildLogic : MonoBehaviour {

	public bool selectState = false;
	public bool built = false;
	public int playerID;
	public int outpostID;
	public int hitPoints;
	public GameObject selectIcon;
	private GameObject activeIcon;
	public float iconHeight = 30.0f;
	public int buildProgress = 0;
	public int buildTotal = 200;
	public GameObject finalBuild;
	public GameObject buildMesh;
	public GameObject explosion;
	public Vector3 interfacePoint;
	public GameObject interfaceMarker;
	public bool activeOnLoad = false;

	public enum Type {
		Standard,
		Factory,
		Upgradeable,
		Refinery,
		Research
	}

	public Type _type;

	void OnEnable(){
		eventManager.onStructureSelect += onSelect;
		eventManager.onDamage += takeDamage;
		eventManager.onRepair += repair;
		eventManager.onClearSelect += clearSelect;
		eventManager.onConstruction += structProgress;
	}

	void OnDisable(){
		eventManager.onStructureSelect -= onSelect;
		eventManager.onDamage -= takeDamage;
		eventManager.onRepair -= repair;
		eventManager.onClearSelect -= clearSelect;
		eventManager.onConstruction -= structProgress;
	}

	// Use this for initialization
	void Start () {
		if (activeOnLoad != true) {
			finalBuild.SetActive (false);
			tag = ("betaStructure");
			eventManager.BuildInit (transform.position, 2);
		} else {
			Collider col = GetComponent<Collider> ();
			col.isTrigger = false;
			this.gameObject.layer = 9;
			var myBounds = new GraphUpdateObject(col.bounds); 
			AstarPath.active.UpdateGraphs (myBounds);
		}
		interfacePoint = interfaceMarker.transform.position;
		eventManager.BuildInit (transform.position, 1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void onSelect (GameObject selected) {
		if (selected == this.gameObject) {
			Debug.Log ("SMSO");
			selectState = true;
			if (activeIcon == null) {
				activeIcon = Instantiate (selectIcon, transform.position, transform.rotation);
				activeIcon.transform.Translate (0, iconHeight, 0);
				activeIcon.transform.parent = transform;
			} 
			else {
				activeIcon.SetActive (true);
			}
			if(_type == Type.Factory){
			eventManager.FactorySelect (this.gameObject);
			}
			// many other ifs
	}
	}
	void clearSelect (GameObject selected) {
		if (selected == true) {
			selectState = false;
			if (activeIcon != null) {
				activeIcon.SetActive (false);
			}
		}
	}
	void structProgress(int amount, GameObject sender, GameObject reciever){
		if (reciever == this.gameObject) {

			buildProgress += amount;
			if (buildProgress >= buildTotal) {
				buildComplete ();
			}
		}
	}

	void buildComplete(){
		finalBuild.SetActive(true);
		built = true;
		DestroyObject (buildMesh);
		tag = "Structure";
		eventManager.BuildInit (transform.position, 2);
		eventManager.BuildInit (transform.position, 1);
		eventManager.onConstruction -= structProgress;
		Collider col = GetComponent<Collider> ();
		col.isTrigger = false;
		this.gameObject.layer = 9;
		var myBounds = new GraphUpdateObject(GetComponent<Collider>().bounds); 
		AstarPath.active.UpdateGraphs (myBounds);

	}
	void takeDamage(int amount, GameObject sender, GameObject reciever){
		if (reciever == this.gameObject) {
			hitPoints -= amount;
			if (hitPoints < 1) {
				boomDem ();}
		}
	}
	void repair(int amount, GameObject sender, GameObject reciever){
	}
	void boomDem(){
		
		var myBounds = new GraphUpdateObject(GetComponent<Collider>().bounds); 
		Collider temp = GetComponent<Collider> ();
		temp.isTrigger = true;
		AstarPath.active.UpdateGraphs (myBounds);
		eventManager.BuildInit (transform.position, 2);
		Vector3 tempRotation = new Vector3(Random.Range(0,360),Random.Range(0,360),Random.Range(0,360));
		tempRotation.x = 0;
		tempRotation.z = 0;
		Instantiate(explosion, transform.position, transform.rotation);
		Destroy(this.gameObject, 1);
	}
}