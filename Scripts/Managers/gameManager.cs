using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class UnitClass{
	int unitID;
	int Type;
	int hitPoints;
	Vector3 unitLocation;
	int State;
	int targetID;
	Vector3 navTarget;
}

public sealed class ResourceClass{
	int unitID;
	int Type;
	int hitPoints;
	Vector3 unitLocation;
}

public sealed class buildQueue{
	int bqID;
	int Type;
	int Progress;
}

public sealed class Structures{
	int buildingID;
	int ownerID;
	int Type;
	Vector3 BuildLocation;
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
	Player[] playerList;
}

public class gameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
