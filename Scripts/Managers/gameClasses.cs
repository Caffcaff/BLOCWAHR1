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
