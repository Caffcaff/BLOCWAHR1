using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectionManager : MonoBehaviour {

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
		unitsInPlay.AddRange(GameObject.FindGameObjectsWithTag("Friendly"));
		reCacheStructs (transform.position, 1);
		reCacheStructs (transform.position, 2);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void unitSelection (GameObject selected) {
		currentSelection.Clear();
		currentSelection.Add(selected);
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
				currentSelection.Add(Unit);
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
		unitsInPlay.AddRange(GameObject.FindGameObjectsWithTag("Friendly"));
		Debug.Log ("Units List Updated");
	}
	void reCacheStructs (Vector3 position, int type)
	{
		if (type == 2) {
			structsInPlay.Clear ();
			structsInPlay.AddRange(GameObject.FindGameObjectsWithTag ("Structure"));
			Debug.Log ("Structs List Updated");
		}
		if (type == 1) {
			betaStructs.Clear ();
			betaStructs.AddRange(GameObject.FindGameObjectsWithTag ("betaStructure"));
			Debug.Log ("BetaStructs List Updated");
		}
	}
}
