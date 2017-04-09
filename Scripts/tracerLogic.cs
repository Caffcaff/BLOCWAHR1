using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tracerLogic : MonoBehaviour {

	public float destroyTimer =2;
	public float movementSpeed = 100;

	// Use this for initialization
	void Start () {
		Destroy(gameObject, destroyTimer);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.forward * movementSpeed * Time.deltaTime;
	}
	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag != "Friendly") {
			ContactPoint contact = col.contacts[0];
			Vector3 pos = contact.point;
			eventManager.ParticleEvent (pos, col.transform.position, 2);
			Destroy (gameObject);
		}
	}
}
