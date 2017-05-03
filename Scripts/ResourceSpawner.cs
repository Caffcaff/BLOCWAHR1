using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour {

	public GameObject Resource1;
	public GameObject Resource2;
	private GameObject toSpawn;
	public float bias;
	public float rate = 20;
	public float scatter = 5;
	public bool Active = true;
	private bool canDrop =false;
	private float ticker =60;
	private int distributer;
	private Vector3 pee;
	public int Spawncount = 0;

	// Use this for initialization
	void Start () {
		toSpawn = Resource1;
		pee = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Active == true) {
			ticker = ticker -(Time.deltaTime * rate);
			if (ticker <= 0) {
				canDrop = true;
			} else
				canDrop = false;
			if (canDrop) {
				float rand = Random.Range (0, 10);
				if (rand > bias) {
					toSpawn = Resource1;
					Spawncount++;
					Spawn ();

				} else {
					toSpawn = Resource2;
					Spawncount++;
					Spawn ();
				}
				ticker = 60;
			}
		}
	}
	public void Spawn () {
		//Vector3 pee = new Vector3((transform.position.x + Random.Range (-scatter, scatter)),transform.position.y,(transform.position.x + Random.Range (-scatter, scatter)));
		if (distributer ==1)
			pee = new Vector3 ((transform.position.x + Random.Range(-scatter, scatter)), (transform.position.y+Random.Range(0, scatter)), (transform.position.z + Random.Range(-scatter, scatter)));

		
		if (distributer ==2)
			pee = new Vector3 ((transform.position.x + Random.Range(-scatter, scatter)), (transform.position.y+Random.Range(0, scatter)), (transform.position.z + Random.Range(-scatter, scatter)));


		if (distributer ==3)
			pee = new Vector3 ((transform.position.x + Random.Range(-scatter, scatter)), (transform.position.y+Random.Range(0, scatter)), (transform.position.z + Random.Range(-scatter, scatter)));

		if (distributer > 3) {
			pee = new Vector3 ((transform.position.x + Random.Range(-scatter, scatter)), (transform.position.y+Random.Range(0, scatter)), (transform.position.z + Random.Range(-scatter, scatter)));
			distributer = 1;
		}

		GameObject spawnTemp = Instantiate(toSpawn, pee,Random.rotation);
		spawnTemp.transform.parent = transform.parent;
		distributer++;
			}}
