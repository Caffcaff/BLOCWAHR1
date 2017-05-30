using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleManager : MonoBehaviour {

	public GameObject resourceGatherObj;
	public GameObject resourceCompleteObj;
	public GameObject mGunParticles;
	public GameObject mGunHit;
	public GameObject Tracer;
	public GameObject navHalo;
	public GameObject unitExplode;

	public LineRenderer pLineRender;

	void OnEnable(){
		eventManager.onParticleEvent += events;
	}

	void OnDisable(){
		eventManager.onParticleEvent -= events;
	}
	// Use this for initialization
	void Start () {
		GameObject temp = GameObject.Find ("lineRenderer");
		pLineRender = temp.GetComponent<LineRenderer> ();

	}
	// Update is called once per frame
	void events (Vector3 sender, Vector3 reciever, int type)
	{
		if (type == 1) {
			Vector3 pos = reciever - sender;
			Quaternion rotation = Quaternion.LookRotation (pos);
			var bullet = Instantiate (Tracer, sender, rotation);
			bullet.transform.parent = gameObject.transform;

		}
		if (type == 2) {
			var bHit = Instantiate (mGunHit, sender, transform.rotation);
			bHit.transform.parent = gameObject.transform;
		}
		if (type == 3) {
			Instantiate (navHalo, sender, transform.rotation);
		}
		if (type == 4) {
		//	resourceComplete ();
		}
		if (type == 5) {
			Instantiate (unitExplode, sender, transform.rotation);
		}
		if (type == 6) {
		//	unitSmoke ();
		}
		if (type == 7) {
		//	unitRepair ();
		}
		if (type == 8) {
		//	missileCollision ();
		}
		if (type == 9) {
		//	missileWaterCollision ();
		}
		if (type == 10) {
		//	waterCollision ();
		}
		if (type == 11) {
		//	unitRecycled ();
		}
		if (type == 12) {
		//	unitCreated ();
		}
		if (type == 13) {
		//	buildingCreated ();
		}
		if (type == 14) {
		//	buildingRecycled ();
		}
		if (type == 15) {
		//	buildingDestroyed ();
		}
		if (type == 16) {
		//	mineExplosion ();
		}
		if (type == 17) {
		//	fire ();
		}
		if (type == 18) {
			patrolLine(sender,reciever);
		//	gunFire ();
		}
		if (type == 19) {
			pLineClear();
		}
		if (type == 20) {
			Debug.Log ("Particle Event 20");
		//	gunFire ();
		}
	}

	void patrolLine(Vector3 pointA, Vector3 PointB){
		pLineRender.positionCount = 2;
		pLineRender.SetPosition (0, pointA);
		pLineRender.SetPosition (1, PointB);
	}
	void pLineClear(){
		pLineRender.positionCount = 0;
	}
}
