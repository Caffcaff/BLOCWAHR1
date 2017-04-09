using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIResource : MonoBehaviour {

	public resourceManager rManager;
	public Text rText;
	public int rTotal;
	public string currency = ("BW$ ");
	private GameObject manager;

	void OnEnable(){
		eventManager.onCollect += rUpdate;
		eventManager.onSpend += rUpdate;
	}

	void OnDisable(){
		eventManager.onCollect -= rUpdate;
		eventManager.onSpend -= rUpdate;
	}

	// Use this for initialization
	void Start () {
		manager = GameObject.FindGameObjectWithTag ("resourceManager");
		rManager = manager.GetComponent<resourceManager>();
		rText = GetComponent<Text>();
		rTotal = rManager.playerResource;
		refresh ();
	}
	
	// Update is called once per frame
	void rUpdate (int amount, int type) {
		rTotal = rManager.playerResource;
		refresh ();
	}
	void refresh(){
		string displayTotal = rTotal.ToString();
		rText.text = currency + displayTotal;
	}
}
