using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitIndex : MonoBehaviour {

	public string unitTag = "Friendly";
	public string structTag = "Structure";
	public string betaStructTag = "betaStructure";
	public string resourceTag = "Resource";

	public bool Init = false;

	private bool structInit = false;
	private bool unitInit = false;
	private bool specialInit = false;

	public bool debugActive = false;

	public List<List<GameObject>> units = new List<List<GameObject>>();
	public List<List<GameObject>> hostiles = new List<List<GameObject>>();

	public List<List<GameObject>> structs = new List<List<GameObject>>();
	public List<List<GameObject>> betaStructs = new List<List<GameObject>>();

	public List<GameObject> resources = new List<GameObject> ();

	public List<List<GameObject>> refineries = new List<List<GameObject>>();
	public List<List<GameObject>> sentries = new List<List<GameObject>>();
	public List<List<GameObject>> silos = new List<List<GameObject>>();


	void OnEnable(){
		eventManager.onUnitCreate += unitReCache;
		eventManager.onBuildInit += structReCache;
		eventManager.onUnitDestroy += unitReCache;
		eventManager.onUnitSpawn += onSpawn;

	}

	void OnDisable(){
		eventManager.onUnitCreate -= unitReCache;
		eventManager.onBuildInit -= structReCache;
		eventManager.onUnitDestroy -= unitReCache;
		eventManager.onUnitSpawn -= onSpawn;
	}


	void Start(){

		unitReCache (this.gameObject, true);
		structReCache (transform.position, 1);
		resourceCache ();

	}

	void resourceCache(){

		resources.Clear ();
		resources.AddRange(GameObject.FindGameObjectsWithTag(resourceTag));
		Debug.Log("Resources Cached: " + resources.Count);
		eventManager.ListInit (1);
	}

	// Use this for initialization
	void unitReCache (GameObject Unit, bool state) {

		List<GameObject> allUnits = new List<GameObject> ();

		List<GameObject> Units0 = new List<GameObject> ();
		List<GameObject> Units1 = new List<GameObject> ();
		List<GameObject> Units2 = new List<GameObject> ();
		List<GameObject> Units3 = new List<GameObject> ();
		List<GameObject> Units4 = new List<GameObject> ();

		List<GameObject> hostiles0 = new List<GameObject> ();
		List<GameObject> hostiles1 = new List<GameObject> ();
		List<GameObject> hostiles2 = new List<GameObject> ();
		List<GameObject> hostiles3 = new List<GameObject> ();
		List<GameObject> hostiles4 = new List<GameObject> ();

		List<List<GameObject>> tempUnits = new List<List<GameObject>>();
		List<List<GameObject>> tempHostiles = new List<List<GameObject>>();

		tempUnits.Add (Units0);
		tempUnits.Add (Units1);
		tempUnits.Add (Units2);
		tempUnits.Add (Units3);
		tempUnits.Add (Units4);

		tempHostiles.Add (hostiles0);
		tempHostiles.Add (hostiles1);
		tempHostiles.Add (hostiles2);
		tempHostiles.Add (hostiles3);
		tempHostiles.Add (hostiles4);

		allUnits.AddRange (GameObject.FindGameObjectsWithTag (unitTag));

		foreach (GameObject unit in allUnits) {
			
			if (unit.GetComponent<unitAgent>() != null) {

				int i = 0;

				while (i < 5) {

					if (unit.GetComponent<unitAgent> ().playerID == i) {
						tempUnits [i].Add (unit);
					} else {
						tempHostiles[i].Add(unit);
					}
					i++;
				}
			}
		}

		units.Clear();
		units = tempUnits;

		hostiles.Clear();
		hostiles = tempHostiles;

		unitInit = true;

		if (structInit && unitInit && specialInit) {
			Init = true;
		}

		eventManager.UnitCache (5);
	}
	
	// Update is called once per frame
	void structReCache (Vector3 position, int type) {

		List<GameObject> allstructs = new List<GameObject> ();
		List<GameObject> structs0 = new List<GameObject> ();
		List<GameObject> structs1 = new List<GameObject> ();
		List<GameObject> structs2 = new List<GameObject> ();
		List<GameObject> structs3 = new List<GameObject> ();
		List<GameObject> structs4 = new List<GameObject> ();

		List<List<GameObject>> tempStructs = new List<List<GameObject>> ();

		tempStructs.Add (structs0);
		tempStructs.Add (structs1);
		tempStructs.Add (structs2);
		tempStructs.Add (structs3);
		tempStructs.Add (structs4);

		List<GameObject> betaStructs0 = new List<GameObject> ();
		List<GameObject> betaStructs1 = new List<GameObject> ();
		List<GameObject> betaStructs2 = new List<GameObject> ();
		List<GameObject> betaStructs3 = new List<GameObject> ();
		List<GameObject> betaStructs4 = new List<GameObject> ();

		List<List<GameObject>> tempBetaStructs = new List<List<GameObject>> ();

		tempBetaStructs.Add (betaStructs0);
		tempBetaStructs.Add (betaStructs1);
		tempBetaStructs.Add (betaStructs2);
		tempBetaStructs.Add (betaStructs3);
		tempBetaStructs.Add (betaStructs4);

		allstructs.AddRange (GameObject.FindGameObjectsWithTag (structTag));
		allstructs.AddRange (GameObject.FindGameObjectsWithTag (betaStructTag));

		foreach (GameObject unit in allstructs) {

			if (unit.GetComponent<buildLogic>() != null) {

				int i = 0;

				while (i < 5) {

					if (unit.GetComponent<buildLogic> ().playerID == i) {
						if (unit.tag == structTag) {
							tempStructs[i].Add (unit);
						} else {
							tempBetaStructs[i].Add (unit);
						}
					}
					i++;
				}
			}
		}

		structs.Clear ();
		structs = tempStructs;

		betaStructs.Clear ();
		betaStructs = tempBetaStructs;

		if (debugActive) {

			Debug.Log ("Structs 0: " + structs [0].Count + " betaStructs:" + betaStructs [0].Count);
			Debug.Log ("Structs 1: " + structs [1].Count + " betaStructs:" + betaStructs [1].Count);
			Debug.Log ("Structs 2: " + structs [2].Count + " betaStructs:" + betaStructs [2].Count);
			Debug.Log ("Structs 3: " + structs [3].Count + " betaStructs:" + betaStructs [3].Count);
		}

		structInit = true;

		if (structInit && unitInit && specialInit) {
			Init = true;
		}

		specialStructs ();
	}

	void specialStructs(){

		List<GameObject> silo0 = new List<GameObject> ();
		List<GameObject> silo1 = new List<GameObject> ();
		List<GameObject> silo2 = new List<GameObject> ();
		List<GameObject> silo3 = new List<GameObject> ();
		List<GameObject> silo4 = new List<GameObject> ();

		List<List<GameObject>> tempSilos = new List<List<GameObject>> ();


		tempSilos.Add (silo0);
		tempSilos.Add (silo1);
		tempSilos.Add (silo2);
		tempSilos.Add (silo3);
		tempSilos.Add (silo4);

		List<GameObject> tower0 = new List<GameObject> ();
		List<GameObject> tower1 = new List<GameObject> ();
		List<GameObject> tower2 = new List<GameObject> ();
		List<GameObject> tower3 = new List<GameObject> ();
		List<GameObject> tower4 = new List<GameObject> ();

		List<List<GameObject>> tempSentries = new List<List<GameObject>> ();

		tempSentries.Add (tower0);
		tempSentries.Add (tower1);
		tempSentries.Add (tower2);
		tempSentries.Add (tower3);
		tempSentries.Add (tower4);

		List<GameObject> refinery0 = new List<GameObject> ();
		List<GameObject> refinery1 = new List<GameObject> ();
		List<GameObject> refinery2 = new List<GameObject> ();
		List<GameObject> refinery3 = new List<GameObject> ();
		List<GameObject> refinery4 = new List<GameObject> ();

		List<List<GameObject>> tempRefineries = new List<List<GameObject>> ();

		tempRefineries.Add (refinery0);
		tempRefineries.Add (refinery1);
		tempRefineries.Add (refinery2);
		tempRefineries.Add (refinery3);
		tempRefineries.Add (refinery4);

		int i = 0;

		while (i < structs.Count){

			foreach(GameObject unit in structs[i]){

				if (unit.GetComponent<buildLogic> ()._type == buildLogic.Type.Refinery) {
					tempRefineries [i].Add (unit);
					if (debugActive)
						Debug.Log ("Refinery++" + "Player: " + i);
				}
				if (unit.GetComponent<buildLogic> ()._type == buildLogic.Type.Silo) {
					tempSilos [i].Add (unit);
					if (debugActive)
						Debug.Log ("Silo++" + "Player: " + i);
				}
				if (unit.GetComponent<buildLogic> ()._type == buildLogic.Type.Sentry) {
					tempSentries [i].Add (unit);
					if (debugActive)
						Debug.Log ("Tower++" + "Player: " + i);
				}
			}

			i++;

		}

		silos.Clear ();
		silos = tempSilos;

		sentries.Clear ();
		sentries = tempSentries;

		refineries.Clear ();
		refineries = tempRefineries;

		if (debugActive) {
			
			Debug.Log ("Special Structs 0 Updated ** Silos:" + silos [0].Count + " Towers:" + sentries [0].Count + " Refineries:" + refineries [0].Count);
			Debug.Log ("Special Structs 1 Updated ** Silos:" + silos [1].Count + " Towers:" + sentries [1].Count + " Refineries:" + refineries [1].Count);
			Debug.Log ("Special Structs 2 Updated ** Silos:" + silos [2].Count + " Towers:" + sentries [2].Count + " Refineries:" + refineries [2].Count);
			Debug.Log ("Special Structs 3 Updated ** Silos:" + silos [3].Count + " Towers:" + sentries [3].Count + " Refineries:" + refineries [3].Count);
		}

		specialInit = true;

		if (structInit && unitInit && specialInit) {
			Init = true;
		}
	}

	void onSpawn (GameObject prefab, Vector3 pos, int ID){

		unitReCache (this.gameObject, true);

	}
}
