using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputManager : MonoBehaviour {

	[Header("Mouse Settings")]
	public Vector2 _box_start_pos;
	public Vector2 _box_end_pos;
	public GameObject unused;
	public int unused2 = 3;
	public Texture boxSelectTexture;
	public float dragSenstivity = 5;
	public float delta;

	[Header("GUI Settings")]
	public GameObject GUIman;
	public GUIManager Gmgmt;
	public bool GUIActive = false;
	public bool buildActive = false;
	public bool patrolActive = false;

	public enum State {
		Initialise,
		Setup,
		Idle,
		InGame,
		GUI,
		Patrol,
		Build
	}

	public State _state;

	// Update is called once per frame
	void OnEnable(){
		eventManager.onButtonEnter += onGUIenter;
		eventManager.onButtonExit += onGUIexit;
	}

	void OnDisable(){
		eventManager.onButtonEnter -= onGUIenter;
		eventManager.onButtonExit -= onGUIexit;
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
//		case State.GUI:
//			inGui ();
//			break;
//		case State.Patrol:
//			inPatrol ();
//			break;
//		case State.Build:
//			inBuild ();
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
					eventManager.ClearSelect (unused);
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
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); 
			if (Physics.Raycast (ray, out hit, 2000.0f)) {
				if (hit.collider.tag != null) {
					if (hit.collider.tag == "Enemy" || hit.collider.tag == "Friendly" || hit.collider.tag == "NPC" || hit.collider.tag == "Structure" || hit.collider.tag == "Resource" || hit.collider.tag == "ResourceG") {
						eventManager.AttackClick (hit.point, hit.collider.gameObject);
						Debug.Log ("Clicked Attack");
					}
					if (hit.collider.tag == "Floor" || hit.collider.tag == "Environment" || hit.collider.tag == "buildPlane") {
						Debug.Log ("Clicked nav");
						eventManager.NavClick (hit.point, hit.collider.gameObject);
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
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); 
			if (Physics.Raycast (ray, out hit, 1000.0f)) {
				if (hit.collider.tag != null) {
					if (hit.collider.tag == "Enemy" || hit.collider.tag == "NPC" || hit.collider.tag == "Friendly") {
						eventManager.FollowClick (hit.point, hit.collider.gameObject);
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
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); 
		if (Physics.Raycast (ray, out hit, 1000.0f)) {
			if (hit.collider.tag != null) {
				if (hit.collider.tag == "Friendly" || hit.collider.tag == "FriendlyStruct") {
					eventManager.UnitSelect (hit.collider.gameObject);
					Debug.Log ("Clicked Select");
				
				} else {
					eventManager.ClearSelect (hit.collider.gameObject);
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
			_state = State.GUI;
		}
	}
	void onGUIexit(GameObject unused){
		if (GUIActive == true) {
			GUIActive = false;
			Debug.Log ("GUI is Not Active");
			if (buildActive == false && patrolActive == false) {
				_state = State.InGame;
			}
		}
	}
}