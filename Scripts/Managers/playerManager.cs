//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class playerManager : MonoBehaviour {
//
//	//AI Targets
//
//	[SerializeField] GameObject followTarget;
//	public Vector3 navTarget;
//	[SerializeField] GameObject attackTarget;
//	[SerializeField] Vector3 gndAttackTarget;
//	[SerializeField] selectionManager sManager;
//	[SerializeField] float aRange = 10;
//	public GameObject miner;
//	public GameObject mGun;
//	public GameObject[] launchers;
//
//	//Player StateMachine States
//
//	public enum State {
//		Initialise,
//		Setup,
//		Idle,
//		Move,
//		Follow,
//		Attack,
//		Done
//	}
//
//	public State _state;
//
//	//Player Stats
//
//	public int health;
//	public int ammo;
//	public float fireRate = 20;
//	private float ticker = 60;
//	public int damage = 5;
//	public bool canFire;
//	public bool selectState;
//	public int EXP;
//
//	IEnumerator Start(){
//		_state = State.Initialise;
//
//		while (true) {
//		
//			switch (_state) {
//			case State.Initialise:
//				initMe ();
//				break;
//			case State.Setup:
//				setMeUp ();
//				break;
//			case State.Idle:
//				idleState ();
//				break;
//			case State.Move:
//				moveState ();
//				break;
//			case State.Follow:
//				followState ();
//				break;
//			case State.Attack:
//				attackState ();
//				break;
//			case State.Done:
//				doneState ();
//				break;
//			}
//			yield return 0;
//		}	
//	}
//		
//	void OnEnable(){
//		eventManager.onNavClick += navMove;
//		eventManager.onFollowClick += navFollow;
//		eventManager.onAttackClick += attackClick;
//		eventManager.onGroundAttackClick += groundAttackClick;
//		eventManager.onSelectEvent += selectEvent;
//		eventManager.onDamage += DamageEvent;
//
//
//	//	Debug.Log("OnEnabled", gameObject);
//
//	}
//
//	void OnDisable(){
//		eventManager.onNavClick -= navMove;
//		eventManager.onFollowClick -= navFollow;
//		eventManager.onAttackClick -= attackClick;
//		eventManager.onGroundAttackClick -= groundAttackClick;
//		eventManager.onSelectEvent += selectEvent;
//		eventManager.onDamage -= DamageEvent;
//
//	//	Debug.Log("OFF", gameObject);
//		
//	}
//
//	private void initMe()
//	{
//		Debug.Log ("Initialise");
//		navTarget = transform.position;
//		_state = State.Setup;
//	}
//
//	private void setMeUp()
//	{
//		if (sManager == null) {
//			GameObject owner = GameObject.FindWithTag("SelectionManager");
//			sManager = owner.GetComponent<selectionManager> ();
//		}
//		navTarget = transform.position;
//		Debug.Log ("Setup");
//		_state = State.Idle;
//	}
//
//	private void idleState()
//	{
//		Debug.Log ("Idle", gameObject);
//	}
//
//	private void moveState()
//	{
//		Debug.Log("Moving", gameObject);
//	}
//
//	private void followState()
//	{
//		navTarget = followTarget.transform.position;
//		Debug.Log("Following", gameObject);
//	}
//
//	private void attackState()
//	{
//		float distance = Vector3.Distance (transform.position, attackTarget.transform.position);
//		if (distance > aRange) {
//			navTarget = attackTarget.transform.position;
//		} else {
//			navTarget = transform.position;
//		}
//		if (distance <= aRange+3) {
//			ticker = ticker - (Time.deltaTime * fireRate);
//			if (ticker <= 0) {
//				canFire = true;
//			} else
//				canFire = false;
//		if (canFire) {
//		dealDamage();
//		ticker = 60;
//		}
//		}
//		Debug.Log ("Actions");
//	}
//
//	private void doneState()
//	{
//		Debug.Log ("Attacking", gameObject);
//	}
//	void navMove (Vector3 point, GameObject actor, int playerID) {
//		Debug.Log ("pManager Move");
//		if (selectState == true) {
//			navTarget = point;
//			_state = State.Move;
//		}
//	}
//	void navFollow (Vector3 point, GameObject actor, int playerID) {
//		if (selectState == true) {
//			followTarget = actor;
//			_state = State.Follow;
//		}
//	}
//	void attackClick (Vector3 point, GameObject actor, int ID) {
//		if (selectState == true) {
//			attackTarget = actor;
//			_state = State.Attack;
//		}
//	}
//	void groundAttackClick (Vector3 point, GameObject actor, int ID) {
//		if (selectState == true) {
//			gndAttackTarget = point;
//			_state = State.Attack;
//		}
//	}
//	public void selectEvent (GameObject unused, int playerID)
//	{
//		if (sManager.currentSelection.Contains (this.gameObject)) {
//			selectState = true;
//		} else {
//			selectState = false;
//		}
//	}
//	void dealDamage(){
//		Debug.Log ("DealDamage");
//		eventManager.Damage (damage, this.gameObject, attackTarget);
//	}
//	public void DamageEvent(int hitpoints, GameObject sender, GameObject reciever){
//
//		Debug.Log ("DamageEvent");
//
//		//Taking Damage
//
//		if (reciever == this.gameObject) {
//			health = (health - damage);
//		}
//		// Collecting Resource
//
//		if (reciever.tag == "Resource") {
//			Debug.Log ("Mining");
//			eventManager.Collect (damage,1);
//		}
//		if (reciever.tag == "ResourceG") {
//			Debug.Log ("THARBEGOLDINDEMHILLS");
//			eventManager.Collect (damage, 2);
//		}
//		if (sender == this.gameObject && reciever.tag != "Resource" && reciever.tag != "ResourceG" && reciever.tag != "Friendly") {
//			EXP++;
//		}
//	}
//}