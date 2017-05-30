using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debrisLogic : MonoBehaviour {

	public GameObject Target;
	public float speed;
	public float damp;
	public float timer = 2;
	

	void Start(){
		Destroy(gameObject, timer);
	}


	void Update () {
		move ();
		turn ();
	}

	void move(){
		transform.position += transform.forward * speed * Time.deltaTime;
	}
	void turn(){
		if (Target != null) {
			Vector3 pos = Target.transform.position - transform.position;
			Quaternion rotation = Quaternion.LookRotation (pos);
			transform.rotation = Quaternion.Slerp (transform.rotation, rotation, damp * Time.deltaTime);
		}
	}
}
