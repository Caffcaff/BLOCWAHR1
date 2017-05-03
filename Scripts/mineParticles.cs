using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mineParticles : MonoBehaviour {

	public ParticleSystem particles;
	private unitLogic uLogic;
	public bool active;
	public float gravity = 5;
	public float startUp = 1.5f;
	private float ticker;


	// Use this for initialization
	void Start () {
		uLogic = GetComponentInParent<unitLogic> ();
		particles = GetComponent<ParticleSystem> ();
		ticker = startUp;
	}
	
	// Update is called once per frame
	void Update () {
		if (uLogic._state == unitLogic.State.Mine || uLogic._state == unitLogic.State.Build) {
			ticker -= Time.deltaTime;
			var em = particles.emission;
			active = true;
			em.enabled = true;
			if (ticker <= 0) {
				var pm = particles.main;
				pm.gravityModifier = 0;
			}
			else{
				var pm = particles.main;
				pm.gravityModifier = gravity;	
			}

		} else {
			var pm = particles.main;
			pm.gravityModifier = gravity;
			var em = particles.emission;
			em.enabled = false;
			active = false;
			ticker = startUp;
		}
	}
}
