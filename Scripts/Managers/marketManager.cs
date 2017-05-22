using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class marketManager : MonoBehaviour {

	[Header("Struct Names")]
	public string HQ;
	public string powerStation;
	public string refinery;
	public string silo;
	public string recyclingDepot;
	public string repairDepot;
	public string gigaFactory;
	public string researchFacility;
	public string aircraftFactory;
	public string wall;
	public string sentryGun;
	public string gate;

	[Header("Unit Names")]
	public string miner1;
	public string miner2;
	public string miner3;
	public string minerX;
	public string missile1;
	public string missile2;
	public string missile3;
	public string missileX;
	public string mGun1;
	public string mGun2;
	public string mGun3;
	public string mGunX;
	public string utility1;
	public string utility2;
	public string utility3;
	public string utilityX;

	[Header("Struct Costs")]
	public int HQ_c = 500;
	public int powerStation_c = 500;
	public int refinery_c = 500;
	public int silo_c = 500;
	public int recyclingDepot_c = 500;
	public int repairDepot_c = 500;
	public int gigaFactory_c = 500;
	public int researchFacility_c = 500;
	public int aircraftFactory_c = 500;
	public int wall_c = 500;
	public int sentryGun_c = 500;
	public int gate_c = 500;

	[Header("Unit Costs")]
	public int miner1_c = 100;
	public int miner2_c = 100;
	public int miner3_c = 100;
	public int minerX_c = 100;
	public int missile1_c = 100;
	public int missile2_c = 100;
	public int missile3_c = 100;
	public int missileX_c = 100;
	public int mGun1_c = 100;
	public int mGun2_c = 100;
	public int mGun3_c = 100;
	public int mGunX_c = 100;
	public int utility1_c = 100;
	public int utility2_c = 100;
	public int utility3_c = 100;
	public int utilityX_c = 100;

	[Header("Building Models")]
	public GameObject HQ_m;
	public GameObject powerStation_m;
	public GameObject refinery_m;
	public GameObject silo_m;
	public GameObject recyclingDepot_m;
	public GameObject repairDepot_m;
	public GameObject gigaFactory_m;
	public GameObject researchFacility_m;
	public GameObject aircraftFactory_m;
	public GameObject wall_m;
	public GameObject sentryGun_m;
	public GameObject gate_m;

	[Header("Unit Models")]
	public GameObject miner1_m;
	public GameObject miner2_m;
	public GameObject miner3_m;
	public GameObject minerX_m;
	public GameObject missile1_m;
	public GameObject missile2_m;
	public GameObject missile3_m;
	public GameObject missileX_m;
	public GameObject mGun1_m;
	public GameObject mGun2_m;
	public GameObject mGun3_m;
	public GameObject mGunX_m;
	public GameObject utility1_m;
	public GameObject utility2_m;
	public GameObject utility3_m;
	public GameObject utilityX_m;

	[Header("Struct Icons")]
	public Sprite HQ_i;
	public Sprite powerStation_i;
	public Sprite refinery_i;
	public Sprite silo_i;
	public Sprite recyclingDepot_i;
	public Sprite repairDepot_i;
	public Sprite gigaFactory_i;
	public Sprite researchFacility_i;
	public Sprite aircraftFactory_i;
	public Sprite wall_i;
	public Sprite sentryGun_i;
	public Sprite gate_i;

	[Header("Unit Icons")]
	public Sprite miner1_i;
	public Sprite miner2_i;
	public Sprite miner3_i;
	public Sprite minerX_i;
	public Sprite missile1_i;
	public Sprite missile2_i;
	public Sprite missile3_i;
	public Sprite missileX_i;
	public Sprite mGun1_i;
	public Sprite mGun2_i;
	public Sprite mGun3_i;
	public Sprite mGunX_i;
	public Sprite utility1_i;
	public Sprite utility2_i;
	public Sprite utility3_i;
	public Sprite utilityX_i;

	public buildType[] units = new buildType[36];
	public buildType[] structs = new buildType[20];


	void Start () {

		structs[0] = new buildType ("unused");
		structs[1] = new buildType(HQ);
		structs[2] = new buildType(powerStation);
		structs[3] = new buildType(refinery);
		structs[4] = new buildType ("unused");
		structs[5] = new buildType(silo);
		structs[6] = new buildType(recyclingDepot);
		structs[7] = new buildType(repairDepot);
		structs[8] = new buildType(gigaFactory);
		structs[9] = new buildType(researchFacility);
		structs[10] = new buildType(aircraftFactory);
		structs[11] = new buildType(wall);
		structs[12] = new buildType(sentryGun);
		structs[13] = new buildType ("unused");
		structs[14] = new buildType ("unused");
		structs[15] = new buildType(gate);

		structs[1].cost = HQ_c;
		structs[2].cost = powerStation_c;
		structs[3].cost = refinery_c;
		structs[5].cost = silo_c;
		structs[6].cost = recyclingDepot_c;
		structs[7].cost = repairDepot_c;
		structs[8].cost = gigaFactory_c;
		structs[9].cost = researchFacility_c;
		structs[10].cost = aircraftFactory_c;
		structs[11].cost = wall_c;
		structs[12].cost = sentryGun_c;
		structs[15].cost = gate_c;

		structs[1].prefab = HQ_m;
		structs[2].prefab = powerStation_m;
		structs[3].prefab = refinery_m;
		structs[5].prefab = silo_m;
		structs[6].prefab = recyclingDepot_m;
		structs[7].prefab = repairDepot_m;
		structs[8].prefab = gigaFactory_m;
		structs[9].prefab = researchFacility_m;
		structs[10].prefab = aircraftFactory_m;
		structs[11].prefab = wall_m;
		structs[12].prefab = sentryGun_m;
		structs[15].prefab = gate_m;

		structs[1].icon = HQ_i;
		structs[2].icon = powerStation_i;
		structs[3].icon = refinery_i;
		structs[5].icon = silo_i;
		structs[6].icon = recyclingDepot_i;
		structs[7].icon = repairDepot_i;
		structs[8].icon = gigaFactory_i;
		structs[9].icon = researchFacility_i;
		structs[10].icon = aircraftFactory_i;
		structs[11].icon = wall_i;
		structs[12].icon = sentryGun_i;
		structs[15].icon = gate_i;

		units[0] = new buildType("unused");
		units[1] = new buildType(miner1); 
		units[2] = new buildType(miner2); 
		units[3] = new buildType(miner3);
		units[4] = new buildType("unused"); 
		units[5] = new buildType(minerX);
		units[6] = new buildType("unused"); 
		units[7] = new buildType("unused"); 
		units[8] = new buildType("unused"); 
		units[9] = new buildType("unused"); 
		units[10] = new buildType("unused"); 
		units[11] = new buildType(missile1);
   	  	units[12] = new buildType(missile2);
    	units[13] = new buildType(missile3);
     	units[15] = new buildType(missileX);
		units[16] = new buildType("unused"); 
		units[17] = new buildType("unused"); 
		units[18] = new buildType("unused"); 
		units[19] = new buildType("unused"); 
		units[20] = new buildType("unused");
		units[21] = new buildType(mGun1);
    	units[22] = new buildType(mGun2);
    	units[23] = new buildType(mGun3);
    	units[25] = new buildType(mGunX);
		units[26] = new buildType("unused"); 
		units[27] = new buildType("unused"); 
		units[28] = new buildType("unused"); 
		units[29] = new buildType("unused"); 
		units[30] = new buildType("unused");
		units[31] = new buildType(utility1);
		units[32] = new buildType(utility2);
		units[33] = new buildType(utility3);
		units[35] = new buildType(utilityX);

		units[1].cost = miner1_c; 
		units[2].cost = miner2_c; 
		units[3].cost = miner3_c; 
		units[5].cost = minerX_c; 
		units[11].cost = missile1_c;
		units[12].cost = missile2_c;
		units[13].cost = missile3_c;
		units[15].cost = missileX_c;
		units[21].cost = mGun1_c;
		units[22].cost = mGun2_c;
		units[23].cost = mGun3_c;
		units[25].cost = mGunX_c;
		units[31].cost = utility1_c;
		units[32].cost = utility2_c;
		units[33].cost = utility3_c;
		units[35].cost = utilityX_c;

		units[1].prefab = miner1_m;
		units[2].prefab = miner2_m;
		units[3].prefab = miner3_m;
		units[5].prefab = minerX_m;
		units[11].prefab = missile1_m;
		units[12].prefab = missile2_m;
		units[13].prefab = missile3_m;
		units[15].prefab = missileX_m;
		units[21].prefab = mGun1_m;
		units[22].prefab = mGun2_m;
		units[23].prefab = mGun3_m;
		units[25].prefab = mGunX_m;
		units[31].prefab = utility1_m;
		units[32].prefab = utility2_m;
		units[33].prefab = utility3_m;
		units[35].prefab = utilityX_m;

		units[1].icon = miner1_i;
		units[2].icon = miner2_i;
		units[3].icon = miner3_i;
		units[5].icon = minerX_i;
		units[11].icon = missile1_i;
		units[12].icon = missile2_i;
		units[13].icon = missile3_i;
		units[15].icon = missileX_i;
		units[21].icon = mGun1_i;
		units[22].icon = mGun2_i;
		units[23].icon = mGun3_i;
		units[25].icon = mGunX_i;
		units[31].icon = utility1_i;
		units[32].icon = utility2_i;
		units[33].icon = utility3_i;
		units[35].icon = utilityX_i;

		marketCache ();
	}
	void marketCache(){
		
		eventManager.MarketCache (1);
	}
}
