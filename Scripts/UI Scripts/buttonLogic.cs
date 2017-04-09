using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonLogic : MonoBehaviour {

	// Update is called once per frame
	public void clicked () {
		eventManager.ButtonClick(this.gameObject);
	}
	public void enter () {
		eventManager.ButtonEnter(this.gameObject);
	}
	public void exit () {
		eventManager.ButtonExit(this.gameObject);
	}
}
