using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIResource : MonoBehaviour {

	public resourceManager rManager;
	public int playerID;
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
		GameObject temp = GameObject.FindGameObjectWithTag ("playerSeed");
		playerID = temp.GetComponent<playerCommand>().playerID;
		manager = GameObject.FindGameObjectWithTag ("resourceManager");
		rManager = manager.GetComponent<resourceManager>();
		rText = GetComponent<Text>();
		rTotal = rManager.resource[playerID];
		refresh ();
	}
	
	// Update is called once per frame
	void rUpdate (int amount, int type) {
		rTotal = rManager.resource[playerID];
		refresh ();
	}
	void refresh(){
		string displayTotal = rTotal.ToString();
		rText.text = currency + displayTotal;
	}
}
