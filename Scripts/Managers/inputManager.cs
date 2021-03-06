﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class inputManager : MonoBehaviour {

	public int playerID = 1;

	[Header("Mouse Settings")]
	public Vector2 _box_start_pos;
	public Vector2 _box_end_pos;
	public GameObject unused;
	public int unused2 = 3;
	public Texture boxSelectTexture;
	public float dragSenstivity = 5;
	public float delta;
	public List<Vector3> patrolMem = new List<Vector3>();

	[Header("GUI Settings")]
	public GameObject GUIman;
	public GUIManager Gmgmt;
	public selectionManager sManager;
	public bool GUIActive = false;
	public bool buildActive = false;
	public bool patrolActive = false;

	[Header("Cursor Settings")]

	public GameObject patrolCursor;
	private GameObject pCursor;
	public GameObject attackCursor;
	private GameObject aCursor;
	public GameObject moveCursor;
	private GameObject mCursor;
	public GameObject negCursor;
	private GameObject nCursor;

	public enum State {
		Initialise,
		Setup,
		Idle,
		InGame,
		GUI,
		Patrol,
		Build,
		Destroy
	}

	public State _state;

	// Update is called once per frame
	void OnEnable(){
		eventManager.onButtonEnter += onGUIenter;
		eventManager.onButtonExit += onGUIexit;
		eventManager.onBuildSelect += buildSelect;
		eventManager.onBuildConfirm += buildConfirm;
		eventManager.onBuildCancel += buildCancel;
		eventManager.onPatrolEnter += patrolSwitch;
	}

	void OnDisable(){
		eventManager.onButtonEnter -= onGUIenter;
		eventManager.onButtonExit -= onGUIexit;
		eventManager.onBuildSelect -= buildSelect;
		eventManager.onBuildConfirm -= buildConfirm;
		eventManager.onBuildCancel -= buildCancel;
		eventManager.onPatrolEnter -= patrolSwitch;
	}


	IEnumerator Start(){
		_state = State.Initialise;

		while (true) {

			switch (_state) {
			case State.Initialise:
				initMe ();
				break;
			case State.Setup:
				setMeUp ();
				break;
			case State.Idle:
				idleState ();
				break;
			case State.InGame:
				inGame ();
					break;
			case State.GUI:
				inGui ();
				break;
			case State.Patrol:
				inPatrol ();
				break;
			case State.Build:
				inBuild ();
				break;
	//		case State.Destroy:
	//			inDestroy ();
	//			break;
			}
			yield return 0;
		}	
	}

	private void initMe()
	{
		Debug.Log ("Initialise");
		_state = State.Setup;
	}

	private void setMeUp(){
		GameObject temp = GameObject.FindGameObjectWithTag ("playerSeed");
		playerID = temp.GetComponent<playerCommand>().playerID;
		GameObject sTemp = GameObject.Find ("Selection Manager");
		sManager = sTemp.GetComponent<selectionManager> ();
		GUIman = GameObject.FindGameObjectWithTag ("GUIManager");
		Gmgmt = GUIman.GetComponent<GUIManager>();
		GUIActive = Gmgmt.GUIIsActive;
		_state = State.Idle;
	}

	private void idleState()
	{
		Debug.Log ("Initialise");
		_state = State.InGame;
	}

	private void inGame ()
	{
		buildActive = false;
		patrolActive = false;

		if (Input.GetKey (KeyCode.Mouse0) && GUIActive == false) {
			// Called on the first update where the user has pressed the mouse button.
			if (Input.GetKeyDown (KeyCode.Mouse0))
				_box_start_pos = Input.mousePosition;
			else  // Else we must be in "drag" mode.
				_box_end_pos = Input.mousePosition;    
		} else {
			// Handle the case where the player had been drawing a box but has now released.
			if (_box_end_pos != Vector2.zero && _box_start_pos != Vector2.zero) {
				delta = Mathf.Abs(_box_end_pos.y - _box_start_pos.y);
				if (delta > dragSenstivity) {
					eventManager.ClearSelect (unused, playerID);
					eventManager.GroupSelect(_box_start_pos,_box_end_pos,unused,unused2);
					_box_end_pos = _box_start_pos = Vector2.zero;
				}
					else {
					singleClickSelect ();
					_box_end_pos = _box_start_pos = Vector2.zero;
				}	
			}
		}
		//Mouse Button Right (Move, Attack, Mine)
		if (Input.GetButtonDown ("Fire1") == true && GUIActive ==false) {
			Debug.Log ("Pressed right click.");
			RaycastHit hit;
			var layerMask = ~(1 << 16);

			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); 
			if (Physics.Raycast (ray, out hit, 2000.0f, layerMask)) {
				if (hit.collider.tag != null) {
					if (hit.collider.tag == "Enemy" || hit.collider.tag == "Friendly" || hit.collider.tag == "NPC" || hit.collider.tag == "Structure" || hit.collider.tag == "betaStructure" || hit.collider.tag == "Resource" || hit.collider.tag == "ResourceG") {
						eventManager.AttackClick (hit.point, hit.collider.gameObject, playerID);
						Debug.Log ("Clicked Attack");
					}
					if (hit.collider.tag == "Floor" || hit.collider.tag == "Environment" || hit.collider.tag == "buildPlane") {
						Debug.Log ("Clicked nav");
						eventManager.NavClick(hit.point, hit.collider.gameObject, playerID);
					}
//					if (hit.collider.tag == "Floor" || hit.collider.tag == "Environment") {
//						eventManager.GroundAttackClick(hit.point, hit.collider.gameObject);
//						Debug.Log("Clicked Ground Attack");
//					}
				} else {
					Debug.Log ("null tag");
				}
			}	
		}
		//Mouse Button Middle (Follow)
		if (Input.GetButtonDown ("Fire2") == true && GUIActive ==false) {
			Debug.Log ("Pressed middle click.");
			RaycastHit hit; 
			var layerMask = ~(1 << 16);
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); 
			if (Physics.Raycast (ray, out hit, 1000.0f, layerMask)) {
				if (hit.collider.tag != null) {
					if (hit.collider.tag == "Enemy" || hit.collider.tag == "NPC" || hit.collider.tag == "Friendly") {
						eventManager.FollowClick (hit.point, hit.collider.gameObject, playerID);
						Debug.Log ("Clicked Actor");
					}
				}
			}	
		}
	}
		public void singleClickSelect ()
	{
		Debug.Log ("Pressed left click.");
		RaycastHit hit; 
		var layerMask = ~(1 << 16);
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); 
		if (Physics.Raycast (ray, out hit, 1000.0f, layerMask)) {
			if (hit.collider.tag != null) {
				eventManager.ClearSelect (hit.collider.gameObject, playerID);
				if (hit.collider.tag == "Friendly" | hit.collider.tag == "Structure") {
					if (hit.collider.tag == "Friendly") {
						eventManager.UnitSelect (hit.collider.gameObject, playerID);
						Debug.Log ("unit clicked");
					}
					if (hit.collider.tag == "Structure") {
						GameObject clickedStruct = hit.collider.gameObject;

						if (clickedStruct.GetComponent<buildLogic> ().playerID == playerID) {
							eventManager.StructureSelect (hit.collider.gameObject, playerID);
							Debug.Log ("Structure clicked");
						}
					}
				} else {
					Debug.Log ("Bebop");
					eventManager.ClearSelect (hit.collider.gameObject, playerID);
				}
			}
		}
	}
	void OnGUI ()
	{

		if (_box_start_pos != Vector2.zero && _box_end_pos != Vector2.zero) {
			// Create a rectangle object out of the start and end position while transforming it
			// to the screen's cordinates.
			var rect = new Rect (_box_start_pos.x, Screen.height - _box_start_pos.y,
				          _box_end_pos.x - _box_start_pos.x,
				          -1 * (_box_end_pos.y - _box_start_pos.y));
			// Draw the texture.
			GUI.DrawTexture (rect, boxSelectTexture);
		}
	}
	void onGUIenter(GameObject unused){
		if (GUIActive == false) {
			GUIActive = true;
			Debug.Log ("GUI is Active");
			if (_state == State.Build) {
				_state = State.GUI;
			} else {
				_state = State.GUI;
			}
		}
	}
	void onGUIexit(GameObject unused){
			GUIActive = false;
			Debug.Log ("GUI is Not Active");
			if (buildActive == false && patrolActive == false) {
				_state = State.InGame;
			}
			if (buildActive == true) {
				_state = State.Build;}
			if (patrolActive == true) {
			_state = State.Patrol;}
	}
	void buildSelect(Vector3 position, int type){
		_state = State.Build;
	}
	void buildConfirm(Vector3 position, int type){
		GUIActive = false;
		_state = State.InGame;
	}
	void buildCancel(Vector3 position, int type){
		_state = State.InGame;
	}
	void inGui(){
		// Something
	}
	void inBuild(){
		buildActive = true;
		patrolActive = false;

		if (Input.GetMouseButtonDown (0)) {
			eventManager.onLeftClick (transform.position);
		}
		if (Input.GetButtonDown("Fire1")) {
			eventManager.onRightClick (transform.position);
		}
		if (Input.GetButtonDown("Fire2")) {
			eventManager.onMiddleClick (transform.position);
		}
		if (Input.GetKeyDown ("escape")) {
			eventManager.onEscapeKey (transform.position);
			_state = State.InGame;
		}
	}

	void inPatrol (){

		bool canMove = true;
		RaycastHit hit;
		var layerMask = ~(1 << 16);
		Vector2 mp = Input.mousePosition;
		Ray ray = Camera.main.ScreenPointToRay (mp); 
	
		if (Physics.Raycast (ray, out hit, 2000.0f, layerMask)) {

			if (patrolMem.Count == 1 && canMove == true) {
				eventManager.ParticleEvent (patrolMem [0], hit.point, 18);
			}

			pCursor.SetActive (true);
			pCursor.transform.position = hit.point;

			GraphNode node = AstarPath.active.GetNearest (hit.point).node;

			if (node.Walkable) {
				canMove = true;
				pCursor.SetActive (true);
			} else {
				canMove = false;
				pCursor.SetActive (false);
				eventManager.ParticleEvent (hit.point, hit.point, 19);
				Debug.Log ("Not Walkable");
			}

			// *** Patrol Left Mouse Events ***

			if (Input.GetMouseButtonDown (0) == true && GUIActive == false) {

				if (canMove) {
					if (patrolMem.Count == 0) {
						Vector3 tempPoint = hit.point;
						tempPoint.y = Terrain.activeTerrain.SampleHeight (hit.point);
						patrolMem.Add (tempPoint);
						eventManager.ParticleEvent (transform.position, tempPoint, 3);
						Debug.Log ("Set Point A");
						return;
					} else {
						Vector3 tempPoint = hit.point;
						tempPoint.y = Terrain.activeTerrain.SampleHeight (hit.point);
						patrolMem.Add (tempPoint);
						eventManager.ParticleEvent (transform.position, tempPoint, 3);
						eventManager.ServePatrol (patrolMem [0], patrolMem [1], playerID);
						Debug.Log ("Set Point B");
						pCursor.SetActive (false);
						eventManager.ParticleEvent (hit.point, hit.point, 19);
						patrolActive = false;
						onGUIexit (this.gameObject);
					}
				}
			}

			// *** Patrol Right Mouse Events ***
			
			if (Input.GetButtonDown ("Fire1") == true && GUIActive == false) {
				pCursor.SetActive (false);
				eventManager.ParticleEvent (hit.point, hit.point, 19);
				patrolActive = false;
				onGUIexit (this.gameObject);
			}

			// *** Patrol Esc Key Events ***

			if (Input.GetKeyDown ("escape")) {
				pCursor.SetActive (false);
				eventManager.ParticleEvent ( hit.point, hit.point, 19);
				patrolActive = false;
				onGUIexit (this.gameObject);
			}
		} else {
			eventManager.ParticleEvent ( hit.point, hit.point, 19);
			pCursor.SetActive (false);
		}
	}

	void patrolSwitch(Vector3 pointA, Vector3 PointB, int ID){

		if (ID == playerID) {

			patrolActive = true;
			buildActive = false;

			if (pCursor == null) {
				pCursor = Instantiate (patrolCursor, transform.position, transform.rotation);
			}
			pCursor.SetActive (true);
			patrolMem.Clear ();
			_state = State.Patrol;
		}
	}
}