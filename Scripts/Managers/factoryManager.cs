using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class factoryManager : MonoBehaviour {

	public List<buildQueue> buildList = new List<buildQueue>();
	public bool hasPower = true;

	private resourceManager rManager;
	private float ticker = 1;
	private float workTicker =1;
	private bool check = false;
	public int queueLength;
	public GameObject spawn;
	private Vector3 spawnPoint;
	public float spawnScatter = 5;
	public float buildTime = 10;

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
	}

	void OnDisable(){
		eventManager.onFactoryOrder -= newOrder;
		eventManager.onFactoryClear -= clearList;
	}

	void Start () {
		GameObject temp = GameObject.Find("resourceManager");
		rManager = temp.GetComponent<resourceManager> ();
	}
	
	void Update () {
		if (buildList.Count > 0 && hasPower == true) {
				if (rManager.playerResource > 0) {
				buildList[0].Progress -= Time.deltaTime;
				if (buildList[0].Progress < 0) {
					Vector3 temp = new Vector3 (Random.Range (-spawnScatter, spawnScatter), 0, 0);
					spawnPoint = spawn.transform.position + temp;
					eventManager.UnitSpawn(buildList[0].Type.prefab,spawnPoint);
					eventManager.UnitCreate (this.gameObject, true);
					buildList [0] = null;
					buildList.RemoveAt(0);
					eventManager.FactoryConfirm (this.gameObject, unitSettings.miner1);
				}
			}
		}
	}
	void newOrder(GameObject factory, buildType type){
		Debug.Log ("Stage 1");
		if(rManager.playerResource > type.cost){
		if (factory == this.gameObject) {
			check = true;
				if (buildList.Count <12 && check == true) {
					buildQueue temp = new buildQueue (type, buildTime);
					buildList.Add (temp);
					eventManager.Spend (type.cost, 1);
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
				eventManager.Collect (order.Type.cost, 1);
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
}
