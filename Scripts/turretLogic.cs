using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretLogic : MonoBehaviour {

	public unitManager unitmgmt;
	public GameObject target;

	// Use this for initialization
	void Start () {
		if (unitmgmt == null) {
			unitmgmt = GetComponentInParent<unitManager> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (unitmgmt.attackTarget != target) {
			target = unitmgmt.attackTarget;
		}
		if (unitmgmt.attackTarget != null) {
			turn ();
		}
	}
	void turn() {

	Vector3 looktarget = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
		transform.LookAt(looktarget);

	}
}