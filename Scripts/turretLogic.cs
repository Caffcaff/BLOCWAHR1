using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretLogic : MonoBehaviour {

	public unitLogic unitmgmt;
	public GameObject target;

	// Use this for initialization
	void Start () {
		if (unitmgmt == null) {
			unitmgmt = GetComponentInParent<unitLogic> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (unitmgmt.target != target) {
			target = unitmgmt.target;
		}
		if (unitmgmt.target != null) {
			turn ();
		}
	}

	void turn() {

	Vector3 looktarget = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
	transform.LookAt(looktarget);
	}
}