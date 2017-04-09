using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class navController : MonoBehaviour {

	public GameObject navHalo;
	public float spread =5;
	public float minScatter = 3f;
	public selectionManager sManager;
	public List<Vector3> currArray = new List<Vector3>();
	public float delta;

	void OnEnable(){
		eventManager.onNavClick += navArray;
	}

	void OnDisable(){
		eventManager.onNavClick -= navArray;
	}

	// Use this for initialization
	void Start () {
		if (sManager == null) {
			GameObject SMObj = GameObject.FindGameObjectWithTag ("SelectionManager");
			sManager = SMObj.GetComponent<selectionManager>();
		}
	}
	
	// Update is called once per frame
	void navArray (Vector3 point, GameObject actor) {
		currArray.Clear();
		foreach (GameObject Unit in sManager.currentSelection) {	
			if (sManager.currentSelection.Count > 1) { 
				reGen:
				Vector3 randN = new Vector3 ((Random.Range (-spread, spread)), point.y, (Random.Range (-spread, spread)));
				Vector3 arrayPos = randN + point;
				if (currArray.Count != 0) {
					foreach (Vector3 navPoint in currArray) {
						delta = Vector3.Distance (arrayPos, navPoint);
						if (delta < minScatter) {
							Debug.Log ("Too Close - Finding new navpoint");
							goto reGen;
						} else {
							eventManager.ParticleEvent (arrayPos, arrayPos, 3);
							eventManager.NavArray (arrayPos, Unit);
						}
					}
				} else {
					eventManager.ParticleEvent (arrayPos, arrayPos, 3);
					eventManager.NavArray (arrayPos, Unit);
				}
				currArray.Add (arrayPos);		
			} else {
				eventManager.ParticleEvent (point, point, 3);
				eventManager.NavArray (point, Unit);
			}
		}
	
	}
}	
