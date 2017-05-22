using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectionManager : MonoBehaviour {

	public int playerID = 1;

	public List<GameObject> unitsInPlay = new List<GameObject>();
	public List<GameObject> structsInPlay = new List<GameObject>();
	public List<GameObject> betaStructs = new List<GameObject>();
	public List<GameObject> currentSelection = new List<GameObject>();
	public List<GameObject> selectGroup = new List<GameObject>();

	void OnEnable(){
		eventManager.onUnitCreate += reCacheUnits;
		eventManager.onBuildInit += reCacheStructs;
		eventManager.onUnitDestroy += reCacheUnits;
		eventManager.onUnitSelect += unitSelection;
		eventManager.onGroupSelect += groupSelection;
		eventManager.onTypeSelect += typeSelection;
		eventManager.onClearSelect += clearSelection;

	}

	void OnDisable(){
		eventManager.onUnitCreate -= reCacheUnits;
		eventManager.onBuildInit -= reCacheStructs;
		eventManager.onUnitDestroy -= reCacheUnits;
		eventManager.onUnitSelect -= unitSelection;
		eventManager.onGroupSelect -= groupSelection;
		eventManager.onTypeSelect -= typeSelection;
		eventManager.onClearSelect -= clearSelection;

	}
	// Use this for initialization
	void Start () {

		GameObject temp = GameObject.FindGameObjectWithTag ("playerSeed");
		playerID = temp.GetComponent<playerCommand>().playerID;

		List<GameObject> tempList = new List<GameObject>();
		tempList.AddRange(GameObject.FindGameObjectsWithTag("Friendly"));

		foreach(GameObject unit in tempList){
			unitLogic ai = unit.GetComponent<unitLogic> ();
			if (ai.playerID == playerID) {
				unitsInPlay.Add (unit);
			}
		}

		reCacheStructs (transform.position, 1);
		reCacheStructs (transform.position, 2);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void unitSelection (GameObject selected) {
		currentSelection.Clear();
		unitLogic logic = selected.GetComponent<unitLogic>();
		if (logic.playerID == playerID) {
			currentSelection.Add(selected);
		}
		eventManager.SelectEvent (this.gameObject);
		Debug.Log ("Units Selected");
	}

	void groupSelection (Vector2 RectStart, Vector2 RectEnd, GameObject selected, int Type) {
		currentSelection.Clear();
		Debug.Log ("GROUP SELECT CALLED");

		Rect aimRect = new Rect(Mathf.Min(RectEnd.x, RectStart.x), Mathf.Min(RectEnd.y, RectStart.y), Mathf.Abs(RectEnd.x - RectStart.x), Mathf.Abs(RectEnd.y - RectStart.y));


		foreach (GameObject Unit in unitsInPlay) {
			Vector3 pos = Camera.main.WorldToScreenPoint(Unit.transform.position);
			//pos.y = Screen.height - pos.y;

			if (aimRect.Contains (pos)) {
				unitLogic logic = Unit.GetComponent<unitLogic> ();
				if (logic.playerID == playerID) {
					currentSelection.Add(Unit);
				}
			}
		
		}
		Debug.Log ("Units Selected");
		eventManager.SelectEvent (this.gameObject);
	}

	void typeSelection (string type) {
		currentSelection.Clear();
		Debug.Log ("Type SELECT CALLED");

		Rect screenRect = new 
			Rect(0,0, Screen.width, Screen.height);

		foreach (GameObject Unit in unitsInPlay) {
			Vector3 pos = Camera.main.WorldToScreenPoint(Unit.transform.position);
			unitLogic mgmt = Unit.GetComponent<unitLogic> ();
			string mgType = mgmt._type.ToString ();
			if (screenRect.Contains (pos) && mgType == type) {
				currentSelection.Add(Unit);
			}
		}
		Debug.Log ("Units Selected");
		eventManager.SelectEvent (this.gameObject);
	}

	void clearSelection (GameObject unused){
		currentSelection.Clear();
		selectGroup.Clear();
		Debug.Log ("Deselect");
		eventManager.SelectEvent (this.gameObject);
	}

	void reCacheUnits (GameObject unit, bool state) {
		unitsInPlay.Clear();

		List<GameObject> tempList = new List <GameObject> ();
		tempList.AddRange(GameObject.FindGameObjectsWithTag("Friendly"));

		foreach(GameObject Unit in tempList){
			unitLogic ai = Unit.GetComponent<unitLogic> ();
			if (ai.playerID == playerID) {
				unitsInPlay.Add (Unit);
			}
		}
		Debug.Log ("Units List Updated");
	}
	void reCacheStructs (Vector3 position, int type)
	{
		if (type == 2) {
			structsInPlay.Clear ();

			List<GameObject> tempList = new List<GameObject> ();
			tempList.AddRange(GameObject.FindGameObjectsWithTag ("Structure"));

			if (tempList.Count != 0) {

				foreach (GameObject unit in tempList) {

					if (unit.GetComponent<buildLogic> () != null) {

						if (unit.GetComponent<buildLogic> ().playerID == playerID) {
							structsInPlay.Add (unit);	
						}
					}
				}
			}

			Debug.Log ("Structs List Updated");
		}

		if (type == 1) {
			betaStructs.Clear ();

			List<GameObject> tempList = new List<GameObject> ();
			tempList.AddRange(GameObject.FindGameObjectsWithTag ("betaStructure"));

			if (tempList.Count != 0) {

				foreach (GameObject unit in tempList) {

					if (unit.GetComponent<buildLogic> () != null) {

						if (unit.GetComponent<buildLogic> ().playerID == playerID) {
							betaStructs.Add (unit);	
						}
					}
				}
			}

			Debug.Log ("BetaStructs List Updated");
		}
	}
}
