using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class unitLogic : MonoBehaviour {

	public bool selectState = false;
	public bool onMission;
	public Order cMemory; //AI Component
	public int playerID = 1;
	public int techLevel = 1;
	public bool isLeader = false;
	public bool isMoving = false;
	public int EXP = 0;

	public enum Type {
		Miner,
		Utility,
		mGun,
		Missile

	}
	public Type _type;

	public enum mineState {
		mine,
		seek,
		unload
	}
	public mineState _mineState;


	public enum attackState {
		toRange,
		engage
	}
	public attackState _attackState;

	public enum logicState {
		Idle,
		Patrol,
		Move,
		Mine,
		Repair,
		Build,
		Attack,
		Flee
	}
	public logicState _logicState;

	public enum State {
		Initialise,
		Setup,
		Idle,
		Patrol,
		Move,
		Mine,
		Repair,
		Build,
		Attack,
		Flee,
		Done
	}

	public State _state;

	[Header("Unit Type & AI State")]
	public GameObject target;
	public Vector3 navTarget;	
	public GameObject fSlot;

	private Vector3 patrolA;
	private Vector3 patrolB;

	public float flockSpeed = 10.0f;
	public float turnDamp = 1.0f;
	public float approachDistance = 100.0f;
	public float navMargin = 10.0f;
	public float formationMargin = 500.0f;
	public float repathTimer = 2.5f;
	private float repathTick;

	[Header("Unit Vitals")]
	public int health;
	public int ammo;
	public int rCapacity = 120;
	public int cargo = 0;

	[Header("Weapon Stuff")]
	public int damage = 5;
	public float fireRate = 3;
	private float reload;
	public bool canFire = true;
	public GameObject ammoType;
	public GameObject[] launchers;

	public float attackRange = 300.0f;
	public float reTargetRange = 300.0f;
	public float skirmishRange = 2000.0f;
	public float chaseMargin = 100.0f;

	private navMemory navMemo1;
	public Vector3 pos1;
	public Vector3 pos2;
	private bool posInit;

	public string playerTag = "Friendly";
	public string AITag = "Enemy";

	private selectionManager sManager;
	public AIPathfinder pathfinder;
	private Rigidbody rb;

	public GameObject skin1;
	public GameObject skin2;
	public GameObject skin3;
	public GameObject skin4;

	public GameObject[] Skins = new GameObject[5];

	void OnEnable(){
		eventManager.onNavArray += navSet;
		eventManager.onAttackArray += attackNav;
		eventManager.onAttackClick += attackClick;
		eventManager.onGroundAttackClick += groundAttackClick;
		eventManager.onSelectEvent += selectEvent;
		eventManager.onDamage += damageEvent;
		eventManager.onUnitDestroy += targetCheck;
		eventManager.onServePatrol += patrolSet;
		eventManager.onServePosition += rePosition;

// AI events

		eventManager.onServeOrder += serveOrder;


		//Debug.Log("OnEnabled", gameObject);

	}

	void OnDisable(){
		eventManager.onNavArray -= navSet;
		eventManager.onAttackArray -= attackNav;
		eventManager.onAttackClick -= attackClick;
		eventManager.onGroundAttackClick -= groundAttackClick;
		eventManager.onSelectEvent -= selectEvent;
		eventManager.onDamage -= damageEvent;
		eventManager.onUnitDestroy -= targetCheck;
		eventManager.onServePatrol -= patrolSet;
		eventManager.onServePosition -= rePosition;

// AI events

		eventManager.onServeOrder -= serveOrder;

	//	Debug.Log("OFF", gameObject);

	}

	IEnumerator Start(){
		_state = State.Initialise;

		while (true) {

			switch (_state) {
			case State.Initialise:
				initMe ();
				break;
			case State.Setup:
				inSetup ();
				break;
			case State.Idle:
				idleState ();
				break;
			case State.Move:
				inMoveState ();
				break;
			case State.Mine:
				inMineState();
				break;
			case State.Build:
				inBuildState();
				break;
			case State.Repair:
				inRepairState();
				break;
			case State.Patrol:
				inPatrolState();
				break;
			case State.Attack:
				inAttackState();
				break;
			case State.Done:
				doneState ();
				break;
			}
			yield return 0;
		}	
	}

	// Core States & State Logic

	private void initMe(){
		GameObject temp = GameObject.Find ("Selection Manager");
		sManager = temp.GetComponent<selectionManager> ();
		pathfinder = GetComponent<AIPathfinder> ();
		rb = GetComponent<Rigidbody> ();
		_state = State.Setup;
		reload = fireRate;
		repathTick = repathTimer;

		Skins [0] = skin1;
		Skins [1] = skin1;
		Skins [2] = skin2;
		Skins [3] = skin3;
		Skins [4] = skin4;

	}
	private void inSetup(){
		int i = 0;

		while(i<5) {
				if (i == playerID) {
					Skins[i].SetActive (true);
				} else {
					Skins[i].SetActive (false);
				}
			i++;
		}

		_logicState = logicState.Idle;
		_state = State.Idle;
	}

	private void idleState(){

		Vector3 pos = transform.position;
		pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
		transform.position = pos;


		if (onMission == true) {

			if (cMemory._type == Order.Type.Recon) {
				eventManager.ReturnOrder (cMemory, null, 1);
			}
			if (cMemory._type == Order.Type.Build) {
				eventManager.ReturnOrder (cMemory, null, 1);
			}
				
			onMission = false;
		}

	}


	// MOVE STATE

	private void inMoveState(){

		Vector3 pos = transform.position;
		pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
		transform.position = pos;


		if (checkMoving () != true) {
			Debug.Log ("Stuck");
			if (_logicState != logicState.Move) {
				state2LogicState ();
			} else {
				eventManager.RequestNav (this.gameObject, navTarget, (attackRange - 1));
			}
		}

		isMoving = true;
	//	if (isLeader == true) {
	//		moveLeader ();
//		} else {
		pathfinder.target = navTarget;

		if (_logicState != logicState.Move) {
			if (Vector3.Distance (transform.position, navTarget) <= navMargin) {
				isMoving = false;
				state2LogicState ();	
			}
		}

		else {
			if (Vector3.Distance (transform.position, navTarget) <= navMargin) {
				isMoving = false;
				_state = State.Idle;
				_logicState = logicState.Idle;
				}
			}

			if (Vector3.Distance (transform.position, navTarget) < (navMargin * 5)) {
				Vector3 tryPos = navTarget;
				Vector3 rayPos = new Vector3 (tryPos.x, 200, tryPos.z);
				RaycastHit hit;
				if (Physics.Raycast (rayPos, -Vector3.up, out hit, 250)) {
				if (hit.collider.tag != "Floor" && hit.collider.tag != "buildPlane") {
						if (_logicState == logicState.Move || _logicState == logicState.Patrol) {
							eventManager.RequestNav (this.gameObject, navTarget, (attackRange - 1));
						} else {
							if (_state == State.Mine) {
								if (_mineState == mineState.seek || _mineState == mineState.unload) {
									eventManager.RequestNav (this.gameObject, navTarget, (attackRange - 1));
								} else {
									if (target != null) {
										if (target.tag == "Resource") {
											eventManager.RequestPosition (this.gameObject, target, (attackRange - 1));
										}
									}
								}
							}
							if (_state == State.Attack) {
							}
						}
					} 
				}
			}
				
//		}
								}

	private void moveLeader(){
		isMoving = true;
		pathfinder.target = navTarget;

		if (_logicState != logicState.Move) {
			if (Vector3.Distance (transform.position, navTarget) <= navMargin) {
				isMoving = false;
				state2LogicState ();	
			}
		}

		else {
			if (Vector3.Distance (transform.position, navTarget) <= navMargin) {
				isMoving = false;
				_state = State.Idle;
				_logicState = logicState.Idle;
			}
		}
		if (Vector3.Distance (transform.position, navTarget) < (navMargin * 2)) {
			GameObject tempGo = null;
			Vector3 tryPos = navTarget;
			Vector3 rayPos = new Vector3 (tryPos.x, 200, tryPos.z);
			RaycastHit hit;
			if (Physics.Raycast (rayPos, -Vector3.up, out hit, 250)) {
				if (hit.collider.tag != "Floor" && hit.collider.tag != "buildPlane") {
					eventManager.RequestNav (this.gameObject, navTarget, (attackRange-1));
				}
			} 
		}
	}
		
	// ATTACK STATE


	private void inAttackState(){
		switch (_attackState) {
		case attackState.toRange:
			navToTarget ();
			break;
		case attackState.engage:
			engageTarget ();
			break;
		}
	}

	// Attack SubState Loops

	private void navToTarget(){
		
		if (Vector3.Distance (transform.position, target.transform.position) <= attackRange) {
			_attackState = attackState.engage;
		} else {
			eventManager.RequestPosition (this.gameObject, target, attackRange);
		}

	}
	private void engageTarget(){
		// Engage whilst in range.



		if (target != null) {

			reload -= Time.deltaTime;

			if (reload <= 0) {
				canFire = true;
			} else {
				canFire = false;}

			if (Vector3.Distance (target.transform.position, transform.position) >= attackRange) {
				_attackState = attackState.toRange;
			} else {

				if (_type == Type.Missile) {
					foreach (GameObject launcher in launchers) {
						if (canFire == true) {
							GameObject tempRocket = Instantiate (ammoType, launcher.transform.position, launcher.transform.rotation);
							tempRocket.transform.parent = this.gameObject.transform;
							reload = fireRate;
						}
					}
				}
				if (_type == Type.mGun) {
					foreach (GameObject launcher in launchers) {
						Vector3 rayOrigin = launcher.transform.position;
						RaycastHit hit;
						if (canFire == true) {
						if (Physics.Raycast (rayOrigin, launcher.transform.forward, out hit, attackRange)) {
							if (hit.collider.gameObject == target) {
									Instantiate (ammoType, launcher.transform.position, launcher.transform.rotation);
									reload = fireRate;
								}
							} else {
								eventManager.RequestPosition (this.gameObject, target, (attackRange*0.6f));
							}
				
						}
					}
				}
			}
		}
		if (target == null) {
			scanInstance vicinity = targetScan (transform.position);
			if (vicinity.hasTarget == true) {
				target = vicinity.nearest;
			} else {
				if (_logicState != logicState.Attack) {
					state2LogicState ();
				} else {
					_state = State.Idle;
					_logicState = logicState.Idle;
				}
			}
		}
	}

	private void inMineState(){
			switch (_mineState) {
			case mineState.mine:
				stateMine ();
				break;
			case mineState.seek:
				seekRefinery ();
				break;
			case mineState.unload:
				stateUnload();
				break;
			}
		}

	private void stateMine(){

		if (cargo >= rCapacity) {
			
			navMemo1 = navScan ();
			_mineState = mineState.seek;
			Debug.Log ("out 2 : Seek");
			return;
		}


		if (target != null) {
			turn ();
			// mine
			reload -= Time.deltaTime;
			if (reload <= 0) {
				eventManager.Damage (damage, this.gameObject, target);
				cargo += damage;
				reload = fireRate;	
			}

			if (Vector3.Distance (transform.position, target.transform.position) > attackRange) {
				eventManager.RequestPosition (this.gameObject, target, (attackRange));
				Debug.Log ("following pod");
			}

			if (cargo >= rCapacity) {
				navMemo1 = navScan ();
				_mineState = mineState.seek;
				Debug.Log ("out 2 : Seek");
			}
		} else {
			scanInstance temp = targetScan(transform.position);
			if (temp.hasTarget == true){
				Debug.Log ("found nearest target");
				target = temp.nearest;
			}
			else{
				if(cargo>10){
					navMemo1 = navScan ();
					_mineState = mineState.seek;
					Debug.Log ("out 3 : Seek");
				}
				else{
					_logicState = logicState.Idle;
					_state = State.Idle;
				}
			}
		}
}

	private void seekRefinery(){

		if (target != null) {
			if (target.name != "Refinery" && target.name != "Refinery(Clone)" ) {
				if (findRefinery () != null) {
					target = findRefinery ();
					buildLogic tempLogic = target.GetComponent<buildLogic> ();
					navTarget = tempLogic.interfacePoint;
					_state = State.Move;
				} else {
					_logicState = logicState.Idle;
					_state = State.Idle;
				}
			} else {
				if (Vector3.Distance (transform.position, target.transform.position) <= (attackRange*2)) {
					_mineState = mineState.unload;
				} else {
					buildLogic tempLogic = target.GetComponent<buildLogic> ();
					navTarget = tempLogic.interfacePoint;
					_state = State.Move;
				}
			}

		} else {
			if (findRefinery () != null) {
				target = findRefinery ();
				buildLogic tempLogic = target.GetComponent<buildLogic> ();
				navTarget = tempLogic.interfacePoint;
				_state = State.Move;
			} else {
				_logicState = logicState.Idle;
				_state = State.Idle;
			}
		}

	}
	private void stateUnload(){

		if (cargo <= 0) {
			cargo = 0;
			if (navMemo1.resourceCount > 0) {
				if (navMemo1.enemyCount != 0 && navMemo1.friendlyCount < (navMemo1.enemyCount * 1.5)) {
					_state = State.Idle;
					_logicState = logicState.Idle;
				} else {
					navTarget = navMemo1.location;
					target = null;
					_mineState = mineState.mine;
					_state = State.Move;
				}
			} else {
				_state = State.Idle;
				_logicState = logicState.Idle;
			}
		}

		if (target != null) {
			turn ();
			reload -= Time.deltaTime;
			if (reload <= 0 && cargo > 0) {
				eventManager.Collect (damage, playerID);
				cargo -= damage;
				reload = fireRate;	
			}
	
		} else {
			if (cargo > 10) {
				_mineState = mineState.seek;
				Debug.Log ("out 5 : Seek");
			}
		}
	}
		
	private void inRepairState(){
		
	}
	private void inBuildState(){

		if (target != null) {
			if (target.tag != "betaStructure") {
				scanInstance tempScan = targetScan (transform.position);
				if (tempScan.hasTarget == true) {
					target = tempScan.nearest;
					eventManager.onRequestPosition (this.gameObject, target, attackRange);
					_state = State.Move;
				} else {
					_logicState = logicState.Idle;
					_state = State.Idle;
				}
			}
			turn ();
			reload -= Time.deltaTime;
			if (reload <= 0) {
				eventManager.Construction (damage, this.gameObject, target);
				reload = fireRate;	
			}
		}
		else{
			scanInstance tempScan = targetScan(transform.position);
			if(tempScan.hasTarget == true){
				target = tempScan.nearest;
			}
			else{
				_logicState = logicState.Idle;
				_state = State.Idle;
			}
		}
	}
	private void inPatrolState(){

		Vector3 temp = patrolA;
		patrolA = patrolB;
		patrolB = temp;

		navTarget = patrolB;
	
		_state = State.Move;
		
	}
	private void doneState(){
		
	}


	// Responses to Events + Support Functions


	void navSet(Vector3 point, GameObject actor, bool leader){

		if (actor == this.gameObject) {
			target = null;
			if (leader == true) {
				isLeader = true;
				navTarget = point;
				_logicState = logicState.Move;
				_state = State.Move;
			} else {
				navTarget = point;
				_logicState = logicState.Move;
				_state = State.Move;
			}
		}
	}
		
	void attackNav(Vector3 point, GameObject actor, bool leader){
		if (actor == this.gameObject) {
			if (leader == true) {
				isLeader = true;
			}	

			if (_type == Type.mGun) {
				_logicState = logicState.Attack;
				navTarget = point;
				_attackState = attackState.toRange;
				_state = State.Move;
			}
			if (_type == Type.Missile) {
				_logicState = logicState.Attack;
				navTarget = point;
				_attackState = attackState.toRange;
				_state = State.Attack;
			}
			if (_type == Type.Miner) {
				navTarget = point;
				_logicState = logicState.Mine;
				_mineState = mineState.mine;
				_state = State.Mine;
			}
			if (_type == Type.Utility) {
				_logicState = logicState.Build;
				navTarget = point;
				_state = State.Move;
			}
		}
		}


	void patrolSet (Vector3 pointA, Vector3 pointB){


		if (selectState == true) {
			patrolA = pointA;
			patrolB = pointB;
		
			float aDist = Vector3.Distance (transform.position, pointA);
			float bDist = Vector3.Distance (transform.position, pointB);

			if (aDist < bDist) {
				Vector3 temp = patrolA;
				patrolA = patrolB;
				patrolB = temp;

				navTarget = patrolB;

			} else {
				navTarget = pointB;
			}

			_logicState = logicState.Patrol;
			_state = State.Move;
		}
	}
	void attackClick(Vector3 point, GameObject actor, int ID){
		if (selectState == true && ID == playerID) {
			if (_type == Type.mGun | _type == Type.Missile) {
				target = actor;}
			if (_type == Type.Miner && actor.tag == "Resource") {
				_state = State.Mine;
				_logicState = logicState.Mine;
				_mineState = mineState.mine;
				target = actor;
			}
			if (_type == Type.Utility && actor.tag == "betaStructure") {
				Debug.Log ("Target Set - State Switched");
				target = actor;
				navTarget = actor.transform.position;
				_logicState = logicState.Build;
				_state = State.Move;
				}
			if (_type == Type.Miner && actor.name == "refinery") {
				target = actor;
				_mineState = mineState.unload;
				_logicState = logicState.Mine;
				_state = State.Move;
			}
		}
		
	}
	void groundAttackClick(Vector3 point, GameObject actor, int ID){
		
	}
	void selectEvent(GameObject selectedUnit){
		if (sManager.currentSelection.Contains (this.gameObject)) {
				selectState = true;
			} else {
				selectState = false;
			}
		}

	void damageEvent(int amount, GameObject sender, GameObject reciever){
		if (reciever == this.gameObject) {
			health -= amount;
		}
		if (sender == this.gameObject && reciever.tag == AITag) {
			EXP++;
		}
		if (health <= 0) {
			_state = State.Done;
		}
	}

	void rePosition(GameObject actor, Vector3 vantage){
		if (actor == this.gameObject) {
			navTarget = vantage;
			_state = State.Move;
		}
	}



	void targetCheck(GameObject actor, bool state){
		scanInstance currScan = targetScan (transform.position);
		bool hasTarget = currScan.hasTarget;

		if (!hasTarget && _logicState != logicState.Attack) {
			state2LogicState ();
		}
		if (!hasTarget && _logicState == logicState.Attack) {
			_logicState = logicState.Idle;
			_state = State.Idle;
		}
		if (hasTarget == true) {
			target = currScan.nearest;
		}
	}

	navMemory navScan (){
		Vector3 point = transform.position;
		GameObject[] friendlies;
		GameObject[] enemies;
		GameObject[] resourceUnits;
		List<GameObject> localFriendlies = new List<GameObject>();
		List<GameObject> localHostiles = new List<GameObject>();
		List<GameObject> localResource = new List<GameObject>();

		enemies = GameObject.FindGameObjectsWithTag (AITag);
		friendlies = GameObject.FindGameObjectsWithTag (playerTag);
		resourceUnits = GameObject.FindGameObjectsWithTag ("Resource");
		foreach (GameObject unit in friendlies) {
			if (Vector3.Distance (point, unit.transform.position) <= reTargetRange) {
				localFriendlies.Add(unit);
			}
		}
		foreach (GameObject unit in enemies) {
			if (Vector3.Distance (point, unit.transform.position) <= reTargetRange) {
				localHostiles.Add(unit);
			}
		}
		foreach (GameObject unit in resourceUnits) {
			if (Vector3.Distance (point, unit.transform.position) <= reTargetRange) {
				localResource.Add(unit);
			}
		}

		int friendlyCount = localFriendlies.Count;
		int enemyCount = localHostiles.Count;
		int resourceCount = localResource.Count;

		navMemory scanreturn = new navMemory (point, friendlyCount, enemyCount, resourceCount);

		return scanreturn;
	}

	scanInstance targetScan(Vector3 position){

		bool hasTarget;
		GameObject nearestTarget;
		int targetCount;
		int friendlyCount;
		GameObject[] enemies;
		GameObject[] friendlies;
		GameObject[] resourceUnits;
		float distance = Mathf.Infinity;
		enemies = GameObject.FindGameObjectsWithTag (AITag);
		friendlies = GameObject.FindGameObjectsWithTag (playerTag);
		resourceUnits = GameObject.FindGameObjectsWithTag ("Resource");
		targetCount = enemies.Length;
		friendlyCount = friendlies.Length;
		nearestTarget = this.gameObject;


		if (_type == Type.mGun | _type == Type.Missile) {
			GameObject closest = null;
		//	Vector3 position = transform.position;
			foreach (GameObject go in enemies) {
				Vector3 diff = go.transform.position - position;
				float curDistance = diff.sqrMagnitude;
				if (curDistance < distance) {
					closest = go;
					distance = curDistance;
					nearestTarget = closest;
				} 
			}
		}
		if (_type == Type.Miner) {
			GameObject closest = null;
			bool currClosest = true;
		//	Vector3 position = transform.position;
			float currLeader = Mathf.Infinity;
			foreach (GameObject go in resourceUnits) {
				float curDistance = Vector3.Distance(position, go.transform.position);
				if (curDistance < currLeader) {
					closest = go;
					distance = curDistance;
					currLeader = curDistance;
					nearestTarget = closest;
				}
			}
		}
		if (_type == Type.Utility) {
			GameObject closest = null;
			bool currClosest = true;
		//	Vector3 position = transform.position;
			float currLeader = Mathf.Infinity;
			foreach (GameObject go in sManager.betaStructs) {
				float curDistance = Vector3.Distance(position, go.transform.position);
				if (curDistance < currLeader) {
					closest = go;
					distance = curDistance;
					currLeader = curDistance;
					nearestTarget = closest;
				}
			}
		}
		if (distance <= reTargetRange && nearestTarget != this.gameObject) {
			hasTarget = true;
		} else {
				hasTarget = false;
		}
		scanInstance scanReturn = new scanInstance (hasTarget, nearestTarget, friendlyCount, targetCount);
		return scanReturn;
		}

	void state2LogicState ()
	{
		if (_logicState == logicState.Idle) {
			_state = State.Idle;
		}
		if (_logicState == logicState.Move) {
			_state = State.Move;
		}
		if (_logicState == logicState.Attack) {
			_state = State.Attack;
		}
		if (_logicState == logicState.Patrol) {
			_state = State.Patrol;
		}
		if (_logicState == logicState.Mine) {
			_state = State.Mine;
		}
		if (_logicState == logicState.Repair) {
			_state = State.Repair;
		}
		if (_logicState == logicState.Build) {
			_state = State.Build;
		}
	}

	void hazardAvoid(){
		// Local short raycast collision avoid
		turn ();
	}



	void turn(){
//	if (_state == State.Move && _moveState == moveState.flock) {
//		Vector3 dir = (fSlot.transform.position - transform.position).normalized;
//		dir.y = 0;
//		Quaternion rotation = Quaternion.LookRotation (dir);
//		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, turnDamp * Time.deltaTime);
//	}
		if (_state == State.Mine | _state == State.Build | _state == State.Attack) {
			Vector3 dir = (target.transform.position - transform.position).normalized;
			dir.y = 0;
			Quaternion rotation = Quaternion.LookRotation (dir);
			transform.rotation = Quaternion.Slerp (transform.rotation, rotation, turnDamp * Time.deltaTime);
		}
	}

	public GameObject findRefinery(){

		List<GameObject> allStructs = new List<GameObject> ();
		List<GameObject> Depots = new List<GameObject>();

		allStructs.AddRange (GameObject.FindGameObjectsWithTag ("Structure"));

		foreach (GameObject part in allStructs) {
			if (part.name == "Refinery(Clone)" || part.name == "Refinery") {
				if (part.GetComponent<buildLogic> ().playerID == playerID) {
					Depots.Add (part);
				}
			}
		}


		if (Depots.Count > 0) {

			GameObject closest = null;
			float distance = Mathf.Infinity;
			Vector3 position = transform.position;
			if (Depots.Count > 0) {
				foreach (GameObject go in Depots) {
					Vector3 diff = go.transform.position - position;
					float curDistance = diff.sqrMagnitude;
					if (curDistance < distance) {
						closest = go;
						distance = curDistance;
					}
				}
				return closest;
			} else {
				return null;
			}
		} else {
			return null;
		}
	}
	bool checkMoving (){
		
		bool moving = true;
		repathTick -= Time.deltaTime;

		if (repathTick <= 0) {
			if (posInit == true) {
				pos1 = transform.position;
				posInit = false;
				repathTick = repathTimer;
				if (Vector3.Distance (pos1, pos2) < 2) {
					moving = false;
				}
			} else {
				pos2 = transform.position;
				posInit = true;
				repathTick = repathTimer;
				if (Vector3.Distance (pos1, pos2) < 2) {
					moving = false;
				}
			}
		}
		return moving;
	}


// ******* AI FUNCTIONS ********* //

	void serveOrder(Order order, GameObject[] units, int statusCode){

		Debug.Log ("Unit Logic");

// Add AI order to memory, set nav, target, states.

		if (order.playerID == playerID) {

			Debug.Log ("Recieved Order");

			List<GameObject> someList = new List<GameObject>(units);

			if (someList.Contains (this.gameObject)) {
				Debug.Log ("On List");
				onMission = true;
				cMemory = order;

				if (cMemory._type == Order.Type.Recon) {

					Debug.Log ("Recieved Recon Mission " + this.gameObject);
					navSet (cMemory.navTarget, this.gameObject, false);

				}

				if (cMemory._type == Order.Type.Patrol) {

					selectState = true;
					patrolSet (cMemory.patrolA, cMemory.patrolB);
					selectState = false;

				}

				if (cMemory._type == Order.Type.Attack || cMemory._type == Order.Type.Build) {
					Debug.Log ("Command Event");
					Debug.Log (cMemory.unitTarget);
					selectState = true;
					attackClick (cMemory.navTarget, cMemory.unitTarget, playerID);
					selectState = false;
				}

				if (cMemory._type == Order.Type.Mine) {

					scanInstance focalScan = targetScan (cMemory.navTarget);
					cMemory.unitTarget = focalScan.nearest;

					selectState = true;

					if (_type == Type.Miner) {
						attackClick (cMemory.navTarget, cMemory.unitTarget, playerID);
						selectState = false;
					} 
					else {
						selectState = true;
						Debug.Log (cMemory.patrolA +":"+ cMemory.patrolB);


						patrolSet (cMemory.patrolA, cMemory.patrolB);
						selectState = false;
					}

				}
			
			} else {
				Debug.Log ("Not in List");
			}
		
		}

	}
		
}