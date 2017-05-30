using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cargoLogic : MonoBehaviour {

	public GameObject[] cargoPods;
	private unitAgent uLogic;
	public int podsCount;
	public int capacity;
	public float refreshRate = 1;
	private float refresh;


	// Use this for initialization
	void Start () {
		podsCount = cargoPods.Length;
		uLogic = GetComponentInParent<unitAgent> ();
		foreach (GameObject pod in cargoPods) {
		pod.SetActive(false);
		capacity = uLogic.rCapacity;
		}
		refresh = refreshRate;
	}
	// Update is called once per frame
	void Update () {
		refresh -= Time.deltaTime;

		if (refresh <= 0) {
			refresh = refreshRate;
			updatePods ();
			}

	 	
	}
	void updatePods () {

		float podRatio = capacity / podsCount;
		float toRound = ((float)uLogic.cargo / podRatio);

		float roundedCargo = Mathf.Round(toRound); 

		int activePod = (int)roundedCargo;
		int i = 1;

		foreach(GameObject pod in cargoPods){
			if(i <= activePod){
				pod.SetActive(true);
			}
			else{
				pod.SetActive(false);
			}
			i++;
		}
	}
}
