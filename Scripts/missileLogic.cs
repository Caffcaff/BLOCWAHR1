using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileLogic : MonoBehaviour {

	public int damage =2;
	public float radius;
	public GameObject target;
	public GameObject explosion;
	public float safetyTimer = 2;
	public float speed =60;
	public float speed2 =60;
	public Collider cl;
	public float turnRadius = 0.5f;
	public float delay =1;
	public float destroyTimer =5;

	void Start ()
	{
		cl = GetComponent<Collider> ();
		unitManager parent = GetComponentInParent<unitManager> ();
		target = parent.attackTarget;
		Destroy(gameObject, destroyTimer);
	}

	void Update ()
	{
		if (delay > 0)
		{
			move ();
		}
		if (delay < 0)
		{
			move ();
			turn ();
		}

		safetyTimer = safetyTimer - (1 * Time.deltaTime);
		if (safetyTimer < 0)
		{
			cl.isTrigger = false;
		}
		delay -= Time.deltaTime;
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.other.gameObject.GetComponent<Collider>().tag != "Friendly" && safetyTimer < 0) {
		eventManager.Damage (damage, transform.parent.gameObject, col.gameObject);
			Instantiate (explosion, transform.position, transform.rotation);
			Destroy (gameObject);
		}
	}
	void turn(){
	Vector3 pos = target.transform.position - transform.position;
	Quaternion rotation = Quaternion.LookRotation(pos);
	transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnRadius * Time.deltaTime);
	}
	void move () {
		transform.position += transform.forward * speed2 * Time.deltaTime;
	}
}
