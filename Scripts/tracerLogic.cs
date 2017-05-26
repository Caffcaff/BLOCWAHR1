using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tracerLogic : MonoBehaviour {

	public float destroyTimer =2;
	public float movementSpeed = 100;
	public bool first = true;

	// Use this for initialization
	void Start () {
		Destroy(gameObject, destroyTimer);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.forward * movementSpeed * Time.deltaTime;
	}
	void OnCollisionEnter(Collision col){

		if (first == true) {
			first = false;
			return;
		}

		if (col.gameObject.layer != 11) {

			Debug.Log ("Tracer hit " + col.gameObject.name);
			eventManager.ParticleEvent (transform.position, transform.position, 2);
			Destroy (gameObject);
		}

	}
	void OnCollisionExit(Collision col){

		Collider coll = GetComponent<SphereCollider>();
		coll.isTrigger = false;
}
}
