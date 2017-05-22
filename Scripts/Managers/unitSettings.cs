using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitSettings : MonoBehaviour {


	public static buildType miner1 = new buildType ("Miner T1");
	public static buildType miner2 = new buildType ("Miner T2");
	public static buildType miner3 = new buildType ("Miner T3");
	public static buildType minerX = new buildType ("Miner TX");

	public static buildType mGun1 = new buildType ("Gunner T1");
	public static buildType mGun2 = new buildType ("Gunner T2");
	public static buildType mGun3 = new buildType ("Gunner T3");
	public static buildType mGunX = new buildType ("Gunner TX");

	public static buildType missile1 = new buildType ("Missile T1");
	public static buildType missile2 = new buildType ("Missile T2");
	public static buildType missile3 = new buildType ("Missile T3");
	public static buildType missileX = new buildType ("Missile TX");

	public static buildType repair1 = new buildType ("Utility T1");
	public static buildType repair2 = new buildType ("Utility T2");
	public static buildType repair3 = new buildType ("Utility T3");
	public static buildType repairX = new buildType ("Utility TX");

	public marketManager mManager;

	void OnEnable(){
		eventManager.onMarketCache += marketCache;
	}

	void OnDisable(){
		
		eventManager.onMarketCache -= marketCache;
	}


	// Use this for initialization
	void Start () {
		mManager = this.gameObject.GetComponent<marketManager> ();
	}

	void marketCache(int ID){

		miner1 = mManager.units[1];
		miner2 = mManager.units[2];
		miner3 = mManager.units[3];
		minerX = mManager.units[5];

		missile1 = mManager.units[11];
		missile2 = mManager.units[12];
		missile3 = mManager.units[13];
		missile3 = mManager.units[15];

		mGun1 = mManager.units[21];
		mGun2 = mManager.units[22];
		mGun3 = mManager.units[23];
		mGun3 = mManager.units[25];

		repair1 = mManager.units[31];
		repair2 = mManager.units[32];
		repair3 = mManager.units[33];
		repair3 = mManager.units[35];

	}
}
