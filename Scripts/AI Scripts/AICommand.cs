using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICommand : MonoBehaviour {

	[Header("General Settings")]
	public int playerID = 2;
	public int techLevel = 1;
	public bool debugActive = false;
	public int outpostIndex;
	public enum State {
		Initialise,
		Setup,
		Idle,
		inGame
	}
	public State _state;

	[Header("Speed")]
	public float progressInterval = 3;
	private float pInterval;
	public float garrisonInterval = 3;
	private float gInterval;
	public float mineInterval = 120;
	private float m_Interval;
	public float cleanupInterval = 100;
	private float cu_Interval;

	public bool issueMineOrder = true;

	public List<baseAI> Outposts = new List<baseAI> ();
	public AIMap aiMap;

	public resourceManager rManager;
	public marketManager mManager;
	public buildManager bManager;

	bool mapInit = false;
	bool listInit = false;

	public bool initRun = true;

	void OnEnable(){
		eventManager.onMapAI += onMap;
		eventManager.onListInit += onList;
		eventManager.onReturnOrder += onReturnOrder;
		eventManager.onUnitEncounter += onEncounter;
		eventManager.onStructEncounter += onEncounter;

	}

	void OnDisable(){

		eventManager.onMapAI -= onMap;
		eventManager.onListInit -= onList;
		eventManager.onReturnOrder -= onReturnOrder;
		eventManager.onUnitEncounter -= onEncounter;
		eventManager.onStructEncounter -= onEncounter;

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
				idle ();
				break;
			case State.inGame:
				inGame ();
				break;
			}
			yield return 0;
		}
	}

	void initMe (){
		_state = State.Setup;
	}

	void inSetup (){
		aiMap = GetComponent<AIMap> ();

		GameObject temp = GameObject.Find ("resourceManager");
		GameObject temp2 = GameObject.Find ("Building Manager");
		rManager = temp.GetComponent<resourceManager> ();
		mManager = temp.GetComponent<marketManager> ();
		bManager = temp2.GetComponent<buildManager> ();

		pInterval = progressInterval;
		gInterval = garrisonInterval;
		cu_Interval = cleanupInterval;
		m_Interval = mineInterval;

		_state = State.Idle;
	}

	void idle (){

		if (mapInit == true && listInit == true) {
			_state = State.inGame;
		}
	
	}

	void inGame (){

		pInterval -= Time.deltaTime;
		gInterval -= Time.deltaTime;
		cu_Interval -= Time.deltaTime;
		m_Interval -= Time.deltaTime;

		if (pInterval <= 0) {
			inProgress ();
			pInterval = progressInterval;
		}

		if (gInterval <= 0) {
			inGarrison ();
			gInterval = garrisonInterval;
		}

		if (cu_Interval <= 0) {
			inCleanup ();
			cu_Interval = cleanupInterval;
		}

		if (m_Interval <= 0) {
			issueMineOrder = true;
			m_Interval = mineInterval;
		}

		// *** FOR FIRST TIME RUN ***

		if (initRun == true) {

			Order mineOrder = new Order (Order.Type.Mine, playerID, outpostIndex);
			eventManager.InitOrder (mineOrder, null, 0);

			inCleanup ();

			initRun = false;
		}







		}
	void inProgress(){

		foreach (baseAI outpost in Outposts) {

	//		Debug.Log ("step 1");

			if (outpost.ID <= outpostIndex) {

				int i = 0;

				while(i < outpost.structs.Count) {

					baseStruct currStruct = outpost.structs[i];

					if (currStruct.state != true) {
						if (currStruct.state == false) {

							if (currStruct.structure.GetComponent<buildLogic> ().built == true) {
								outpost.structs[i].state = true;
								if (i == 4) {
									Order reconOrder = new Order (Order.Type.Recon, playerID, (outpostIndex + 1));
									Debug.Log ("Recon Order");
									eventManager.InitOrder (reconOrder,null, 0);
									// Request Recon (playerID, outposts[(outpostIndex+1)]);
								}
							}
							i = outpost.structs.Count;
						}

						if (currStruct.state == null) {
							Debug.Log ("in build");
							GameObject tempGo = checkBuild (currStruct.type, outpost.ID);

							if (tempGo != null) {
								issueMineOrder = true;
								outpost.structs [i].structure = tempGo;
								outpost.structs [i].state = false;
							}
							else {
								if (issueMineOrder == true) {
									Order mineOrder = new Order (Order.Type.Mine, playerID, outpost.ID);
									eventManager.InitOrder (mineOrder, null, 0);
									issueMineOrder = false;
								}
							}
							i = outpost.structs.Count;
						} 
					}
						i++;
				}
			}
		}
	}

	void inGarrison(){

	}

	void inCleanup(){

		List<GameObject> allBetaStructs = new List<GameObject> ();
		List<GameObject> myBetaStructs = new List<GameObject> ();
		List<GameObject> jobBetaStructs = new List<GameObject> ();
		List<GameObject> cleanUpStructs = new List<GameObject> ();

		allBetaStructs.AddRange (GameObject.FindGameObjectsWithTag ("betaStructure"));

		foreach (GameObject part in allBetaStructs) {

			if (part.GetComponent<buildLogic> ().playerID == playerID) {
				myBetaStructs.Add (part);
			}

		}

		if (GetComponent<AIunitController> ().orderQueue.Count > 0) {
			
			foreach (Order order in GetComponent<AIunitController>().orderQueue) {

				if (order.unitTarget != null) {
					jobBetaStructs.Add (order.unitTarget);
				}
			}

			foreach (GameObject part in myBetaStructs) {

				if (jobBetaStructs.Contains (part) == false) {
					cleanUpStructs.Add (part);
				}
			}


			foreach (GameObject part in cleanUpStructs) {

				Order buildOrder = new Order (Order.Type.Build, playerID, 0);
				buildOrder.unitTarget = part;
				eventManager.InitOrder (buildOrder, null, 0);

			}
		} else {

			if (myBetaStructs.Count > 0) {

				foreach (GameObject part in myBetaStructs) {

					Order buildOrder = new Order (Order.Type.Build, playerID, 0);
					buildOrder.unitTarget = part;
					eventManager.InitOrder (buildOrder, null, 0);

				}
			}else{
				Debug.Log ("No Init Structs for player " + playerID);
			}

		}
	}

	void onMap(int ID){

		if (ID == playerID) {
			Outposts = aiMap.outposts;
			outpostIndex = 0;

			foreach (baseAI outpost in Outposts) {

				outpost.structs.Add (outpost.refinery);
				outpost.structs.Add (outpost.tower1);
				outpost.structs.Add (outpost.silo1);
				outpost.structs.Add (outpost.power);
				outpost.structs.Add (outpost.factory);
				outpost.structs.Add (outpost.tower2);
				outpost.structs.Add (outpost.silo2);
				outpost.structs.Add (outpost.research);
				outpost.structs.Add (outpost.tower3);
				outpost.structs.Add (outpost.hq);
				outpost.structs.Add (outpost.silo3);
				outpost.structs.Add (outpost.tower4);

			}
			mapInit = true;
		}
	}

	GameObject checkBuild(int type, int ID){
	
		/*
		[*** Cheat Sheet ***]
		
		HQ - 1
		Power Station - 2
		Refinery - 3
		Silo - 5
		Recycling Depot - 6
		Repair Depot - 7
		GigaFactory - 8
		Research Facility - 9
		Aircraft Factory - 10
		Wall - 11
		Sentry Gun - 12
		Gate - 15
*/

		if (mManager.structs [type].cost <= (rManager.resource [playerID] / 2)) {

			eventManager.Spend (mManager.structs [type].cost, playerID);

			baseAI outpost = Outposts [ID];
			buildSlot slot = new buildSlot(transform.position);
			int rand = 1;
			int R = 20;

			slot:
		
			if (type != 12 && type != 5) {
				rand = Random.Range (0, 8);
				slot = outpost.buildSlots [rand];
			}
			if (type == 5) {
				rand = Random.Range (8, 11);
				slot = outpost.buildSlots [rand];
			}
			if (type == 12) {
				rand = Random.Range (11,15);
				slot = outpost.buildSlots [rand];
			}

		
			if (slot.state == false) {
				Debug.Log ("Tried Build");
				Vector3 pos = slot.point;
				Vector3 relativePos = outpost.focal - pos;
				Quaternion rot = Quaternion.LookRotation(relativePos);
				GameObject temp = Instantiate (mManager.structs [type].prefab, pos, rot);
				outpost.buildSlots [rand].state = true;
				temp.GetComponent<buildLogic> ().playerID = playerID;
				temp.GetComponent<buildLogic> ().outpostID = outpost.ID;

				Order buildOrder = new Order (Order.Type.Build, playerID, outpostIndex);
				buildOrder.unitTarget = temp;
				buildOrder.navTarget = pos;

				eventManager.InitOrder (buildOrder,null, 0);
			
				return temp;

			} else {
				Debug.Log ("loopback slot");
				R--;
				if (type == 5) {
					Debug.Log ("Attempts Left: " + R);
				}
				if (R > 0) {
					goto slot;
				} else {
					return null;
				}
			}

		} else {

			if (debugActive) {
				Debug.Log ("Too Expensive" + " Cost:" + mManager.structs [type].cost + " Money:" + (rManager.resource [playerID] / 2));
			}
			return null;

		}
	}

	void onReturnOrder(Order order, GameObject[] units, int StatusCode){

		if (order.playerID == playerID) {

			if (StatusCode != 1) {
				eventManager.InitOrder (order, null, 0);
				return;
			}

			if (StatusCode == 1 && order._type == Order.Type.Recon) {

				order._type = Order.Type.Patrol;

				order.patrolA = Outposts [order.outpostID].focal; 
				order.patrolB = Outposts [order.outpostID+1].focal;

				outpostIndex = order.outpostID;


				eventManager.InitOrder (order, null, 0);
				return;
			}

			return;
			}


		}

	void onEncounter(navMemory scan, int ID){

		if (ID == playerID) { 

			Order attack = new Order(Order.Type.Attack, playerID, outpostIndex);
			attack.navTarget = scan.location;

			eventManager.InitOrder (attack, null, 0);
		}
	}

	void onList (int type){

		listInit = true;

	}	

	}
