using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVlogic : MonoBehaviour {

	public unitLogic uLogic;


	void Start () {
		uLogic = GetComponentInParent<unitLogic> ();	
	}
	
	void onTriggerEnter (Collider other) {

		uLogic.AIencounter (other);

	}
}
