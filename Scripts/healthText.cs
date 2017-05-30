using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthText : MonoBehaviour {

	public string healthAmount;
	public int managerAmount;
	private int total = 100;
	public unitAgent manager;
	public TextMesh text;
	public Color mid;
	public Color low;
	public Color danger;



	// Use this for initialization
	void Start () {
		manager = GetComponentInParent<unitAgent> ();
		text = GetComponent<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (manager != null && manager.health > 0) {
			managerAmount = manager.health;
			healthAmount = managerAmount.ToString ();
			text.text = healthAmount;
		}
		if ((total/managerAmount) < 1.5) {
				text.color = Color.white;
			}

		if ((total/managerAmount) < 2) {
				text.color = mid;
			}
		if ((total/managerAmount) < 3) {
				text.color = low;
			}
		if ((total/managerAmount) > 3) {
				text.color = danger;
			}
		}
	}
