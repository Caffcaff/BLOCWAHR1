using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class navController : MonoBehaviour {

	public GameObject navHalo;
	public float spread =5;
	public float minScatter = 3f;
	public selectionManager sManager;
	public List<Vector3> currArray = new List<Vector3>();
	public float delta;

	// Formation Controls

	public int columns = 5;
	public float leadDistance;
	private float offset;
	public float hSpacing = 15;
	public float vSpacing = 15;
	public GameObject fSlot;

	public int sectors = 5;

	private float ang;
	private float angStep;
	public float ringSpacing = 12;
	public float ringOffset = 12;
	public bool halfMoon;

	public List<GameObject> slotMarkers = new List<GameObject>();


	void OnEnable(){
		
		eventManager.onProcessOrder += orderSwitch;
		eventManager.onRequestPosition += newPosition;
		eventManager.onRequestNav += navPosition;
	}

	void OnDisable(){
		

		eventManager.onProcessOrder -= orderSwitch;
		eventManager.onRequestPosition -= newPosition;
		eventManager.onRequestNav -= navPosition;
	}

	// Use this for initialization
	void Start () {
		if (sManager == null) {
			GameObject SMObj = GameObject.FindGameObjectWithTag ("SelectionManager");
			sManager = SMObj.GetComponent<selectionManager>();
		}
	}
	
	// Update is called once per frame
	void navArray (Order order, GameObject[] units) {

		Order thisOrder = order;

		currArray.Clear();

		Vector3 hit = thisOrder.navTarget;

		int i = 0;
		int r = 0;
		bool oddColumns = false;

//		var heading = sManager.currentSelection[0].transform.position - hit;
//		var distance = heading.magnitude;
//		Vector3 direction = heading / distance;
//		direction.x = 0;
//		direction.z = 0;


		foreach (GameObject unit in units) {
			if (i > 0 && r < columns) {
				eventManager.ParticleEvent (hit, hit, 3);
				thisOrder.navTarget = hit;
				eventManager.ServeOrder (thisOrder, unit, 0, order.playerID);
				Vector3 shift = new Vector3 (hSpacing, 0, 0);
				hit += shift;
				r++;
			}
			if (i > 0 && r == columns) {
				Vector3 shift = new Vector3 (-(hSpacing*(float)columns), 0, -vSpacing);
				hit += shift;
				r = 0;
			}

			if (i == 0) {
				eventManager.ParticleEvent (hit, hit, 3);
				thisOrder.navTarget = hit;
				eventManager.ServeOrder (thisOrder, unit, 0, order.playerID);

				if ((columns % 2) > 0) {
					offset = (((float)columns - 1) / 2);
					oddColumns = true;
				} else {
					offset = ((float)columns / 2);
					oddColumns = false;
				}
				if (oddColumns == true) {
					Vector3 shift = new Vector3 (-(hSpacing * offset), 0, -(vSpacing + leadDistance));
					hit += shift;
					i++;
				}
				if (oddColumns == false) {
					Vector3 shift = new Vector3 (((-(hSpacing * offset)) + (hSpacing / 2)), 0, -(vSpacing + leadDistance));
					hit += shift;
					i++;
				}
			}
		}
	}
	void surroundObject(Order order, GameObject[] units){

		Order thisOrder = order;

		int r = 0;
		int i = 0;
		int surround = sectors;

		ang = (360/(float)surround);
		angStep = ang;

		GameObject target = order.unitTarget;

		Collider tempCol = target.GetComponent<Collider> ();
		Bounds footPrint = tempCol.bounds;
		Vector3 shift = new Vector3 (ringOffset, 0, ringOffset);
		footPrint.Expand (shift);

		Vector3 center = target.transform.position;

		foreach (GameObject unit in units){
			r++;
			Vector3 pos = RandomCircle(center, footPrint.extents.x);
//			Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center-pos);
			Vector3 hit = footPrint.ClosestPoint (pos);

			thisOrder.navTarget = hit;

			eventManager.ServeOrder (thisOrder, unit, 0, order.playerID);

//			if (i == 0) {
//				eventManager.AttackArray (hit, unit, true);
//				i++;
//			}
//			if (i != 0) {
//				eventManager.AttackArray (hit, unit, false);
//			}

			eventManager.ParticleEvent (hit, hit, 3);

			if (ang >= 180 && halfMoon == true) {
				shift = new Vector3(ringSpacing, 0, ringSpacing);
				footPrint.Expand (shift);
				ang = (0+(angStep / 2));
				r = 0;
				surround++;
				angStep = (360 / surround);
			}

			if (r == surround) {
				shift = new Vector3(ringSpacing, 0, ringSpacing);
				footPrint.Expand (shift);
				ang += (angStep / 2);
				r = 0;
				surround++;
				angStep = (360 / surround);
			}

		}
	}

	Vector3 RandomCircle (Vector3 center,float radius){
		//	float ang = (360/(float)sectors);
		Vector3 pos;
		pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
		//			pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
		pos.y = center.y;
		pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
		ang += angStep;
		return pos;
	}

	// Sets the follow formation of leader Unit

	void stackFormation(Vector3 hit, GameObject leader, bool unused){

		foreach (GameObject unit in sManager.currentSelection) {
				eventManager.FlockArray (unit, leader);
		}	
	}
	void newPosition(GameObject actor, GameObject target, float range, int ID){
		int i = 0;
		pointGen:
		Vector3 tempRand = new Vector3 (Random.Range (-range,range), 0, Random.Range (-range,range));
		i++;
		Vector3 tryPos = target.transform.position + tempRand;
		Vector3 rayPos = new Vector3 (tryPos.x, 1000, tryPos.z);
		var layerMask = ~(1 << 11);
		RaycastHit hit;
		if (Physics.Raycast (rayPos, -Vector3.up, out hit, 1500, layerMask)) {
			Instantiate (navHalo, hit.point, transform.rotation);
			if (hit.collider.tag == "Floor" | hit.collider.tag == "buildPlane") {
				eventManager.ServePosition(actor, tryPos);
			} 
			else {
				if (i < 12) {
					Debug.Log ("Retrying Point Gen");
					goto pointGen;
				} 
				else {
					Debug.Log ("No Point Available");
					eventManager.ServePosition(actor, target.transform.position);
				}
			}

	}
}
	void navPosition(GameObject actor, Vector3 target, float range, int ID){
		int i = 0;
		pointGen:
		Vector3 tempRand = new Vector3 (Random.Range (-range,range), 0, Random.Range (-range,range));
		i++;
		Vector3 tryPos = target + tempRand;
		Vector3 rayPos = new Vector3 (tryPos.x, 1000, tryPos.z);
		var layerMask = ~(1 << 11);
		RaycastHit hit;
		if (Physics.Raycast (rayPos, -Vector3.up, out hit, 1500, layerMask)) {
			Instantiate (navHalo, hit.point, transform.rotation);
			if (hit.collider.tag == "Floor" | hit.collider.tag == "buildPlane") {
				eventManager.ServePosition(actor, tryPos);
			} 
			else {
				if (i < 12) {
					Debug.Log ("Retrying Nav Point Gen");
					goto pointGen;
				} 
				else {
					Debug.Log ("No Nav Point Available");
					eventManager.ServePosition(actor, target);
				}
			}

		}
	}

	void orderSwitch(Order order, GameObject [] units){

		if (order._type == Order.Type.Attack || order._type == Order.Type.Mine || order._type == Order.Type.Build || order._type == Order.Type.Repair) {

			surroundObject (order, units);

		}

		if (order._type == Order.Type.Move || order._type == Order.Type.Recon) {

			navArray (order, units);

		}
	}
}