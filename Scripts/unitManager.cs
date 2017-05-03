using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class unitManager : MonoBehaviour {

	[Header("Player Information")]
	public int ownerID;

	[Header("AI Targets")]
	public GameObject followTarget;
	public GameObject attackTarget;
	public Vector3 gndAttackTarget;
	[SerializeField] selectionManager sManager;
	[SerializeField] float aRange = 10;
	[Header("Weapon Components")]
	public GameObject[] launchers;
	public GameObject missile;
	public Material lightUp;
	public Material chrome;
	public enum Type {
		Miner,
		mGun,
		Missile
	}
	[Header("Unit Type Settings")]
	public Type _type;
	//Player StateMachine States
	public enum State {
		Initialise,
		Setup,
		Idle,
		Move,
		Follow,
		Attack,
		Done
	}

	public State _state;

	//Player Stats
	[Header("Game Stats")]
	public int health;
	public int ammo;
	[Range(0.0f, 150.0f)]
	public float fireRate = 20;
	private float ticker = 60;
	[Range(0.0f, 150.0f)]
	public int damage = 5;
	public bool canFire;
	public bool selectState;
	public int EXP;

	//Pathfinding Variables
	public float autotargetRange = 100.0f;
	[Header("Movement Settings")]
	public float distance;
	public Vector3 targetPosition;
	private Seeker seeker;
	private CharacterController controller;
	// The calculated path
	public Path path;
	// The AI's speed in meters per second
	public float speed = 2;
	// The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 3;
	// The waypoint we are currently moving towards
	private int currentWaypoint = 0;
	// How often to recalculate the path (in seconds)
	public float repathRate = 0.5f;
	private float lastRepath = -9999;
	public float followrate =0;
	public float turnDamp = 1;
	public float followreset = 3;
	private bool isMining = false;
	public GameObject Miner;
	private Renderer mrender;
	public GameObject resourceGatherObj;

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
			case State.Move:
				moveState ();
				break;
			case State.Follow:
				followState ();
				break;
			case State.Attack:
				attackState ();
				break;
			case State.Done:
				doneState ();
				break;
			}
			yield return 0;
		}	
	}
		
	void OnEnable(){
		eventManager.onNavArray += navSet;
		eventManager.onFollowClick += navFollow;
		eventManager.onAttackClick += attackClick;
		eventManager.onGroundAttackClick += groundAttackClick;
		eventManager.onSelectEvent += selectEvent;
		eventManager.onDamage += DamageEvent;


		Debug.Log("OnEnabled", gameObject);

	}

	void OnDisable(){
		eventManager.onNavArray -= navSet;
		eventManager.onFollowClick -= navFollow;
		eventManager.onAttackClick -= attackClick;
		eventManager.onGroundAttackClick -= groundAttackClick;
		eventManager.onSelectEvent += selectEvent;
		eventManager.onDamage -= DamageEvent;

		Debug.Log("OFF", gameObject);
		
	}

	private void initMe()
	{
		Debug.Log ("Initialise");
		_state = State.Setup;
	}

	private void setMeUp()
	{
		//PlayerManager Variables

		if (sManager == null) {
			GameObject owner = GameObject.FindWithTag("SelectionManager");
			sManager = owner.GetComponent<selectionManager> ();
		}
		if (Miner != null) {
			mrender = Miner.GetComponent<Renderer> ();
		}

		// Pathfinding Variables

		seeker = GetComponent<Seeker>();
		controller = GetComponent<CharacterController>();
		targetPosition = transform.position;

		//Follow target

		Debug.Log ("Setup");
		_state = State.Idle;
	}

	private void idleState()
	{
		Debug.Log ("Idle", gameObject);
	}

	private void moveState()
	{
		move();
	}

	void navSet(Vector3 point, GameObject actor, bool leader){
		if (selectState == true && actor == gameObject) {
			targetPosition = point;
			Debug.Log ("New Position Recieved");
			_state = State.Move;
		}		
	}

	void navFollow (Vector3 point, GameObject actor) {
		if (selectState == true) {
			targetPosition = actor.transform.position;
			followTarget = actor;
			_state = State.Follow;
		}
	}

	public void OnPathComplete (Path p) {
		Debug.Log("A path was calculated. Did it fail with an error? " + p.error);
		if (!p.error) {
			path = p;
			// Reset the waypoint counter so that we start to move towards the first point in the path
			currentWaypoint = 0;
		}
	}

	private void followState()
	{
		followrate = followrate - Time.deltaTime;
		if (followrate < 0) {
			targetPosition = followTarget.transform.position;
			followrate = followreset;
			Debug.Log ("Follow Update");
		}
		move ();
	}

	private void attackState()
	{
		if (attackTarget != null) {
			followrate = followrate - Time.deltaTime;
			if (followrate < 0) {
				targetPosition = attackTarget.transform.position;
				followrate = followreset;
				Debug.Log ("Attack Follow Update");
			}
			float distance = Vector3.Distance (transform.position, attackTarget.transform.position);
			if (distance > aRange) {
				move ();
			}

			if (distance <= aRange + 3) {
				turn ();
				ticker = ticker - (Time.deltaTime * fireRate);
				if (ticker <= 0) {
					canFire = true;
				} else
					canFire = false;
				if (canFire && _type != Type.Missile && _type != Type.mGun) {
					dealDamage ();
					ticker = 60;
				}
				if (canFire && _type == Type.Miner)
				{
					if (isMining == false) {
						mineLight ();
						foreach (GameObject launcher in launchers) {
							var mining = Instantiate (resourceGatherObj, Miner.transform.position, transform.rotation);
							mining.transform.parent = launcher.transform;
							isMining = true;
						}
					}
						dealDamage ();
						ticker = 60;
						canFire = false;
				}
				if (canFire && _type == Type.mGun)
				{
					foreach (GameObject launcher in launchers) {
						eventManager.ParticleEvent (launcher.transform.position, attackTarget.transform.position, 1);
						dealDamage ();
						ticker = 60;
						canFire = false;
					}
				}
				if (canFire && _type == Type.Missile)
				{
					foreach (GameObject launcher in launchers) {
					var boomer = Instantiate (missile, launcher.transform.position, launcher.transform.rotation);
						boomer.transform.parent = gameObject.transform;
						ticker = 60;
						canFire = false;
					}
				}
			}
		}
		else {
			if (_type == Type.Miner){
				FindNextResource ();}
			if (_type != Type.Miner) {
				FindNextTarget ();
			}
			Debug.Log ("Actions");
		}
	}

	private void doneState()
	{
		Debug.Log ("Attacking", gameObject);
	}
		
	void attackClick (Vector3 point, GameObject actor) {
		if (selectState == true) {
			attackTarget = actor;
			_state = State.Attack;
			followrate = 0;
		}
	}
	void groundAttackClick (Vector3 point, GameObject actor) {
		if (selectState == true) {
			gndAttackTarget = point;
			_state = State.Attack;
			followrate = 0;
		}
	}
	public void selectEvent (GameObject unused)
	{
		if (sManager.currentSelection.Contains (this.gameObject)) {
			selectState = true;
		} else {
			selectState = false;
		}
	}
	void dealDamage(){
		Debug.Log ("DealDamage");
		eventManager.Damage (damage, this.gameObject, attackTarget);
	}
	public void DamageEvent(int hitpoints, GameObject sender, GameObject reciever){

		Debug.Log ("DamageEvent");

		//Taking Damage

		if (reciever == this.gameObject) {
			health = (health - hitpoints);
			Debug.Log ("Damage Recieved");
		}
		// Collecting Resource
		if (sender == this.gameObject) {

			if (reciever.tag == "Resource" && _type == Type.Miner) {
				Debug.Log ("Mining");
				eventManager.Collect (damage, 1);
			}
			if (reciever.tag == "ResourceG" && _type == Type.Miner) {
				Debug.Log ("THARBEGOLDINDEMHILLS");
				eventManager.Collect (damage, 2);
			}
			if (reciever.tag != "Resource" && reciever.tag != "ResourceG" && reciever.tag != "Friendly") {
				EXP++;
			}
		}
	}
	void move()
	{
		if (Time.time - lastRepath > repathRate && seeker.IsDone()) {
			lastRepath = Time.time+ Random.value*repathRate*0.5f;
			// Start a new path to the targetPosition, call the the OnPathComplete function
			// when the path has been calculated (which may take a few frames depending on the complexity)
			seeker.StartPath(transform.position, targetPosition, OnPathComplete);
		}
		if (path == null) {
			// We have no path to follow yet, so don't do anything
			return;
		}
		if (currentWaypoint > path.vectorPath.Count) return;
		if (currentWaypoint == path.vectorPath.Count) {
			Debug.Log("End Of Path Reached");
			currentWaypoint++;
			return;
		}
		// Direction to the next waypoint
		Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
		dir.y = 0;
		Quaternion rotation = Quaternion.LookRotation(dir);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnDamp * Time.deltaTime);
		dir *= speed;
		// Note that SimpleMove takes a velocity in meters/second, so we should not multiply by Time.deltaTime
		controller.SimpleMove(dir);
		// The commented line is equivalent to the one below, but the one that is used
		// is slightly faster since it does not have to calculate a square root
		//if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
		if ((transform.position-path.vectorPath[currentWaypoint]).sqrMagnitude < nextWaypointDistance*nextWaypointDistance) {
			currentWaypoint++;
			return;
		}
		Debug.Log("Moving", gameObject);
	}

	void turn(){
		Vector3 dir = (targetPosition-transform.position).normalized;
		dir.y = 0;
		Quaternion rotation = Quaternion.LookRotation(dir);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnDamp * Time.deltaTime);
	}


	void FindNextResource ()
	{
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag ("Resource");
		GameObject closest = null;
		distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject go in gos) {
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				closest = go;
				distance = curDistance;
				attackTarget = closest;
				Debug.Log ("Finding new target");
			} 
		}
		if (distance > autotargetRange) {
			_state = State.Idle;
			Debug.Log ("No Targets");
		}
	}
	void FindNextTarget ()
	{
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag ("Enemy");
		GameObject closest = null;
		distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject go in gos) {
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				closest = go;
				distance = curDistance;
				attackTarget = closest;

			} 
		}
		if (distance > autotargetRange) {
			_state = State.Idle;
		}
	}
	void mineLight(){
		mrender.materials[0] = lightUp;
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == 12) {
			Physics.IgnoreCollision (collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
		}
	}
}