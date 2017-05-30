//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class FOVlogic : MonoBehaviour {
//
//	public unitAgent uLogic;
//	public bool debugActive = false;
//
//
//	void Start () {
//		GameObject temp = transform.parent.gameObject;
//		uLogic = temp.GetComponent<unitAgent> ();	
//	}
//	
//	void OnTriggerEnter (Collider other) {
//
//		if (debugActive) {
//			Debug.Log ("Collision: " + other.gameObject.name);
//		}
//
//		if(other.gameObject.tag != "Friendly" && other.gameObject.tag != "Structure" && other.gameObject.tag != "tracer" && other.gameObject.tag != "missile"){
//			return;
//		} else {
//			uLogic.AIencounter (other);
//		}
//
//	}
//}
