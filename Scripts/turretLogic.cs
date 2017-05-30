using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretLogic : MonoBehaviour {

	public unitAgent unitmgmt;
	public GameObject target;
	public Vector3 lookSpot;
	bool looking = true;

	// Use this for initialization
	void Start () {
		if (unitmgmt == null) {
			unitmgmt = GetComponentInParent<unitAgent> ();
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (unitmgmt.target != target) {
			target = unitmgmt.target;
		}
		if (unitmgmt.target != null) {
			looking = true;
			turn ();
		} else {

			if (looking == true) {
				lookSpot = transform.forward * 50;
				look ();
				looking = false;

			}
		}
	}

	void turn() {

	Vector3 looktarget = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
	transform.LookAt(looktarget);

	}

	void look(){

		Vector3 looktarget = new Vector3(lookSpot.x, transform.position.y, lookSpot.z);
		transform.LookAt(looktarget);

	}


}