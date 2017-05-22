using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMap : MonoBehaviour {

	[Header("General")]
	public int playerID = 2;
	public GameObject AIseed;
	public float mapX = 10000;
	public float mapZ = 10000;
	public float maxHeight = 1000;
	public float minHeight = -20;
	public float minDistance = 600;
	public Vector3 startPos = new Vector3 (0, 0, 0);

	[Header("Spacing")]
	public string Floor = ("Floor");
	public string buildPlane = ("buildPlane");


	[Header("Spacing")]
	public float blockSize = 200;
	public float pointScatter = 50;
	public float buildScatter = 5;
	public float focalOffset = 200;
	public float buildOffset = 100;
	public float buildSlots = 8;
	public int buildSectors = 6;
	private float ang;
	private float angStep;
	public bool halfMoon;

	[Header("Culling")]
	bool isCull = true;
	float cullRatio = 3;
	public float slope = 20;

	[Header("Point Plotting")]
	public Vector3 scanPosition;
	public List<Vector3> scanPoints = new List<Vector3>();

	[Header("Debug Options")]
	public bool debugActive;

	public GameObject pPuck;
	public GameObject fPuck;
	public GameObject sPuck;
	public GameObject tPuck;
	public GameObject wPuck;

	public List<baseLocale> locales = new List<baseLocale>();
	public List<baseAI> outposts = new List<baseAI>();

	private AICommand aiCommand;

	public enum State {
		Initialise,
		Setup,
		Idle,
		Scan,
		Strata,
		Cull,
		Submit
	}

	public State _state;

	// Use this for initialization
	IEnumerator Start(){
		_state = State.Initialise;

		while (true) {

			switch (_state) {
			case State.Initialise:
				initMe ();
				break;
			case State.Setup:
				inSetup ();
				break;
			case State.Idle:
				idle ();
				break;
			case State.Scan:
				inScan ();
				break;
			case State.Strata:	
				inStrata ();
				break;
			case State.Cull:
				inCull ();
				break;
			case State.Submit:
				inSubmit ();
				break;
			}
			yield return 0;
		}
	}
	void initMe () {
		_state = State.Setup;

	}
	void inSetup () {
		aiCommand = GetComponent<AICommand> ();
		playerID = aiCommand.playerID;
		AIseed = this.gameObject;
		_state = State.Scan;
	}
	void idle () {

	}
	public void onButton () {
		scanPosition = new Vector3 (0, 0, 0);
		_state = State.Scan;
	}
	void inScan () {

		if (scanPosition.z >= (startPos.z+mapZ)) {
			_state = State.Strata;
		}

		if (checkPoint(scanPosition, 1) == true) {
			scanPoints.Add (scanPosition);
			if (debugActive)
			Instantiate (pPuck, scanPosition, transform.rotation);
		}
			
		if (scanPosition.x < mapX) {
			scanPosition.x += (blockSize+(Random.Range(-pointScatter,pointScatter)));
		} 
		else {
			scanPosition.x = startPos.x;
			scanPosition.z += (blockSize+(Random.Range(-pointScatter,pointScatter)));
		}
	}
	void inStrata () {

		foreach(Vector3 point in scanPoints){
			baseLocale thisLocale = new baseLocale (point);

			int r = 0;
			int i = 0;
			int surround = buildSectors;

			ang = (360/(float)surround);
			angStep = ang;

			Vector3 center = point;
			float shift = focalOffset;

			while (buildSlots > i){
				r++;
				Vector3 pos = RandomCircle(center, shift);
				Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center-pos);

				if (ang >= 180 && halfMoon == true) {
					shift += buildOffset;
					ang = (0+(angStep / 2));
					r = 0;
					surround++;
					angStep = (360 / surround);
				}

				if (r == surround) {
					shift += buildOffset;
					ang += (angStep / 2);
					r = 0;
					surround++;
					angStep = (360 / surround);
				}

				if (i == buildSlots - 4){
					shift += buildOffset;
					ang += (angStep / 2);
					r = 0;
					surround++;
					angStep = (360 / 4);
				}


				if (checkPoint (pos, 2) == true) {

					//Silos
					Vector3 bPos = new Vector3 ((pos.x + Random.Range (-buildScatter, buildScatter)), Terrain.activeTerrain.SampleHeight (pos), (pos.z + Random.Range (-buildScatter, buildScatter)));
					if (i > (buildSlots - 8) && i < (buildSlots - 4)) {
						thisLocale.silo.Add (bPos);
						if (debugActive)
						Instantiate (fPuck, bPos, rot);
					}
					//Buildings
					if (i <= (buildSlots - 8)) {
						thisLocale.slots.Add (bPos);
						if (debugActive)
						Instantiate (sPuck, bPos, rot);
					}
					//Towers
					if (i >= (buildSlots - 4) && i < buildSlots) {
						thisLocale.towers.Add (bPos);
						if (debugActive)
						Instantiate (tPuck, bPos, rot);
					}
				}
				 else {
					Debug.Log ("nope");
				}
				i++;
			}
			locales.Add (thisLocale);
		}
		_state = State.Cull;
	}
		
	void inCull () {

		int i = locales.Count-1;

		while (i >= 0) {

			bool keeper = true;
			string shout = " null";

			if (locales[i].silo.Count != 3) {
				keeper = false;
				shout = " silo";
			}
			if (locales[i].towers.Count != 4) {
				keeper = false;
				shout = " towers";
			}
			if (locales[i].slots.Count != 8) {
				keeper = false;
				shout = " buildSlots";
			}

			int s = 0;
			while (s < locales[i].silo.Count) {
				if (Mathf.Abs (locales[i].silo[s].y - locales[i].focal.y) > slope) {
					keeper = false;
					shout = " slope";}
				s++;
			}

			int t = 0;
			while (t < locales[i].towers.Count) {
				if (Mathf.Abs (locales[i].towers[t].y - locales[i].focal.y) > slope) {
					keeper = false;
					shout = " slope";}
				t++;
			}

			if (keeper == false) {
				if (debugActive) {
					Instantiate (pPuck, locales [i].focal, transform.rotation);
				}
				locales.RemoveAt(i);
				Debug.Log ("Culled" + i + shout);
			}
			i--;
		}

		List<baseLocale> reOrder = new List <baseLocale> ();
		float closest = Mathf.Infinity;
		Vector3 seed = AIseed.transform.position;

		locales.Sort(delegate(baseLocale c1, baseLocale c2){
			return Vector3.Distance(seed, c1.focal).CompareTo
				((Vector3.Distance(seed, c2.focal)));   
		});
			
		_state = State.Submit;
	}
	void inSubmit () {

		int i = 0;

		foreach (baseLocale locale in locales) {

			List<buildSlot> tempSlots = new List<buildSlot> ();

			foreach (Vector3 slot in locale.slots) {
				buildSlot temp = new buildSlot (slot);
				tempSlots.Add (temp);
			}

			foreach (Vector3 slot in locale.silo) {
				buildSlot temp = new buildSlot (slot);
				tempSlots.Add (temp);
			}

			foreach (Vector3 slot in locale.towers) {
				buildSlot temp = new buildSlot (slot);
				tempSlots.Add (temp);
			}

			baseAI outPost = new baseAI (locale.focal, tempSlots, i);
			outposts.Add (outPost);
			i++;
		}

		int d = 0;
		while (d < locales.Count) {
			//Debug.Log (Vector3.Distance (outposts [d].focal, AIseed.transform.position) + "outpost " + outposts[d].ID);
			d++;
		}

		eventManager.MapAI (playerID);
		_state = State.Idle;

	}
	bool checkPoint(Vector3 point, int type){

		Vector3 rayPos = new Vector3 (point.x, 2000, point.z);
		RaycastHit hit;
		if (Physics.Raycast (rayPos, -Vector3.up, out hit, 2500)) {
			if (hit.collider.tag == Floor || hit.collider.tag == buildPlane) {
				if (hit.point.y < maxHeight && hit.point.y > minHeight) {
					scanPosition.y = hit.point.y;
					return true;
				} else {
					return false;
				}
			}
			else{
			return false;
			}
}
		else{
			if (type == 2) {
				Debug.Log ("no ray hit");
			}
			return false;

		}
	}

	Vector3 RandomCircle (Vector3 center,float radius){
		//	float ang = (360/(float)sectors);
		Vector3 pos;
		pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
		//pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
		pos.y = center.y;
		pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
		ang += angStep;
		return pos;
	}
}