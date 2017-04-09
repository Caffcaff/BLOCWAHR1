using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

	[Header("GUI Input Settings")]

	private GameObject rTextObj;
	private GameObject tTextObj;
	private GameObject MapObj;
	private Text rText;
	private Text tText;
	private Image Map;
	private GameObject minersButton;
	private GameObject mGunsButton;
	private GameObject missilesButton;
	public string clickTag;
	public bool GUIIsActive = false;

	void OnEnable(){
		eventManager.onButtonClick += onClick;
		eventManager.onButtonEnter += onGUIenter;
		eventManager.onButtonExit += onGUIexit;
	}

	void OnDisable(){
		eventManager.onButtonClick -= onClick;
		eventManager.onButtonEnter -= onGUIenter;
		eventManager.onButtonExit -= onGUIexit;
	}

	// Use this for initialization
	void Start () {
		rTextObj = GameObject.FindGameObjectWithTag ("rText");
		tTextObj = GameObject.FindGameObjectWithTag ("tText");
		tTextObj = GameObject.FindGameObjectWithTag ("map");
		minersButton = GameObject.FindGameObjectWithTag ("minerSelect");
		mGunsButton = GameObject.FindGameObjectWithTag ("mGunSelect");
		missilesButton = GameObject.FindGameObjectWithTag ("missileSelect");
	}
	
	// Update is called once per frame
	void onClick (GameObject button) {
		if (button == minersButton){
			clickTag = ("Miner");
			eventManager.TypeSelect(clickTag);
		}
		if (button == mGunsButton){
			clickTag = ("mGun");
			eventManager.TypeSelect(clickTag);
		}
		if (button == missilesButton){
			clickTag = ("Missile");
			eventManager.TypeSelect(clickTag);
		}
//	if (button == minersButton){
//		clickTag = ("Build");
//		eventManager.BuildCick(clickTag);
//	}
//	if (button == mGunsButton){
//		clickTag = ("Destroy");
//		eventManager.DestroyClick(clickTag);
//	}
//	if (button == missilesButton){
//		clickTag = ("buildCard");
//		eventManager.BuildSelect(buildID);
//	}
	}
	void onGUIenter(GameObject unused){
		if (GUIIsActive == false) {	
			GUIIsActive = true;
			Debug.Log ("GUI is Active");
		}
	}
	void onGUIexit(GameObject unused){
		if (GUIIsActive == true) {
			GUIIsActive = false;
			Debug.Log ("GUI is Not Active");
		}
	}
}
