using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDistributer : MonoBehaviour {

	public GameObject resourceUnitA;
	public GameObject resourceUnitB;
	public int bias = 1;
	public float maxSize = 5;
	public float minSize = 1;
	public float spacing = 20;
	public float density = 5;
	public float spawnHeight = 10;
	private Vector3 tryPos;
	private Vector3 target;

	public float gridX;
	public float gridZ;

	public float minAltitude = 10;
	public float maxAltitude = 60;

	public bool active = true;


	// Use this for initialization
	void Start () {
		target = new Vector3 ((transform.position.x - (gridX/2)), transform.position.y, (transform.position.z + (gridZ/2)));
		target.y = Terrain.activeTerrain.SampleHeight (target);
	}
	
	// Update is called once per frame
	void Update () {
		if (active) {
			distro ();
			}
	}
	void distro(){
		float terrainHeight = Terrain.activeTerrain.SampleHeight(target);
		if (target.x < (transform.position.x + (gridX / 2))) {
			target.y = Terrain.activeTerrain.SampleHeight (target);
			target.x += spacing;
		} else {
			target.z -= spacing;
			target.x = (transform.position.x - (gridX/2));
			target.y = Terrain.activeTerrain.SampleHeight (target);
		}
		if (target.z <= (transform.position.z - (gridZ/2))) {
			target.y = Terrain.activeTerrain.SampleHeight (target);
			active = false;
		}
		if(terrainHeight > minAltitude && terrainHeight < maxAltitude){
			tumbler();
		}


	}

	void tumbler(){
		Debug.Log ("tumbler");
		GameObject type;
		float rand = Random.Range(0,10);
		if (rand < bias) {
			type = resourceUnitA;
		} else {
			type = resourceUnitB;
		}
		if (rand < density) {
			Spawn (type);
		}
	}

	void Spawn(GameObject unit){
		Vector3 heightBuffer = new Vector3(target.x, (target.y+spawnHeight), target.z);
		Vector3 rotate = new Vector3 (0, Random.Range (0, 120), 0);
		Quaternion qrotate = Quaternion.Euler(rotate.x, rotate.y, rotate.z);
		GameObject newSpawn = Instantiate(unit, heightBuffer, qrotate);
		float scale = Random.Range(minSize, maxSize);
		Vector3 scaleTo = new Vector3 (scale, scale, scale);
		newSpawn.transform.localScale = scaleTo;
	}
}
