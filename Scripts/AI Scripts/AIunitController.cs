using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIunitController : MonoBehaviour {


	public int playerID;
	public bool debugActive = true;
	public AICommand command;

	public bool isLive = false;

	public float interval = 1;
	private float c_interval;

	public List<GameObject> miners = new List<GameObject>();
	public List<GameObject> mGuns = new List<GameObject>();
	public List<GameObject> missiles = new List<GameObject>();
	public List<GameObject> utilities = new List<GameObject>();

	public List<GameObject> idle_miners = new List<GameObject>();
	public List<GameObject> idle_missiles = new List<GameObject>();
	public List<GameObject> idle_mGuns = new List<GameObject>();
	public List<GameObject> idle_utilities = new List<GameObject>();

	public List<Order> orderQueue = new List<Order>();

	public GameObject[] currSelect;


	[Header("Recon Loadout")]
	private missionLoadout reconLoadout = new missionLoadout (0, 0, 0, 0, 0);
	public int r_Miners = 0;
	public int r_Gunners = 2;
	public int r_Missiles = 0;
	public int r_Utilities = 0;
	public int r_tech = 1;

	[Header("Mining Loadout")]
	private missionLoadout miningLoadout = new missionLoadout (0, 0, 0, 0, 0);
	public int M_Miners = 5;
	public int M_Gunners = 1;
	public int M_Missiles = 2;
	public int M_Utilities = 0;

	[Header("Building Loadout")]
	private missionLoadout buildingLoadout = new missionLoadout (0, 0, 0, 0, 0);
	public int b_Miners = 0;
	public int b_Gunners = 2;
	public int b_Missiles = 2;
	public int b_Utilities = 2;

	[Header("Patrol Loadout")]
	private missionLoadout patrolLoadout = new missionLoadout (0, 0, 0, 0, 0);
	public int p_Miners = 0;
	public int p_Gunners = 4;
	public int p_Missiles = 2;
	public int p_Utilities = 0;

	public GameObject gManager;

	void OnEnable(){

		eventManager.onUnitCache += reCacheUnits;
		eventManager.onInitOrder += onCommand;
		eventManager.onListInit += onInit;
	}

	void OnDisable(){

		eventManager.onUnitCache += reCacheUnits;
		eventManager.onInitOrder -= onCommand;
		eventManager.onListInit -= onInit;
	}

	// Use this for initialization
	void Start () {

		gManager = GameObject.Find ("gameManager");
		c_interval = interval;
		command = GetComponent<AICommand> ();
		playerID = command.playerID;
	}
	
	// Update is called once per frame
	void Update () {

		if (isLive) {

			c_interval -= Time.deltaTime;

			if (c_interval <= 0) {
				c_interval = interval;
				actionList ();
			}
		}
	}
	public void reCacheCheck(GameObject unused, Vector3 Unused, int ID){
	//	if (ID == playerID) {
	//		reCacheUnits ();
	//	}
	}

	public void reCacheUnits (int ID){

		miners.Clear ();
		mGuns.Clear ();
		missiles.Clear ();
		utilities.Clear ();

		idle_miners.Clear ();
		idle_mGuns.Clear ();
		idle_missiles.Clear ();
		idle_utilities.Clear ();


		foreach (GameObject unit in gManager.GetComponent<unitIndex>().units[playerID]) {

			if (unit != null) {

				unitAgent ai = unit.GetComponent<unitAgent> ();

				if (ai._type == unitAgent.Type.mGun) {
					mGuns.Add (unit);
					if (ai.onMission == false) {
						idle_mGuns.Add (unit);
					}
				}
				if (ai._type == unitAgent.Type.missile) {
					missiles.Add (unit);
					if (ai.onMission == false) {
						idle_missiles.Add (unit);
					}
				}
				if (ai._type == unitAgent.Type.miner) {
					miners.Add (unit);
					if (ai.onMission == false) {
						idle_miners.Add (unit);
					}
				}
				if (ai._type == unitAgent.Type.utility) {
					utilities.Add (unit);
					if (ai.onMission == false) {
						idle_utilities.Add (unit);
					}
				}
			}
		}
	}

	void actionList(){

		if (orderQueue.Count != 0) {

			reCacheUnits (5);

			Order currOrder = orderQueue [0];
			missionLoadout loadout = new missionLoadout (0, 0, 0, 0, 0);
			List<GameObject> missionUnits = new List <GameObject> ();

			int factory_mgun = 0;
			int factory_missile = 0;
			int factory_utility = 0;
			int factory_miner = 0;

			if (currOrder._type == Order.Type.Recon) {
				Debug.Log ("Set Recon Loadout");
				loadout = reconLoadout;
			}
			if (currOrder._type == Order.Type.Mine) {
				loadout = miningLoadout;
			}
			if (currOrder._type == Order.Type.Build) {
				Debug.Log ("Set build Loadout");
				loadout = buildingLoadout;
			}
			if (currOrder._type == Order.Type.Patrol) {
				loadout = patrolLoadout;
			}
			if (currOrder._type == Order.Type.Attack) {
				loadout = new missionLoadout(0,3,3,0,1);
			}

			//Check Mgun Amount

			if (idle_mGuns.Count >= loadout.mGun) {
				int i = 0;
				while (i < loadout.mGun) {
					missionUnits.Add (idle_mGuns [i]);
					Debug.Log ("Added mGun Units");
					i++;
				}
			}

			else{
				Debug.Log ("Not enough MGuns");
				factory_mgun = (loadout.mGun - idle_mGuns.Count);
			}


			//Check Miner Amount

			if (idle_miners.Count >= loadout.miners) {
				int i = 0;
				while (i < loadout.miners) {
					missionUnits.Add (idle_miners [i]);
					i++;
				}
			}
			else{ factory_miner = (loadout.miners - idle_miners.Count);}


			//Check Missile Amount

			if (idle_missiles.Count >= loadout.missiles) {
				int i = 0;
				while (i < loadout.missiles) {
					missionUnits.Add (idle_missiles [i]);
					i++;
				}
			}
			else{ factory_missile = (loadout.missiles - idle_missiles.Count);}


			//Check Utility Amount

			if (idle_utilities.Count >= loadout.utilities) {
				int i = 0;
				while (i < loadout.utilities) {
					missionUnits.Add (idle_utilities [i]);
					Debug.Log ("Added Units");
					i++;
				}
			}
			else{ 
				factory_utility = (loadout.utilities - idle_utilities.Count);	
				Debug.Log ("added units to factory order");
			}
				


			if ((factory_mgun + factory_miner + factory_missile + factory_utility) < 1 ) {

				GameObject[] units = missionUnits.ToArray ();
				foreach (GameObject unit in units) {
				eventManager.SelectEvent (unit, playerID);
				}
				currSelect = units;
				serveOrder (currOrder, units);
				orderQueue.RemoveAt (0);
				Debug.Log ("ORDER: "+ currOrder._type + " order served to " + missionUnits.Count + " units.");
				reCacheUnits (5);
				return;

				}
			else
			{
				Debug.Log ("not enough units");

				if (currOrder.onHold == true) {
					orderQueue.RemoveAt (0);
					orderQueue.Add (currOrder);
					return;
				}

				currOrder.onHold = true;

				FactoryOrder factoryReq = new FactoryOrder (playerID, command.techLevel, factory_miner, factory_mgun, factory_missile, factory_utility);

				eventManager.AIFactoryOrder (factoryReq);

				orderQueue.RemoveAt (0);
				orderQueue.Add (currOrder);
				return;

			}

		}
	}

	void onCommand(Order order, GameObject unit, int statusCode, int ID){

		if(order.playerID == playerID){

			if (order._type == Order.Type.Recon) {

				order.navTarget = command.Outposts [order.outpostID].focal;
				order.patrolA = order.navTarget;
				order.patrolB = command.Outposts[order.outpostID-1].focal;

				orderQueue.Add(order);
			}

			if (order._type == Order.Type.Attack) {
				// Type Specicific Settings
				orderQueue.Add(order);
			}

			if (order._type == Order.Type.Move) {
				// Type Specicific Settings
				orderQueue.Add(order);
			}

			if (order._type == Order.Type.Patrol) {
				// Type Specicific Settings
				orderQueue.Add(order);
			}

			if (order._type == Order.Type.Mine) {
				// Type Specicific Settings
				order.navTarget = command.Outposts [order.outpostID].focal;

				order.unitTarget = nearestResource (order.navTarget);

				order.patrolA = command.Outposts [order.outpostID].buildSlots [11].point;
				order.patrolB = command.Outposts [order.outpostID].buildSlots [12].point;

				orderQueue.Add(order);
			}

			if (order._type == Order.Type.Build) {
				// Type Specicific Settings
				orderQueue.Add(order);
			}
		
		}

	}
	void setupLoadouts(){

		reconLoadout.mGun = r_Gunners;
		reconLoadout.miners = r_Miners;
		reconLoadout.missiles = r_Missiles;
		reconLoadout.utilities = r_Utilities;
		reconLoadout.techLevel = r_tech;

		miningLoadout.mGun = M_Gunners;
		miningLoadout.miners = M_Miners;
		miningLoadout.missiles = M_Missiles;
		miningLoadout.utilities = M_Utilities;
		miningLoadout.techLevel = command.techLevel;

		patrolLoadout.mGun = p_Gunners;
		patrolLoadout.miners = p_Miners;
		patrolLoadout.missiles = p_Missiles;
		patrolLoadout.utilities = p_Utilities;
		patrolLoadout.techLevel = command.techLevel;

		buildingLoadout.mGun = b_Gunners;
		buildingLoadout.miners = b_Miners;
		buildingLoadout.missiles = b_Missiles;
		buildingLoadout.utilities = b_Utilities;
		buildingLoadout.techLevel = command.techLevel;

	}

	void onInit(int ID){

		reCacheUnits (5);
		setupLoadouts ();

		isLive = true;

	}

	void serveOrder(Order order, GameObject[] units){

		if (order._type != Order.Type.Patrol) {

			eventManager.ProcessOrder (order, units);

		} else {


			foreach (GameObject unit in units) {

				eventManager.ServeOrder (order, unit, 0, playerID);

			}

		}

	}

	GameObject nearestResource (Vector3 point)
	{
		float distance = Mathf.Infinity;
		GameObject closest = new GameObject ();
		List<GameObject> resos = gManager.GetComponent<unitIndex> ().resources;

		if (resos.Count > 0) {

			foreach (GameObject resource in resos) {
				if(resource != null){
				float curr = Vector3.Distance (resource.transform.position, point);

				if (curr < distance) {
					closest = resource;
					distance = curr;
				}
			}
		}

			return closest;
		} else {
		}
		if(debugActive){
			Debug.Log ("No resource or no List Init");
				}
		return null;
	}
}
