using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitAgent : MonoBehaviour {


	public int playerID = 0;
	public bool debugActive = false;
	public bool onMission;
	public bool isMoving;
	public bool isAggressive;

	public int hitPoints = 100;
	public int health = 100;
	public int shield = 100;
	public int damage = 20;

	public float fireRate = 1;
	private float f_rate;

	public float repathTimer = 2.5f;
	private float r_rate;

	public int techLevel = 1;

	public int cargo = 0;
	public int rCapacity = 100;

	public float reTargetRange = 600;

	public float attackRange = 200;
	public float weaponRange = 300;
	public float chaseRange = 600;
	public float turnDamp = 2;
	public float navMargin = 10;
	private int stuckCycles = 2;

	public GameObject target;
	public GameObject target_2;
	public GameObject target_3;

	public Vector3 navTarget = new Vector3 (0, 0, 0);

	public Order cMemory;
	public navMemory navMem_1;
	public navMemory navMem_2;

	public Vector3 patrolA;
	public Vector3 patrolB;

	public GameObject[] launchers;
	public GameObject [] effectParticles;

	private GameObject gManager;

	private Vector3 pos1 = new Vector3(0,0,0);
	private Vector3 pos2 = new Vector3(0,0,0);
	private bool posInit = false;

	public enum Type {
		miner,
		mGun,
		missile,
		utility
	}

	public Type _type;

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
		
	public State _state = State.Idle;
	public State _Logic_1 = State.Idle;
	public State _Logic_2 = State.Idle;

	public enum attackState {
		
		toRange,
		Engage

	}

	public attackState _attackState;

	public enum mineState {

		Mine,
		Seek,
		Unload

	}

	public mineState _mineState;

	public enum moveState {

		moveInit,
		moving

	}

	public moveState _moveState;


	void OnEnable(){

		eventManager.onServeOrder += onOrder;
		eventManager.onServePosition += onNav;
		eventManager.onRepair += onRepair;
		eventManager.onUnitDestroy += onDestroy;
		eventManager.onBuildInit += onBuild;

	}

	void OnDisable(){

		eventManager.onServeOrder -= onOrder;
		eventManager.onServePosition -= onNav;
		eventManager.onRepair -= onRepair;
		eventManager.onUnitDestroy += onDestroy;
		eventManager.onBuildInit += onBuild;

	}


	IEnumerator Start(){
		
			_state = State.Initialise;

			while (true) {

			cleanupChecks ();

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
					buildState();
					break;
				case State.Repair:
					repairState();
					break;
				case State.Flee:
					fleeState();
					break;
				case State.Patrol:
					patrolState();
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
	
	void initMe () {

		_state = State.Setup;
		
	}

	void inSetup () {

		gManager = GameObject.Find ("gameManager");
		f_rate = fireRate;
		r_rate = repathTimer;

		_Logic_1 = State.Idle;
		_Logic_2 = State.Idle;

		_state = State.Idle;

	}

	void idleState () {

		if (cMemory != null) {
			if (cMemory._type == Order.Type.Mine || cMemory._type == Order.Type.Move || cMemory._type == Order.Type.Recon || cMemory._type == Order.Type.Repair) {
				eventManager.ReturnOrder (cMemory, this.gameObject, 1, playerID);
			}
			cMemory = null;
		}

	}

	void inMoveState () {

		switch (_moveState) {
		case moveState.moveInit:
			moveInit ();
			break;
		case moveState.moving:
			moving ();
			break;
		}

	}

	void moveInit () {



		GetComponent<AIPathfinder> ().target = navTarget;

		_moveState = moveState.moving;

		stuckCycles = 2;
		
	}

	void moving () {

		levelY ();

		if (_Logic_1 == State.Attack && target != null) {

			if (Vector3.Distance (transform.position, target.transform.position) < weaponRange) {
				engageTarget ();
			}
		}


		if (checkMoving () != true) {
			if (debugActive) {
				Debug.Log ("Stuck");
			}

			if (_Logic_1 != State.Move && _Logic_1 != State.Patrol) {
				if (stuckCycles > 0) {
					_state = _Logic_1;
					stuckCycles--;
				} else {
					if (debugActive) {
						Debug.Log ("stuckCycle init: " + name);
					}
					eventManager.RequestNav (this.gameObject, target.transform.position, (attackRange - 2), playerID);
					stuckCycles = 2;
				}
			} else {
				eventManager.RequestNav (this.gameObject, navTarget, (attackRange - 2), playerID);
			}
		}

		if (Vector3.Distance (transform.position, navTarget) <= navMargin) {

			if (_Logic_1 == State.Move) {

				_Logic_2 = _Logic_1;
				_state = State.Idle;
				_Logic_1 = _state;
			}
			else {
				_state = _Logic_1;
			}
		}

	}

	void inMineState () {

		switch (_mineState) {
		case mineState.Mine:
			stateMine ();
			break;
		case mineState.Seek:
			seekRefinery ();
			break;
		case mineState.Unload:
			stateUnload();
			break;
		}

	}

	void stateMine(){

		if (cargo >= rCapacity) {
			_mineState = mineState.Seek;
		}

		if (target == null) {
			scanInstance now = targetScan (transform.position);
			if (now.hasTarget) {
				target = now.nearest;
				eventManager.RequestPosition (this.gameObject, target, (attackRange*(0.69f)), playerID);
			} else {
				if (cargo > 0) {
					_mineState = mineState.Seek;
				} else {
					_Logic_2 = _Logic_1;
					_state = State.Idle;
					_Logic_1 = _state;
				}
			}
			return;
		}

		if (checkDistance () == false) {

			if (debugActive) {
				Debug.Log ("Target Too Far");
			}

			_Logic_1 = _state;
			_state = State.Move;
			return;
		}

		f_rate -= Time.deltaTime;

		if (f_rate <= 0) {
			eventManager.Damage (damage, this.gameObject, target);
			cargo += damage;
			f_rate = fireRate;
		}
			

		turn ();
	
	}

	void seekRefinery(){

		var fRef = findRefinery (transform.position);

		if (fRef != null) {

			target = fRef;
			eventManager.RequestPosition (this.gameObject, target, (attackRange * 0.69f), playerID);
			_mineState = mineState.Unload;


		} else {

			if (debugActive) {
				Debug.Log ("No refineries");
			}

			_Logic_2 = _Logic_1;
			_state = State.Idle;
			_Logic_1 = _state;
			
		}
		
	}

	void stateUnload(){

		if (target == null) {
			_mineState = mineState.Seek;
			return;
		}	

		f_rate -= Time.deltaTime;

		if (!checkDistance ()) {
			_Logic_1 = _state;
			_state = State.Move;
			return;
		}

		if (f_rate <= 0) {

			if (cargo <= 0) {

				if (navMem_1 != null) {

					navTarget = navMem_1.location;
					_state = State.Move;
					_mineState = mineState.Mine;
					navMem_1 = null;
					return;
				} 
				else {
					_Logic_2 = _Logic_1;
					_state = State.Idle;
					_Logic_1 = _state;
				}
			}

			cargo -= damage;
			eventManager.Collect(damage,playerID);
			f_rate = fireRate;
		}

		turn ();

	}


	void buildState () {

		if (target != null) {

			if (target.tag != "betaStructure") {
				scanInstance now = targetScan (transform.position);
				if (now.hasTarget) {
					target = now.nearest;
					eventManager.RequestPosition (this.gameObject, target, (attackRange * 0.69f), playerID);
				} else {
					_Logic_2 = _Logic_1;
					_state = State.Idle;
					_Logic_1 = _state;
				}
				return;
			}	

			f_rate -= Time.deltaTime;

			if (!checkDistance ()) {
				_Logic_1 = _state;
				_state = State.Move;
				return;
			}

			if (f_rate <= 0) {
				eventManager.Construction (damage, this.gameObject, target);
				f_rate = fireRate;
			}

			turn ();

		} else {

			scanInstance now = targetScan (transform.position);
			if (now.hasTarget) {
				target = now.nearest;
				eventManager.RequestPosition (this.gameObject, target, (attackRange * 0.69f), playerID);
				return;
			} else {
				_Logic_2 = _Logic_1;
				_state = State.Idle;
				_Logic_1 = _state;
			}
		}
	}

	void repairState () {

		if (target == null) {
			var repTarget = nextRepair ();

			if (repTarget != null) {
				target = repTarget;
			} else {
				_Logic_2 = _Logic_1;
				_state = State.Idle;
				_Logic_1 = _state;
			}
			return;
		}

		f_rate -= Time.deltaTime;

		if (!checkDistance ()) {
			_Logic_1 = _state;
			_state = State.Move;
			return;
		}

		if (f_rate <= 0) {

			if (target.GetComponent<unitAgent> ().health >= hitPoints) {

				if(cMemory != null){
					if (target == cMemory.unitTarget) {
						eventManager.ReturnOrder (cMemory, this.gameObject, 1, playerID);
						cMemory = null;
					}
				}

				target = null;
				return;
			}

			eventManager.Repair (damage, this.gameObject, target);
			f_rate = fireRate;
		}

		turn ();

	}

	void fleeState() {

		
	}

	void patrolState(){

		Vector3 temp = patrolA;
		patrolA = patrolB;
		patrolB = temp;

		navTarget = patrolB;

		_state = State.Move;

	}

	void inAttackState(){

		if (target == null) {
			scanInstance now = targetScan (transform.position);
			if (now.hasTarget) {
				target = now.nearest;
			} else {
				_Logic_2 = _Logic_1;
				_state = State.Idle;
				_Logic_1 = _state;
			}
			return;
		}	


		if (!checkDistance ()){
			_Logic_1 = _state;
			_state = State.Move;
			return;
		}

		engageTarget ();

	}

	void engageTarget(){

		// raycast from turrets > Shoot

	}

	void doneState() {

	if (cMemory != null) {
			eventManager.ReturnOrder (cMemory, this.gameObject, 2, playerID);
			cMemory = null;
	}

	eventManager.ParticleEvent (transform.position, transform.position, 5);
	eventManager.onUnitDestroy (this.gameObject, false);
	Destroy (this.gameObject);
		
	}

	void onOrder(Order order, GameObject unit, int statusCode, int ID){

		if (order.playerID == playerID) {

			if (unit == this.gameObject) {

				if (debugActive) {
					Debug.Log ("Order Recieved: " + name);
					}

				cMemory = order;

				if (order._type == Order.Type.Move || order._type == Order.Type.Recon) {

					navTarget = order.navTarget;
					_Logic_2 = _Logic_1;
					_state = State.Move;
					_Logic_1 = _state;

					_moveState = moveState.moveInit;
					return;
				}

				if (order._type == Order.Type.Attack) {

					if (_type == Type.mGun || _type == Type.missile) {

						if (order.unitTarget.tag == "Friendly") {

							if (order.unitTarget.GetComponent<unitAgent> ().playerID != playerID) {
							
								_Logic_2 = _Logic_1;
								_state = State.Attack;
								_Logic_1 = _state;

								navTarget = order.navTarget;
								target = order.unitTarget;

								_moveState = moveState.moveInit;
								return;
							}
						}

						if (order.unitTarget.tag == "Structure") {

							if (order.unitTarget.GetComponent<buildLogic> ().playerID != playerID) {

								_Logic_2 = _Logic_1;
								_state = State.Attack;
								_Logic_1 = _state;

								navTarget = order.navTarget;
								target = order.unitTarget;

								_moveState = moveState.moveInit;
								return;
							}
						}

						if (order.unitTarget.tag == "Resource") {

							if (isAggressive == true) {

								_Logic_2 = _Logic_1;
								_state = State.Attack;
								_Logic_1 = _state;

								navTarget = order.navTarget;
								target = order.unitTarget;

								_moveState = moveState.moveInit;
								return;
						
							} else {

								cMemory._type = Order.Type.Move;

								_Logic_2 = _Logic_1;
								_state = State.Move;
								_Logic_1 = _state;

								navTarget = order.navTarget;
								target = null;

								_moveState = moveState.moveInit;
								return;

							}

						}
					}

					if (_type == Type.miner) {

						if (order.unitTarget.tag == "Resource") {

							_Logic_2 = _Logic_1;
							_state = State.Mine;
							_Logic_1 = _state;

							_mineState = mineState.Mine;

							navTarget = order.navTarget;
							target = order.unitTarget;

							_moveState = moveState.moveInit;
							return;

						}
				
						if (order.unitTarget.tag == "Structure") {

							buildLogic bLogic = order.unitTarget.GetComponent<buildLogic> ();

							if (bLogic.playerID == playerID && bLogic._type == buildLogic.Type.Refinery) {

								_Logic_2 = _Logic_1;
								_state = State.Mine;
								_Logic_1 = _state;

								_mineState = mineState.Unload;

								navTarget = order.navTarget;
								target = order.unitTarget;

								_moveState = moveState.moveInit;
								return;
							}

						}

					}

					if (_type == Type.utility) {

						if (order.unitTarget.tag == "Friendly") {

							if (order.unitTarget.GetComponent<unitAgent> ().playerID == playerID) {

								_Logic_2 = _Logic_1;
								_state = State.Repair;
								_Logic_1 = _state;

								navTarget = order.navTarget;
								target = order.unitTarget;

								_moveState = moveState.moveInit;
								return;
							}
						}
						if (order.unitTarget.tag == "betaStructure") {

							buildLogic bLogic = order.unitTarget.GetComponent<buildLogic> ();

							if (bLogic.playerID == playerID) {

								_Logic_2 = _Logic_1;
								_state = State.Build;
								_Logic_1 = _state;

								navTarget = order.navTarget;
								target = order.unitTarget;

								_moveState = moveState.moveInit;
								return;
							}

						}

					}


				}

				if (order._type == Order.Type.Patrol) {

					patrolA = order.patrolA;
					patrolB = order.patrolB;

					_Logic_2 = _Logic_1;
					_state = State.Patrol;
					_Logic_1 = _state;

					_moveState = moveState.moveInit;
					return;

				}
					
				}
					
			}
		}

	scanInstance targetScan(Vector3 position){

		bool hasTarget;
		GameObject nearestTarget;
		int targetCount;
		int friendlyCount;
		recache:
		List<GameObject> enemies = gManager.GetComponent<unitIndex> ().hostiles [playerID];
		List<GameObject> friendlies = gManager.GetComponent<unitIndex> ().units [playerID];
		List<GameObject> betaStructs = gManager.GetComponent<unitIndex> ().betaStructs [playerID];
		List<GameObject> resourceUnits = gManager.GetComponent<unitIndex> ().resources;

		float distance = Mathf.Infinity;

		targetCount = enemies.Count;
		friendlyCount = friendlies.Count;
		nearestTarget = this.gameObject;


		if (_type == Type.mGun | _type == Type.missile) {
			GameObject closest = null;
			//	Vector3 position = transform.position;
			foreach (GameObject go in enemies) {
				if (go != null) {
					Vector3 diff = go.transform.position - position;
					float curDistance = diff.sqrMagnitude;
					if (curDistance < distance) {
						closest = go;
						distance = curDistance;
						nearestTarget = closest;
					}
				}
			}
		}
		if (_type == Type.miner) {
			GameObject closest = null;
			bool currClosest = true;
			float currLeader = Mathf.Infinity;
			foreach (GameObject go in gManager.GetComponent<unitIndex>().resources) {
				if (go != null) {
					float curDistance = Vector3.Distance (position, go.transform.position);
					if (curDistance < currLeader) {
						closest = go;
						distance = curDistance;
						currLeader = curDistance;
						nearestTarget = closest;
					}
				}
			}
		}
		if (_type == Type.utility) {
			GameObject closest = null;
			bool currClosest = true;
			//	Vector3 position = transform.position;
			float currLeader = Mathf.Infinity;
			foreach (GameObject go in betaStructs) {
				float curDistance = Vector3.Distance (position, go.transform.position);
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


	void turn(){
		
		if (target != null) {
			
			Vector3 dir = (target.transform.position - transform.position).normalized;
			dir.y = 0;
			Quaternion rotation = Quaternion.LookRotation (dir);
			transform.rotation = Quaternion.Slerp (transform.rotation, rotation, turnDamp * Time.deltaTime);
		}
	}

	void levelY(){

		Vector3 pos = transform.position;
		pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
		transform.position = pos;

	}

	bool checkMoving (){

		bool moving = true;
		r_rate -= Time.deltaTime;

		if (r_rate <= 0) {
			if (posInit == true) {
				pos1 = transform.position;
				posInit = false;
				r_rate = repathTimer;
				if (Vector3.Distance (pos1, pos2) < 2) {
					moving = false;
				}
			} else {
				pos2 = transform.position;
				posInit = true;
				r_rate = repathTimer;
				if (Vector3.Distance (pos1, pos2) < 2) {
					moving = false;
				}
			}
		}
		return moving;
	}


	bool checkDistance(){

		bool distance = true;

		if (target != null) {

			if (Vector3.Distance (transform.position, target.transform.position) > (attackRange * 1.3)) {

				if (_attackState == attackState.Engage) {
					eventManager.RequestPosition (this.gameObject, target, attackRange, playerID);
				}

				distance = false;

			} else {
				distance = true;
			}

		}

		return distance;
	}

	GameObject findRefinery(Vector3 position){

		List<GameObject> refineries = gManager.GetComponent<unitIndex> ().refineries [playerID];
		float distance = Mathf.Infinity;
		GameObject closest = null;
		GameObject nearestTarget = null;

		foreach (GameObject go in refineries) {
				if (go != null) {
					Vector3 diff = go.transform.position - position;
					float curDistance = diff.sqrMagnitude;
					if (curDistance < distance) {
						closest = go;
						distance = curDistance;
						nearestTarget = closest;
					}
				}
			}
		return nearestTarget;
	}

	void onNav(GameObject actor, Vector3 position){

		if (actor == this.gameObject) {

			navTarget = position;
			_moveState = moveState.moveInit;
			_state = State.Move;


		}

	}

	GameObject nextRepair(){

		List<GameObject> friendlies = gManager.GetComponent<unitIndex> ().units [playerID];
		List<GameObject> needRepair = new List<GameObject> ();
		float distance = Mathf.Infinity;
		GameObject closest = null;
		GameObject repTarget = null;
		int lowest = 10000;

		foreach (GameObject go in friendlies){
			if (go != null) {
				float curDistance = Vector3.Distance(go.transform.position, transform.position);
				if (curDistance < reTargetRange) {
					needRepair.Add (go);
					}
				}
			}
		foreach (GameObject part in needRepair) {

			if (part != null) {

				int hitPoints = part.GetComponent<unitAgent> ().health;

				if (hitPoints < lowest) {

					lowest = hitPoints;
					repTarget = part;
				
				}
			}
		}
		return repTarget;
		}

	void onRepair(int rep, GameObject sender, GameObject reciever){

		if (reciever = this.gameObject) {

			health += rep;

			if (health > hitPoints) {
				health = hitPoints;	
			}
		}
	}
	void onDestroy(GameObject unit, bool State){

		if (cMemory != null) {

			if (cMemory._type == Order.Type.Attack && cMemory.unitTarget == unit) {
				eventManager.ReturnOrder (cMemory, this.gameObject, 1, playerID);
				cMemory = null;
				return;
			}

			if (cMemory._type == Order.Type.Repair && cMemory.unitTarget == unit) {
				eventManager.ReturnOrder (cMemory, this.gameObject, 2, playerID);
				cMemory = null;
				return;
			}
				
		}
		
	}
	void onBuild(Vector3 position, int type){


		if (cMemory != null) {

			if (cMemory._type == Order.Type.Build && cMemory.unitTarget.tag == "Structure") {
				eventManager.ReturnOrder (cMemory, this.gameObject, 1, playerID);
				cMemory = null;
			}
		}

	}

	void cleanupChecks(){

		// Frame cleanup checks here.

		if (cMemory != null) {
			onMission = true;
		} else {
			onMission = false;
		}

		if (_state == State.Move) {
			isMoving = true;
		} else {
			isMoving = false;
			_moveState = moveState.moveInit;
		}

//		if (_Logic_1 == State.Attack && target == null) {
//			_state = _Logic_1;
//		}

	}
}
