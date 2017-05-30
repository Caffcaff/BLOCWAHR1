using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectionManager : MonoBehaviour {

	public int playerID = 1;
	public bool debugActive = true;

	public List<GameObject> unitsInPlay = new List<GameObject>();
	public List<GameObject> structsInPlay = new List<GameObject>();
	public List<GameObject> betaStructs = new List<GameObject>();
	public List<GameObject> currentSelection = new List<GameObject>();
	public List<GameObject> selectGroup = new List<GameObject>();

	public GameObject gManager;

	void OnEnable(){

		eventManager.onUnitCache += reCacheUnits;
		eventManager.onUnitSelect += unitSelection;
		eventManager.onGroupSelect += groupSelection;
		eventManager.onTypeSelect += typeSelection;
		eventManager.onClearSelect += clearSelection;

		eventManager.onAttackClick += onAttackClick;
		eventManager.onNavClick += onNavClick;
		eventManager.onServePatrol += onPatrol;

	}

	void OnDisable(){

		eventManager.onUnitCache -= reCacheUnits;
		eventManager.onUnitSelect -= unitSelection;
		eventManager.onGroupSelect -= groupSelection;
		eventManager.onTypeSelect -= typeSelection;
		eventManager.onClearSelect -= clearSelection;

		eventManager.onAttackClick += onAttackClick;
		eventManager.onNavClick += onNavClick;
		eventManager.onServePatrol += onPatrol;

	}
	// Use this for initialization
	void Start () {

		gManager = GameObject.Find("gameManager");

		GameObject temp = GameObject.FindGameObjectWithTag ("playerSeed");
		playerID = temp.GetComponent<playerCommand>().playerID;

		List<GameObject> tempList = new List<GameObject>();
		tempList.AddRange(GameObject.FindGameObjectsWithTag("Friendly"));

		foreach(GameObject unit in tempList){
			unitAgent ai = unit.GetComponent<unitAgent> ();
			if (ai.playerID == playerID) {
				unitsInPlay.Add (unit);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	void unitSelection (GameObject selected, int ID) {
		currentSelection.Clear ();
		unitAgent logic = selected.GetComponent<unitAgent> ();

		if (logic.playerID == playerID) {
			currentSelection.Add (selected);
		}
		eventManager.SelectEvent (this.gameObject, playerID);
		if (debugActive) {
			Debug.Log ("Units Selected");
		}
	}

	void groupSelection (Vector2 RectStart, Vector2 RectEnd, GameObject selected, int Type) {
		currentSelection.Clear();
		if (debugActive) {
			Debug.Log ("GROUP SELECT CALLED");
		}

		Rect aimRect = new Rect(Mathf.Min(RectEnd.x, RectStart.x), Mathf.Min(RectEnd.y, RectStart.y), Mathf.Abs(RectEnd.x - RectStart.x), Mathf.Abs(RectEnd.y - RectStart.y));


		foreach (GameObject Unit in unitsInPlay) {
			if (Unit != null) {
				Vector3 pos = Camera.main.WorldToScreenPoint (Unit.transform.position);
				//pos.y = Screen.height - pos.y;

				if (aimRect.Contains (pos)) {
					unitAgent logic = Unit.GetComponent<unitAgent> ();
					if (logic.playerID == playerID) {
						currentSelection.Add (Unit);
					}
				}
			}
		
		}
		if (debugActive) {
			Debug.Log ("Units Selected");
		}
		setUnitSelects ();
	}

	void typeSelection (string type) {
		currentSelection.Clear();
		if (debugActive) {
			Debug.Log ("Type SELECT CALLED");
		}

		Rect screenRect = new 
			Rect(0,0, Screen.width, Screen.height);

		foreach (GameObject Unit in unitsInPlay) {
			Vector3 pos = Camera.main.WorldToScreenPoint(Unit.transform.position);
			unitAgent mgmt = Unit.GetComponent<unitAgent> ();
			string mgType = mgmt._type.ToString ();
			if (screenRect.Contains (pos) && mgType == type) {
				currentSelection.Add(Unit);
			}
		}
		setUnitSelects ();
		if(debugActive)
			Debug.Log ("Units Selected");
	}

	void clearSelection (GameObject unused, int ID){
		currentSelection.Clear();
		selectGroup.Clear();
		eventManager.SelectEvent (this.gameObject, playerID);
		if(debugActive)
			Debug.Log ("Deselect");
	}

	void reCacheUnits (int ID) {


		if (gManager != null) {

			if (gManager.GetComponent<unitIndex> ().Init) {

				unitsInPlay = gManager.GetComponent<unitIndex> ().units [playerID];

				if (debugActive) {
					Debug.Log ("Units List Updated");
				}
			}
		}
	}

	void reCacheStructs (Vector3 position, int type)
	{

		if (gManager != null) {

			if (gManager.GetComponent<unitIndex> ().Init) {

				structsInPlay = new List<GameObject> ();
				structsInPlay = gManager.GetComponent<unitIndex> ().structs [playerID];

				betaStructs = new List<GameObject> ();
				betaStructs = gManager.GetComponent<unitIndex> ().betaStructs [playerID];

			
				Debug.Log ("BetaStructs List Updated");
			}
		}
	}

	void setUnitSelects (){

		int i = 1;

		foreach (GameObject unit in currentSelection) {
			eventManager.SelectEvent (unit, playerID);
			i++;
		}
		if (debugActive) {
			Debug.Log ("Player " + playerID + " | Selected " + i + " units.");
		}
	}


	void onAttackClick(Vector3 point, GameObject actor, int ID){
		if (ID == playerID) {
			if (currentSelection != null) {

				Order attack = new Order (Order.Type.Attack, playerID, 0);
				attack.unitTarget = actor;

				eventManager.ProcessOrder (attack, currentSelection.ToArray());

			}
		}
		
	}


	void onNavClick(Vector3 point, GameObject actor, int ID){

		if (ID == playerID) {
	
			if (currentSelection != null) {

				Order move = new Order (Order.Type.Move, playerID, 0);
				move.navTarget = point;

				eventManager.ProcessOrder (move, currentSelection.ToArray());

				}

			}
		}

	void onPatrol(Vector3 pointA, Vector3 pointB, int ID ){

		if (ID == playerID) {

			if (currentSelection != null) {

				Order patrol = new Order (Order.Type.Patrol, playerID, 0);
				patrol.patrolA = pointA;
				patrol.patrolB = pointB;

				foreach (GameObject unit in currentSelection) {

					eventManager.ServeOrder (patrol, unit, 0, playerID);

				}

			}
		}
	}

}
