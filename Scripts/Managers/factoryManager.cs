using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class factoryManager : MonoBehaviour {

	public List<buildQueue> buildList = new List<buildQueue>();
	public bool hasPower = true;
	public int playerID;
	public int techLevel;
	public float upgradeRange = 1000f;
	private resourceManager rManager;
	private float ticker = 1;
	private float workTicker =1;
	private bool check = false;
	public int queueLength;
	public float spawnOffset = 30;
	private Vector3 spawnPoint;
	public float buildTime = 10;
	public buildLogic bLogic;

	public buildQueue list1;
	public buildQueue list2;
	public buildQueue list3;
	public buildQueue list4;
	public buildQueue list5;
	public buildQueue list6;
	public buildQueue list7;
	public buildQueue list8;
	public buildQueue list9;
	public buildQueue list10;
	public buildQueue list11;

	void OnEnable(){
		eventManager.onFactoryOrder += newOrder;
		eventManager.onFactoryClear += clearList;
		eventManager.onServeFactoryOrder += AIorder;
		eventManager.onBuildInit += checkTech;
	}

	void OnDisable(){
		eventManager.onFactoryOrder -= newOrder;
		eventManager.onFactoryClear -= clearList;
		eventManager.onServeFactoryOrder -= AIorder;
		eventManager.onBuildInit -= checkTech;
	}

	void Start () {
		GameObject temp = GameObject.Find("resourceManager");
		bLogic = GetComponent<buildLogic> ();
		playerID = bLogic.playerID;
		rManager = temp.GetComponent<resourceManager> ();
		spawnPoint = bLogic.interfaceMarker.transform.position;
		techLevel = 1;
	}
	
	void Update () {
		if (buildList.Count > 0 && hasPower == true) {
			if (rManager.resource[playerID] > 0) {
				buildList[0].Progress -= Time.deltaTime;
				if (buildList[0].Progress < 0) {
					eventManager.UnitSpawn(buildList[0].Type.prefab,spawnPoint,playerID);
					Vector2 rand = new Vector2(Random.Range(-20,20),Random.Range(-5,5));
					spawnPoint = spawnPoint + transform.right * rand.x;
					eventManager.UnitCreate (this.gameObject, true);
					buildList [0] = null;
					buildList.RemoveAt(0);
					eventManager.FactoryConfirm (this.gameObject, unitSettings.miner1);
				}
			}
		}
	}
	void newOrder(GameObject factory, buildType type){
		//Debug.Log ("Stage 1");
		if(rManager.resource[playerID] > type.cost){
		if (factory == this.gameObject) {
			check = true;
				if (buildList.Count <12 && check == true) {
					buildQueue temp = new buildQueue (type, buildTime);
					buildList.Add (temp);
					eventManager.Spend (type.cost, playerID);
					eventManager.FactoryConfirm (factory, type);
					check = false;
					queueLength = buildList.Count;
				}
	
		}
	}
	}
	void clearList(GameObject factory, buildType type){
		if (factory == this.gameObject) {
			foreach (buildQueue order in buildList) {
				eventManager.Collect (order.Type.cost, playerID);
			}
			buildList.Clear();
		}
	}
	void spend(){
		ticker -= Time.deltaTime;
		if (ticker <= 0){
			eventManager.onSpend (1, 1);
			ticker = 1;
		}
	}

	void checkTech(Vector3 position, int type) {

		List<GameObject> allStructures = new List<GameObject> ();
		List<GameObject> myResearch = new List<GameObject> ();
		float distance = Mathf.Infinity;
		int highest = 1;

		allStructures.AddRange (GameObject.FindGameObjectsWithTag ("Structure"));

		foreach (GameObject part in allStructures) {

			if(part.GetComponent<researchLogic>() != null && part.GetComponent<buildLogic>().playerID == playerID){
				myResearch.Add (part);
			}
		}


		if (myResearch.Count == 0) {

			techLevel = 1;
			return;
		}

		else{
			
		foreach (GameObject station in myResearch) {

			float gap = Vector3.Distance (station.transform.position, transform.position);
			if (gap <= upgradeRange) {

				int stationTech = station.GetComponent<researchLogic> ().techLevel;

					if (highest < stationTech) {
					highest = stationTech;
					}
				}

			}

			techLevel = highest;
		}
}

	void AIorder(FactoryOrder order){

		if (order.ofactory == this.gameObject) {

			buildType miner = unitSettings.miner1;
			buildType missile = unitSettings.missile1;
			buildType mGun = unitSettings.mGun1;
			buildType utility = unitSettings.repair1;


			if (order.tLevel == 2) {
				miner = unitSettings.miner2;
				missile = unitSettings.missile2;
				mGun = unitSettings.mGun2;
				utility = unitSettings.repair2;
			}

			if (order.tLevel == 3) {
				miner = unitSettings.miner3;
				missile = unitSettings.missile3;
				mGun = unitSettings.mGun3;
				utility = unitSettings.repair3;
			}

			if (order.tLevel == 5) {
				miner = unitSettings.minerX;
				missile = unitSettings.missileX;
				mGun = unitSettings.mGunX;
				utility = unitSettings.repairX;

			}


			if (order.miner == 1) {
				AIOrder (miner);
			}
			if (order.missile == 1) {
				AIOrder (missile);
			}
			if (order.mgun == 1) {
				AIOrder (mGun);
			}
			if (order.utility == 1) {
				AIOrder (utility);
			}
		}
	}
	void AIOrder(buildType type){
		
			buildQueue temp = new buildQueue (type, buildTime);
			buildList.Add (temp);
			queueLength = buildList.Count;
		}
}
