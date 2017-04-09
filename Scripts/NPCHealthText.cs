using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHealthText : MonoBehaviour {

	public string healthAmount;
	public int managerAmount;
	public enemyManager manager;
	public TextMesh text;


	// Use this for initialization
	void Start () {
		manager = GetComponentInParent<enemyManager> ();
		text = GetComponent<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () {
		managerAmount = manager.health;
		healthAmount = managerAmount.ToString();;

		text.text = healthAmount;
	}
}
