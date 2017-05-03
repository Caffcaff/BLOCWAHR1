using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitSpawner : MonoBehaviour {

	public GameObject actors;
	public GameObject resources;
	public GameObject structures;

	void OnEnable(){
		eventManager.onUnitSpawn += onSpawn;
	}
	void OnDisable(){
		eventManager.onUnitSpawn -= onSpawn;
	}

	void Start(){
		actors = GameObject.Find ("Actors");
		resources = GameObject.Find ("levelResources");
		structures = GameObject.Find ("Structures");
	}
	
	// Update is called once per frame
	void onSpawn (GameObject prefab, Vector3 location) {
		GameObject spawnee = Instantiate (prefab, location, transform.rotation);
		if (spawnee.tag == "Friendly" | spawnee.tag == "NPC" | spawnee.tag == "Enemy") {
			spawnee.transform.parent = actors.transform;
		}
		if (spawnee.tag == "Resource" | spawnee.tag == "ResourceG") {
			spawnee.transform.parent = resources.transform;
		}

	}
}
