using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventManager : MonoBehaviour {

	public delegate void inputDelegate(Vector3 point, GameObject actor);

		public static inputDelegate onNavClick;
		public static inputDelegate onNavArray;
		public static inputDelegate onFollowClick;
		public static inputDelegate onAttackClick;
		public static inputDelegate onGroundAttackClick;

	public delegate void GUIinputDelegate(GameObject button);

	public static GUIinputDelegate onButtonClick;
	public static GUIinputDelegate onButtonEnter;
	public static GUIinputDelegate onButtonExit;

	public delegate void groupSelectionDelegate(Vector2 Rectx, Vector2 Recty, GameObject selectedUnits, int Type);

	public static groupSelectionDelegate onGroupSelect;

	public delegate void typeSelectionDelegate(string type);

	public static typeSelectionDelegate onTypeSelect;

	public delegate void selectionDelegate(GameObject selectedUnit);

	public static selectionDelegate onClearSelect;
	public static selectionDelegate onUnitSelect;
	public static selectionDelegate onSelectEvent;

	public delegate void hitpointDelegate(int amount, GameObject sender, GameObject reciever);

		public static hitpointDelegate onDamage;
		public static hitpointDelegate onRepair;

	public delegate void resourceDelegate(int amount, int type);

		public static resourceDelegate onCollect;
		public static resourceDelegate onSpend;

	public delegate void unitsDelegate(GameObject unit, bool state);

		public static unitsDelegate onUnitCreate;
		public static unitsDelegate onUnitDestroy;

	public delegate void particleEventDelegate(Vector3 sender, Vector3 reciever, int type);

		public static particleEventDelegate onParticleEvent;

	public delegate void buildEventDelegate(Vector3 position, int type, int playerID);


	// Input Event Methods

	public static void NavClick (Vector3 point, GameObject actor) {
		if (onNavClick != null)
			onNavClick (point, actor);
	}
	public static void NavArray (Vector3 point, GameObject actor) {
		if (onNavArray != null)
			onNavArray (point, actor);
	}
	public static void FollowClick (Vector3 point, GameObject actor) {
		if (onFollowClick != null)
			onFollowClick (point, actor);
	}
	public static void AttackClick (Vector3 point, GameObject actor) {
		if (onAttackClick != null)
			onAttackClick (point, actor);
	}
	public static void GroundAttackClick (Vector3 point, GameObject actor) {
		if (onGroundAttackClick != null)
			onGroundAttackClick (point, actor);
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

	// Selection Event Methods

	public static void UnitSelect (GameObject selectedUnit) {
		if (onUnitSelect != null)
			onUnitSelect (selectedUnit);
	}
	public static void SelectEvent (GameObject selectedUnit) {
		if (onSelectEvent != null)
			onSelectEvent (selectedUnit);
	}
	public static void GroupSelect (Vector2 Rectx, Vector2 Recty, GameObject selectedUnits, int Type) {
		if (onGroupSelect != null)
			onGroupSelect (Rectx, Recty, selectedUnits, Type);
	}
	public static void TypeSelect (string type) {
		if (onTypeSelect != null)
			onTypeSelect (type);
	}
	public static void ClearSelect (GameObject selectedUnit) {
		if (onClearSelect != null)
			onClearSelect (selectedUnit);
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
}
