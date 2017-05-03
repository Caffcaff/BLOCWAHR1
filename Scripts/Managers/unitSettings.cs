using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitSettings : MonoBehaviour {


	public static buildType miner1 = new buildType ("Miner T1");
	public static buildType miner2 = new buildType ("Miner T2");
	public static buildType miner3 = new buildType ("Miner T3");
	public static buildType mGun1 = new buildType ("Gunner T1");
	public static buildType mGun2 = new buildType ("Gunner T2");
	public static buildType mGun3 = new buildType ("Gunner T3");
	public static buildType missile1 = new buildType ("Missile T1");
	public static buildType missile2 = new buildType ("Missile T2");
	public static buildType missile3 = new buildType ("Missile T3");
	public static buildType repair1 = new buildType ("Utility T1");
	public static buildType repair2 = new buildType ("Utility T2");
	public static buildType repair3 = new buildType ("Utility T3");


	[Header("Miner Units")]
	public Sprite iconM1;
	public int costM1;
	public GameObject prefabM1;

	public Sprite iconM2;
	public int costM2;
	public GameObject prefabM2;

	public Sprite iconM3;
	public int costM3;
	public GameObject prefabM3;

	[Header("Gunner Units")]
	public Sprite iconG1;
	public int costG1;
	public GameObject prefabG1;

	public Sprite iconG2;
	public int costG2;
	public GameObject prefabG2;

	public Sprite iconG3;
	public int costG3;
	public GameObject prefabG3;

	[Header("Missile Units")]
	public Sprite iconR1;
	public int costR1;
	public GameObject prefabR1;

	public Sprite iconR2;
	public int costR2;
	public GameObject prefabR2;

	public Sprite iconR3;
	public int costR3;
	public GameObject prefabR3;


	[Header("Utility Units")]
	public Sprite iconU1;
	public int costU1;
	public GameObject prefabU1;

	public Sprite iconU2;
	public int costU2;
	public GameObject prefabU2;

	public Sprite iconU3;
	public int costU3;
	public GameObject prefabU3;


	// Use this for initialization
	void Start () {
		miner1.cost = costM1;
		miner1.icon = iconM1;
		miner2.cost = costM2;
		miner2.icon = iconM2;
		miner3.cost = costM3;
		miner3.icon = iconM3;
		mGun1.cost = costG1;
		mGun1.icon = iconG1;
		mGun2.cost = costG2;
		mGun2.icon = iconG2;
		mGun3.cost = costG3;
		mGun3.icon = iconG3;
		missile1.cost = costR1;
		missile1.icon = iconR1;
		missile2.cost = costR2;
		missile2.icon = iconR2;
		missile3.cost = costR3;
		missile3.icon = iconR3;
		repair1.cost = costU1;
		repair1.icon = iconU1;
		repair2.cost = costU2;
		repair2.icon = iconU2;
		repair3.cost = costU3;
		repair3.icon = iconU3;

		miner1.prefab = prefabM1;
		miner2.prefab = prefabM2;
		miner3.prefab = prefabM3;
		mGun1.prefab = prefabG1;
		mGun2.prefab = prefabG2;
		mGun3.prefab = prefabG3;
		missile1.prefab = prefabR1;
		missile2.prefab = prefabR2;
		missile3.prefab = prefabR3;
		repair1.prefab = prefabU1;
		repair2.prefab = prefabU2;
		repair3.prefab = prefabU3;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
