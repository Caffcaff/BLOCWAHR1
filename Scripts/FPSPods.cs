using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSPods : MonoBehaviour {

	public TextMesh FPS;
	public TextMesh Pods;
	public GameObject Pooper;
	public GameObject FPSp;
	public GameObject Podsp;
	public ResourceSpawner poopcomponent;
	public int podCount;
	public float frameCount;
	public GameObject boom;

	// Use this for initialization
	void Start () {
		poopcomponent = Pooper.GetComponent<ResourceSpawner> ();
		FPS = FPSp.GetComponent<TextMesh> ();
		Pods = Podsp.GetComponent<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hit; 
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); 
			if (Physics.Raycast (ray, out hit, 1000.0f)) {
				Instantiate (boom, hit.point, Random.rotation);
			}
		}

		frameCount = 1 / Time.deltaTime;
		podCount = poopcomponent.Spawncount;
		FPS.text = frameCount.ToString();;
		Pods.text = podCount.ToString();;
	}
}