using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class updateNavMesh : MonoBehaviour {

	public float updateInterval;
	private float interval;

	void Start(){
		interval = updateInterval;
		updateMesh ();
	}

	void Update () {
		updateInterval -= Time.deltaTime;
		if (updateInterval < 0) {
			updateInterval = interval;
			updateMesh ();
	}
}
	void updateMesh(){
		var myBounds = new GraphUpdateObject (GetComponent<Collider> ().bounds); 
		AstarPath.active.UpdateGraphs (myBounds);
	}
}
