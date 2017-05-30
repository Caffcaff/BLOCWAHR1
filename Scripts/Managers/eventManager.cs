using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventManager : MonoBehaviour {

	public delegate void inputDelegate(Vector3 point, GameObject actor, int playerID);

		public static inputDelegate onNavClick;
		public static inputDelegate onFollowClick;
		public static inputDelegate onAttackClick;
		public static inputDelegate onGroundAttackClick;

	public delegate void navArrayDelegate(Vector3 point, GameObject actor, bool leader, int ID);

		public static navArrayDelegate onNavArray;
		public static navArrayDelegate onAttackArray;
		public static navArrayDelegate onSetFormation;

	public delegate void flockDelegate(GameObject actor, GameObject slot);

	public static flockDelegate onFlockArray;

	public delegate void GUIinputDelegate(GameObject button);

		public static GUIinputDelegate onButtonClick;
		public static GUIinputDelegate onButtonEnter;
		public static GUIinputDelegate onButtonExit;

	public delegate void groupSelectionDelegate(Vector2 Rectx, Vector2 Recty, GameObject selectedUnits, int Type);

		public static groupSelectionDelegate onGroupSelect;

	public delegate void typeSelectionDelegate(string type);

		public static typeSelectionDelegate onTypeSelect;

	public delegate void selectionDelegate(GameObject selectedUnit, int ID);

		public static selectionDelegate onClearSelect;
		public static selectionDelegate onUnitSelect;
		public static selectionDelegate onStructureSelect;
		public static selectionDelegate onSelectEvent;
		public static selectionDelegate onFactorySelect;


	public delegate void hitpointDelegate(int amount, GameObject sender, GameObject reciever);

		public static hitpointDelegate onDamage;
		public static hitpointDelegate onRepair;
		public static hitpointDelegate onConstruction;

	public delegate void resourceDelegate(int amount, int type);

		public static resourceDelegate onCollect;
		public static resourceDelegate onSpend;

	public delegate void unitsDelegate(GameObject unit, bool state);

		public static unitsDelegate onUnitCreate;
		public static unitsDelegate onUnitDestroy;

	public delegate void particleEventDelegate(Vector3 sender, Vector3 reciever, int type);

		public static particleEventDelegate onParticleEvent;

	public delegate void factoryEventDelegate(GameObject factory, buildType type);

		public static factoryEventDelegate onFactoryOrder;
		public static factoryEventDelegate onFactoryClear;
		public static factoryEventDelegate onFactoryConfirm;


	public delegate void buildEventDelegate(Vector3 position, int type);

		public static buildEventDelegate onBuildSelect;
		public static buildEventDelegate onBuildRequest;
		public static buildEventDelegate onBuildConfirm;
		public static buildEventDelegate onBuildInit;
		public static buildEventDelegate onBuildCancel;

	public delegate void rawInputDelegate(Vector3 position);

		public static rawInputDelegate onLeftClick;
		public static rawInputDelegate onRightClick;
		public static rawInputDelegate onMiddleClick;
		public static rawInputDelegate onEscapeKey;

	public delegate void spawnEventDelegate(GameObject prefab, Vector3 position, int playerID);

		public static spawnEventDelegate onUnitSpawn;

	public delegate void patrolEventDelegate(Vector3 origin, Vector3 destination, int ID);

		public static patrolEventDelegate onPatrolSet;

	public delegate void vantageDelegate(GameObject actor, GameObject target, float range, int ID);

		public static vantageDelegate onRequestPosition;

	public delegate void navRequestDelegate(GameObject actor, Vector3 target, float range, int ID);

	public static navRequestDelegate onRequestNav;

	public delegate void repositionDelegate(GameObject actor, Vector3 position);
		
		public static repositionDelegate onServePosition; 

	public delegate void wallDelegate(GameObject wall);

	public static wallDelegate onConfirmWall; 

	public delegate void patrolDelegate(Vector3 pointA, Vector3 pointB, int ID);

	public static patrolDelegate onPatrolEnter; 
	public static patrolDelegate onServePatrol;

	public delegate void cacheEventDelegate (int ID);

	public static cacheEventDelegate onMapAI;
	public static cacheEventDelegate onMarketCache;
	public static cacheEventDelegate onListInit;
	public static cacheEventDelegate onUnitCache;

	public delegate void orderEventDelegate (Order order, GameObject unit, int statusCode, int ID);

	public static orderEventDelegate onInitOrder;
	public static orderEventDelegate onServeOrder;
	public static orderEventDelegate onReturnOrder;

	public delegate void orderProcessDelegate (Order order, GameObject[] units);

	public static orderProcessDelegate onProcessOrder;

	public delegate void AiFactoryDelegate (FactoryOrder AIFactory);

	public static AiFactoryDelegate onAIFactoryOrder;
	public static AiFactoryDelegate onServeFactoryOrder;

	public delegate void encounterDelegate (navMemory scan, int playerID);

	public static encounterDelegate onUnitEncounter;
	public static encounterDelegate onStructEncounter;


	// Input Event Methods

	public static void NavClick (Vector3 point, GameObject actor, int ID) {
		if (onNavClick != null)
			onNavClick (point, actor, ID);
	}
	public static void NavArray (Vector3 point, GameObject actor, bool leader, int ID) {
		if (onNavArray != null)
			onNavArray (point, actor, leader, ID);
	}
	public static void SetFormation (Vector3 point, GameObject actor, bool leader, int ID) {
		if (onSetFormation != null)
			onSetFormation (point, actor, leader, ID);
	}
	public static void AttackArray (Vector3 point, GameObject actor, bool leader, int ID) {
		if (onAttackArray != null)
			onAttackArray (point, actor, leader, ID);
	}
	public static void FlockArray (GameObject actor, GameObject slot) {
		if (onFlockArray != null)
			onFlockArray (actor, slot);
	}
	public static void FollowClick (Vector3 point, GameObject actor, int ID) {
		if (onFollowClick != null)
			onFollowClick (point, actor, ID);
	}
	public static void AttackClick (Vector3 point, GameObject actor, int ID) {
		if (onAttackClick != null)
			onAttackClick (point, actor, ID);
	}
	public static void GroundAttackClick (Vector3 point, GameObject actor, int ID) {
		if (onGroundAttackClick != null)
			onGroundAttackClick (point, actor, ID);
	}
	public static void ButtonClick (GameObject button) {
		if (onButtonClick != null)
			onButtonClick (button);
	}
	public static void ButtonEnter (GameObject button) {
		if (onButtonEnter != null)
			onButtonEnter (button);
	}
	public static void ButtonExit (GameObject button) {
		if (onButtonExit != null)
			onButtonExit (button);
	}

	// Selection Event Methods2

	public static void UnitSelect (GameObject selectedUnit, int ID) {
		if (onUnitSelect != null)
			onUnitSelect (selectedUnit, ID);
	}
	public static void StructureSelect (GameObject selectedUnit, int ID) {
		if (onStructureSelect != null)
			onStructureSelect (selectedUnit, ID);
	}
	public static void FactorySelect (GameObject selectedUnit, int ID) {
		if (onFactorySelect != null)
			onFactorySelect (selectedUnit, ID);
	}
	public static void SelectEvent (GameObject selectedUnit, int ID) {
		if (onSelectEvent != null)
			onSelectEvent (selectedUnit, ID);
	}
	public static void GroupSelect (Vector2 Rectx, Vector2 Recty, GameObject selectedUnits, int Type) {
		if (onGroupSelect != null)
			onGroupSelect (Rectx, Recty, selectedUnits, Type);
	}
	public static void TypeSelect (string type) {
		if (onTypeSelect != null)
			onTypeSelect (type);
	}
	public static void ClearSelect (GameObject selectedUnit, int ID) {
		if (onClearSelect != null)
			onClearSelect (selectedUnit, ID);
	}

	// HitPoint Event Methods

	public static void Damage (int amount, GameObject sender, GameObject reciever) {
		if (onDamage != null)
			onDamage (amount, sender, reciever);
	}
	public static void Repair (int amount, GameObject sender, GameObject reciever) {
		if (onRepair != null)
			onRepair (amount, sender, reciever);
	}
	public static void Construction (int amount, GameObject sender, GameObject reciever) {
		if (onConstruction != null)
			onConstruction (amount, sender, reciever);
	}

	// Resource Event Methods

	public static void Collect (int amount, int type) {
		if (onCollect != null)
			onCollect (amount, type);
	}
	public static void Spend (int amount, int type) {
		if (onSpend != null)
			onSpend (amount, type);
	}

	//Unit Event Methods
	public static void UnitCreate (GameObject unit, bool state) {
		if (onUnitCreate != null)
			onUnitCreate (unit, state);
	}
	public static void UnitDestroy (GameObject unit, bool state) {
		if (onUnitDestroy != null)
			onUnitDestroy (unit, state);
	}
	public static void ParticleEvent (Vector3 sender, Vector3 reciever, int type) {
		if (onParticleEvent != null)
			onParticleEvent (sender, reciever, type);
	}
	//Build Events
	public static void BuildSelect (Vector3 position, int type) {
		if (onBuildSelect != null)
			onBuildSelect (position, type);
	}
	public static void BuildRequest (Vector3 position, int type) {
		if (onBuildRequest != null)
			onBuildRequest (position, type);
	}
	public static void BuildConfirm (Vector3 position, int type) {
		if (onBuildConfirm != null)
			onBuildConfirm (position, type);
	}
	public static void BuildInit (Vector3 position, int type) {
		if (onBuildInit != null)
			onBuildInit (position, type);
	}
	public static void BuildCancel (Vector3 position, int type) {
		if (onBuildCancel != null)
			onBuildCancel (position, type);
	}

	// Raw Input Events
	public static void LeftClick (Vector3 position) {
		if (onLeftClick != null)
			onLeftClick (position);
	}
	public static void RightClick (Vector3 position) {
		if (onRightClick != null)
			onRightClick (position);
	}
	public static void MiddleClick (Vector3 position) {
		if (onMiddleClick != null)
			onMiddleClick (position);
	}
	public static void EscapeKey (Vector3 position) {
		if (onEscapeKey != null)
			onEscapeKey (position);
	}
	// Factory Events
	public static void FactoryOrder (GameObject factory, buildType type) {
		if (onFactoryOrder != null)
			onFactoryOrder (factory, type);
	}
	public static void FactoryClear (GameObject factory, buildType type) {
		if (onFactoryClear != null)
			onFactoryClear (factory, type);
	}
	public static void FactoryConfirm (GameObject factory, buildType type) {
		if (onFactoryConfirm != null)
			onFactoryConfirm (factory, type);
	}
	public static void UnitSpawn (GameObject prefab, Vector3 location, int playerID) {
		if (onUnitSpawn != null)
			onUnitSpawn (prefab, location, playerID);
	}
	// Patrol Events
	public static void PatrolSet (Vector3 origin, Vector3 destination, int ID) {
		if (onPatrolSet != null)
			onPatrolSet (origin, destination, ID);
	}

	// Position Reqeust Events
	public static void RequestPosition (GameObject actor, GameObject target, float range, int ID) {
		if (onRequestPosition != null)
			onRequestPosition (actor, target, range, ID);
	}
	public static void RequestNav (GameObject actor, Vector3 target, float range, int ID) {
		if (onRequestNav != null)
			onRequestNav (actor, target, range, ID);
	}
	public static void ServePosition (GameObject actor, Vector3 position) {
		if (onServePosition != null)
			onServePosition (actor, position);
	}
	public static void ConfirmWall (GameObject wall) {
		if (onConfirmWall != null)
			onConfirmWall (wall);
	}
	public static void PatrolEnter (Vector3 pointA, Vector3 pointB, int ID) {
		if (onPatrolEnter != null)
			onPatrolEnter (pointA, pointB, ID);
	}
	public static void ServePatrol (Vector3 pointA, Vector3 pointB, int ID) {
		if (onServePatrol != null)
			onServePatrol (pointA, pointB, ID);
	}
	public static void MapAI (int ID) {
		if (onMapAI != null)
			onMapAI (ID);
	}
	public static void MarketCache (int ID) {
		if (onMarketCache != null)
			onMarketCache (ID);
	}
	public static void ListInit (int ID) {
		if (onListInit != null)
			onListInit (ID);
	}
	public static void UnitCache(int ID) {
		if (onUnitCache != null)
			onUnitCache (ID);
	}
	public static void InitOrder (Order order, GameObject unit, int statusCode, int ID) {
		if (onInitOrder != null)
			onInitOrder (order, unit, statusCode, ID);
	}
	public static void ServeOrder (Order order, GameObject unit, int statusCode, int ID) {
		if (onServeOrder != null)
			onServeOrder (order, unit, statusCode, ID);
	}
	public static void ReturnOrder (Order order, GameObject unit, int statusCode, int ID) {
		if (onReturnOrder != null)
			onReturnOrder (order, unit, statusCode, ID);
	}

	public static void ProcessOrder (Order order, GameObject[] units) {
		if (onProcessOrder != null)
			onProcessOrder (order, units);
	}

	// AI Factory

	public static void AIFactoryOrder (FactoryOrder AIFactory) {
		if (onAIFactoryOrder != null)
			onAIFactoryOrder (AIFactory);
	}

	public static void ServeFactoryOrder (FactoryOrder AIFactory) {
		if (onServeFactoryOrder != null)
			onServeFactoryOrder (AIFactory);
	}

	public static void UnitEncounter (navMemory scan, int playerID) {
		if (onUnitEncounter != null)
			onUnitEncounter (scan, playerID);
	}

	public static void StructEncounter (navMemory scan, int playerID) {
		if (onStructEncounter != null)
			onStructEncounter (scan, playerID);
	}
}