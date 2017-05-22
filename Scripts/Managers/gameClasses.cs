using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperMethods
{
	public static List<GameObject> GetChildren(this GameObject go)
	{
		List<GameObject> children = new List<GameObject>();
		foreach (Transform tran in go.transform)
		{
			children.Add(tran.gameObject);
		}
		return children;
	}
}

public sealed class FactoryOrder{

	public int playerID;
	public int tLevel;
	public int miner;
	public int mgun;
	public int missile;
	public int utility;
	public GameObject ofactory;

	public FactoryOrder (int PlayerID, int TLevel, int Miner, int Mgun, int Missile, int Utility){

		playerID = PlayerID;
		tLevel = TLevel;
		miner = Miner;
		mgun = Mgun;
		missile = Missile;
		utility = Utility;
		ofactory = null;

	}

}
	

public sealed class UnitClass{
	int unitID;
	int Type;
	int hitPoints;
	Vector3 unitLocation;
	int State;
	int targetID;
	int EXP;
	Vector3 navTarget;
}

public sealed class ResourceClass{
	int unitID;
	int Type;
	int hitPoints;
	Vector3 unitLocation;
}
public class buildType{
	public string name;
	public int cost;
	public Sprite icon;
	public GameObject prefab;

	public buildType(string Name){
		name = Name;
		cost = 500;
		icon = null;
		prefab = null;
	}
}

public sealed class missionLoadout{

	public int techLevel;
	public int miners;
	public int mGun;
	public int missiles;
	public int utilities;

	public missionLoadout (int Miners, int Mgun, int Missiles, int Utilities, int Tlevel){

		techLevel = Tlevel;
		miners = Miners;
		mGun = Mgun;
		missiles = Missiles;
		utilities = Utilities;

	}

}




public sealed class Order{

	public int playerID;
	public int outpostID;
	public Vector3 navTarget;
	public GameObject unitTarget;

	public Vector3 patrolA;
	public Vector3 patrolB;

	public bool onHold;

	public enum Type {
		Recon,
		Move,
		Mine,
		Build,
		Attack,
		Patrol
	}
	public Type _type;

	public Order(Type orderType,int PlayerID, int OutpostID){

		_type = orderType;
		playerID = PlayerID;
		outpostID = OutpostID;
		navTarget = new Vector3 (0, 0, 0);
		unitTarget = null;
		patrolA = new Vector3 (0, 0, 0);
		patrolB = new Vector3 (0, 0, 0);
		onHold = false;

	}
}

public class scanInstance{
	public bool hasTarget;
	public GameObject nearest;
	public int friendlyCount;
	public int enemyCount;

	public scanInstance(bool hastarget, GameObject Nearest, int friendlycount, int enemycount){
		hasTarget = hastarget;
		nearest = Nearest;
		friendlyCount = friendlycount;
		enemyCount = enemycount;
	}
}

public sealed class baseStruct{

	public GameObject structure;
	public bool? state;
	public int type;

	public baseStruct(int Type){
		structure = null;
		state = null;
		type = Type;
	}
}

public sealed class baseAI{
	public int ID;
	public Vector3 focal;
	public baseStruct hq;
	public baseStruct refinery;
	public baseStruct power;
	public baseStruct factory;
	public baseStruct research;
	public baseStruct silo1;
	public baseStruct silo2;
	public baseStruct silo3;
	public baseStruct tower1;
	public baseStruct tower2;
	public baseStruct tower3;
	public baseStruct tower4;

	public List<buildSlot> buildSlots;
	public List<baseStruct> structs;

	public baseAI(Vector3 Focal, List<buildSlot> BuildSlots, int id){

		ID = id;
		focal = Focal;
		buildSlots = BuildSlots;
		hq = new baseStruct(1);
		refinery = new baseStruct(3);
		power = new baseStruct(2);
		factory = new baseStruct(8);
		research = new baseStruct(9);
		silo1 = new baseStruct(5);
		silo2 = new baseStruct(5);
		silo3 = new baseStruct(5);
		tower1 = new baseStruct(12);
		tower2 = new baseStruct(12);
		tower3 = new baseStruct(12);
		tower4 = new baseStruct(12);

		structs = new List<baseStruct> ();
	}
}
public sealed class buildSlot{
	public Vector3 point;
	public bool state;

	public buildSlot(Vector3 Point){
		point = Point;
		state = false;
	}
}

public sealed class baseLocale{
	public Vector3 focal;
	public List<Vector3> slots;
	public List<Vector3> silo;
	public List<Vector3> towers;
	public List<Vector3> walls;

	public baseLocale(Vector3 Focal){
		focal = Focal;
		slots = new List<Vector3>();
		silo = new List<Vector3>();
		towers = new List<Vector3>();
		walls = new List<Vector3>();
	}
}

public class navMemory{
	public Vector3 location;
	public int friendlyCount;
	public int enemyCount;
	public int resourceCount;

	public navMemory(Vector3 point, int friendlycount, int enemycount, int resourcecount){
		location = point;
		friendlyCount = friendlycount;
		enemyCount = enemycount;
		resourceCount = resourcecount;
	}
}



public class buildQueue{
	public buildType Type;
	public float Progress;

	public buildQueue(buildType type, float progress){
		Type = type;
		Progress = progress;
	}

}

public sealed class Structures{
	int buildingID;
	int ownerID;
	int Type;
	Vector3 BuildLocation;
	Vector3 BuildRotation;
	int TechLevel;
	buildQueue[] buildQueue;
}

public sealed class RTSCam{
	int camID;
	int ownerID;
	Vector3 camPosition;
}

public sealed class Player{
	string playerID;
	int systemID;
	int resource;
	bool isAI;
	int pColor;
	int startTechLevel;
	RTSCam playerCam;
	Structures[] buildList;
	UnitClass [] unitList;
}

public sealed class Game{
	float Time;
	float mapID;
	Player[] playerList;
}


public class gameClasses : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
