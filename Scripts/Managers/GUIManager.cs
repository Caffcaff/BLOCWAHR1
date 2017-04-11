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
	public GameObject buildGUI;
	private GameObject buildPage1;
	private GameObject buildPage2;
	private GameObject buildCancel;


	void OnEnable(){
		eventManager.onButtonClick += onClick;
		eventManager.onButtonEnter += onGUIenter;
		eventManager.onButtonExit += onGUIexit;
		eventManager.onEscapeKey += onEsc;
		eventManager.onBuildConfirm += buildConfirm;
	}

	void OnDisable(){
		eventManager.onButtonClick -= onClick;
		eventManager.onButtonEnter -= onGUIenter;
		eventManager.onButtonExit -= onGUIexit;
		eventManager.onEscapeKey -= onEsc;
		eventManager.onBuildConfirm -= buildConfirm;
	}

	// Use this for initialization
	void Start () {
		rTextObj = GameObject.FindGameObjectWithTag ("rText");
		tTextObj = GameObject.FindGameObjectWithTag ("tText");
		tTextObj = GameObject.FindGameObjectWithTag ("map");
		minersButton = GameObject.FindGameObjectWithTag ("minerSelect");
		mGunsButton = GameObject.FindGameObjectWithTag ("mGunSelect");
		missilesButton = GameObject.FindGameObjectWithTag ("missileSelect");
		buildGUI = GameObject.FindGameObjectWithTag ("buildGUI");
		buildPage1 = GameObject.FindGameObjectWithTag ("buildPage1");
		buildPage2 = GameObject.FindGameObjectWithTag ("buildPage2");
		buildCancel = GameObject.FindGameObjectWithTag ("buildCancel");
		buildGUI.SetActive (false);
		buildPage2.SetActive (false);
		buildCancel.SetActive (false);

	}
	
	// Update is called once per frame
	void onClick (GameObject button) {

		//HUD Buttons

		if (button == minersButton) {
			clickTag = ("Miner");
			eventManager.TypeSelect (clickTag);
		}
		if (button == mGunsButton) {
			clickTag = ("mGun");
			eventManager.TypeSelect (clickTag);
		}
		if (button == missilesButton) {
			clickTag = ("Missile");
			eventManager.TypeSelect (clickTag);
		}
		if (button.name == ("buildButton")){
			onBuildGUI ();
		}
		if (button.name == ("destroyButton")){
			onDestroy ();
		}

		//Build GUI Buttons

		if (button.name == ("buildExit")){
			onBuildQuit ();
		}
		if (button.name == ("buildForward")){
			onBuildForward ();
		}
		if (button.name == ("buildBack")){
			onBuildBack ();
		}
		if (button.name == ("Build1")){
			eventManager.BuildSelect (transform.position,1);
			buildCancel.SetActive (true);
			onBuildQuit ();
		}
		if (button.name == ("Build2")){
			eventManager.BuildSelect (transform.position,2);
			buildCancel.SetActive (true);
			onBuildQuit ();
		}
		if (button.name == ("Build3")){
			eventManager.BuildSelect (transform.position,3);
			buildCancel.SetActive (true);
			onBuildQuit ();
		}
		if (button.name == ("Build5")){
			eventManager.BuildSelect (transform.position,5);
			buildCancel.SetActive (true);
			onBuildQuit ();
		}
		if (button.name == ("Build6")){
			eventManager.BuildSelect (transform.position,6);
			buildCancel.SetActive (true);
			onBuildQuit ();
		}
		if (button.name == ("Build7")){
			eventManager.BuildSelect (transform.position,7);
			buildCancel.SetActive (true);
			onBuildQuit ();
		}
		if (button.name == ("Build8")){
			eventManager.BuildSelect (transform.position,8);
			buildCancel.SetActive (true);
			onBuildQuit ();
		}
		if (button.name == ("Build9")){
			eventManager.BuildSelect (transform.position,9);
			buildCancel.SetActive (true);
			onBuildQuit ();
		}
		if (button.name == ("Build10")){
			eventManager.BuildSelect (transform.position,10);
			buildCancel.SetActive (true);
			onBuildQuit ();
		}
		if (button.name == ("Build11")){
			eventManager.BuildSelect (transform.position,11);
			buildCancel.SetActive (true);
			onBuildQuit ();
		}
		if (button.name == ("Build12")){
			eventManager.BuildSelect (transform.position,12);
			buildCancel.SetActive (true);
			onBuildQuit ();
		}
		if (button.name == ("Build15")){
			eventManager.BuildSelect (transform.position,15);
			buildCancel.SetActive (true);
			onBuildQuit ();
		}
		if (button.name == ("Build16")){
			eventManager.BuildSelect (transform.position,16);
			buildCancel.SetActive (true);
			onBuildQuit ();
		}
		if (button.name == ("Build17")){
			eventManager.BuildSelect (transform.position,17);
			buildCancel.SetActive (true);
			onBuildQuit ();
		}
		if (button.name == ("Build18")){
			eventManager.BuildSelect (transform.position,18);
			buildCancel.SetActive (true);
			onBuildQuit ();
		}
		if (button.name == ("Build19")){
			eventManager.BuildSelect (transform.position,19);
			buildCancel.SetActive (true);
			onBuildQuit ();
		}
		if (button.name == ("buildCancel")){
			eventManager.BuildCancel (transform.position,19);
			eventManager.ButtonExit (buildCancel);
			buildCancel.SetActive (false);
		}
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

	void onDestroy (){
		Debug.Log ("Destroy Clicked");
	}


	//Build GUI Settings


	void onBuildGUI(){
		if (GUIIsActive == false) {	
			GUIIsActive = true;
		}
		buildGUI.SetActive(true);
	}
	void onBuildQuit(){
		buildGUI.SetActive(false);
		if (GUIIsActive == true) {	
			GUIIsActive = false;
		}
	}
	void onBuildForward(){
		if (GUIIsActive == false) {	
			GUIIsActive = true;
		}
		if (buildPage1.activeInHierarchy == true) {
				buildPage2.SetActive(true);
				buildPage1.SetActive(false);
		}
	}
	void onBuildBack(){
		if (GUIIsActive == false) {	
			GUIIsActive = true;
		}
		if (buildPage2.activeInHierarchy == true) {
				buildPage1.SetActive(true);
				buildPage2.SetActive(false);
		}
	}
	void onEsc(Vector3 point){
		if (buildCancel.activeInHierarchy == true) {
			buildCancel.SetActive (false);
		}
	}
	void buildConfirm(Vector3 point, int type){
		if (buildCancel.activeInHierarchy == true) {
			buildCancel.SetActive (false);
		}
	}
}
