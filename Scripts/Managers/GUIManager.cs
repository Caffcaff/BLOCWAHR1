using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {


	public class factoryIcon{
		public Image icon;
		public buildType bType;

		public factoryIcon(Image Icon, buildType Btype){
			icon = Icon;
			bType = Btype;
		}
	}

	public int playerID;

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
	private GameObject utilitiesButton;
	public string clickTag;
	public bool GUIIsActive = false;
	public GameObject buildGUI;
	private GameObject buildPage1;
	private GameObject buildPage2;
	private GameObject buildCancel;
	// Factory GUI Elements
	private GameObject factoryGUI;
	private GameObject factoryTech1;
	private GameObject factoryTech2;
	private GameObject factoryTech3;
	//Factory BList Thumbnails
	private factoryManager fManager;
	private GameObject list1;
	private GameObject list2;
	private GameObject list3;
	private GameObject list4;
	private GameObject list5;
	private GameObject list6;
	private GameObject list7;
	private GameObject list8;
	private GameObject list9;
	private GameObject list10;
	private GameObject list11;
	private Image icon1;
	private Image icon2;
	private Image icon3;
	private Image icon4;
	private Image icon5;
	private Image icon6;
	private Image icon7;
	private Image icon8;
	private Image icon9;
	private Image icon10;
	private Image icon11;
	public List<GameObject> buildIcons = new List<GameObject>();
	public List<Image> GUIIcons = new List<Image>();
	public List<factoryIcon> iconPairing = new List<factoryIcon>();

	void OnEnable(){
		eventManager.onButtonClick += onClick;
		eventManager.onButtonEnter += onGUIenter;
		eventManager.onButtonExit += onGUIexit;
		eventManager.onEscapeKey += onEsc;
		eventManager.onBuildConfirm += buildConfirm;
		eventManager.onFactorySelect += initFactoryUI;
		eventManager.onFactoryConfirm += updateIcons;
	}

	void OnDisable(){
		eventManager.onButtonClick -= onClick;
		eventManager.onButtonEnter -= onGUIenter;
		eventManager.onButtonExit -= onGUIexit;
		eventManager.onEscapeKey -= onEsc;
		eventManager.onBuildConfirm -= buildConfirm;
		eventManager.onFactorySelect -= initFactoryUI;
		eventManager.onFactoryConfirm -= updateIcons;
	}

	// Use this for initialization
	void Start () {

		GameObject temp = GameObject.FindGameObjectWithTag ("playerSeed");
		playerID = temp.GetComponent<playerCommand>().playerID;

		// Identify Buttons & UI elements
		rTextObj = GameObject.FindGameObjectWithTag ("rText");
		tTextObj = GameObject.FindGameObjectWithTag ("tText");
		tTextObj = GameObject.FindGameObjectWithTag ("map");
		minersButton = GameObject.FindGameObjectWithTag ("minerSelect");
		mGunsButton = GameObject.FindGameObjectWithTag ("mGunSelect");
		missilesButton = GameObject.FindGameObjectWithTag ("missileSelect");
		utilitiesButton = GameObject.FindGameObjectWithTag ("utilitySelect");
		buildGUI = GameObject.FindGameObjectWithTag ("buildGUI");
		buildPage1 = GameObject.FindGameObjectWithTag ("buildPage1");
		buildPage2 = GameObject.FindGameObjectWithTag ("buildPage2");
		buildCancel = GameObject.FindGameObjectWithTag ("buildCancel");
		buildGUI.SetActive (false);
		buildPage2.SetActive (false);
		buildCancel.SetActive (false);

		// Factory UI elements
		factoryGUI = GameObject.Find("factoryGUI");
		factoryTech1 = GameObject.Find("pageT1");
		factoryTech2 = GameObject.Find("pageT2");
		factoryTech3 = GameObject.Find("pageT3");
		//Factory BList Thumbnails
		list1 = GameObject.Find("list1");
		list2 = GameObject.Find("list2");
		list3 = GameObject.Find("list3");
		list4 = GameObject.Find("list4");
		list5 = GameObject.Find("list5");
		list6 = GameObject.Find("list6");
		list7 = GameObject.Find("list7");
		list8 = GameObject.Find("list8");
		list9 = GameObject.Find("list9");
		list10 = GameObject.Find("list10");
		list11 = GameObject.Find("list11");
		factoryTech2.SetActive (false);
		factoryTech3.SetActive (false);
		factoryGUI.SetActive (false);
		buildIcons.Add(list1);
		buildIcons.Add(list2);
		buildIcons.Add(list3);
		buildIcons.Add(list4);
		buildIcons.Add(list5);
		buildIcons.Add(list6);
		buildIcons.Add(list7);
		buildIcons.Add(list8);
		buildIcons.Add(list9);
		buildIcons.Add(list10);
		buildIcons.Add(list11);

		// Setup Access to Icons
		icon1 = list1.GetComponent<Image>();
		icon2 = list2.GetComponent<Image>();
		icon3 = list3.GetComponent<Image>();
		icon4 = list4.GetComponent<Image>();
		icon5 = list5.GetComponent<Image>();
		icon6 = list6.GetComponent<Image>();
		icon7 = list7.GetComponent<Image>();
		icon8 = list8.GetComponent<Image>();
		icon9 = list9.GetComponent<Image>();
		icon10 = list10.GetComponent<Image>();
		icon11 = list11.GetComponent<Image>();
		// List Icons
		GUIIcons.Add(icon1);
		GUIIcons.Add(icon2);
		GUIIcons.Add(icon3);
		GUIIcons.Add(icon4);
		GUIIcons.Add(icon5);
		GUIIcons.Add(icon6);
		GUIIcons.Add(icon7);
		GUIIcons.Add(icon8);
		GUIIcons.Add(icon9);
		GUIIcons.Add(icon10);
		GUIIcons.Add (icon11);
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
		if (button == utilitiesButton) {
			clickTag = ("Utility");
			eventManager.TypeSelect (clickTag);
		}
		if (button.name == ("buildButton")){
			onBuildGUI ();
		}
		if (button.name == ("destroyButton")){
			onDestroy ();
		}
		if (button.name == "Patrol") {
			Debug.Log ("Patrol clicked");
			eventManager.onPatrolEnter (transform.position, transform.position, playerID);
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

		// Factory GUI Buttons

		if (button.name == ("factoryExit")){
			factoryGUIQuit();
		}
		if (button.name == ("t1Select")){
			factoryTech1.SetActive (true);
			factoryTech2.SetActive (false);
			factoryTech3.SetActive (false);
		}
		if (button.name == ("t2Select")){
			factoryTech1.SetActive (false);
			factoryTech2.SetActive (true);
			factoryTech3.SetActive (false);
		}
		if (button.name == ("t3Select")){
			factoryTech1.SetActive (false);
			factoryTech2.SetActive (false);
			factoryTech3.SetActive (true);
		}
		if (button.name == ("unitBuild1")){
			eventManager.FactoryOrder (fManager.gameObject, unitSettings.miner1);
		}
		if (button.name == ("unitBuild11")){
			eventManager.FactoryOrder (fManager.gameObject, unitSettings.mGun1);
		}
		if (button.name == ("unitBuild21")){
			eventManager.FactoryOrder (fManager.gameObject, unitSettings.missile1);
		}
		if (button.name == ("unitBuild31")){
			eventManager.FactoryOrder (fManager.gameObject, unitSettings.repair1);
		}
		if (button.name == ("unitBuild2")){
			eventManager.FactoryOrder (fManager.gameObject, unitSettings.miner2);
		}
		if (button.name == ("unitBuild12")){
			eventManager.FactoryOrder (fManager.gameObject, unitSettings.mGun2);
		}
		if (button.name == ("unitBuild22")){
			eventManager.FactoryOrder (fManager.gameObject, unitSettings.missile2);
		}
		if (button.name == ("unitBuild32")){
			eventManager.FactoryOrder (fManager.gameObject, unitSettings.repair2);
		}
		if (button.name == ("unitBuild3")){
			eventManager.FactoryOrder (fManager.gameObject, unitSettings.miner3);
		}
		if (button.name == ("unitBuild13")){
			eventManager.FactoryOrder (fManager.gameObject, unitSettings.mGun3);
		}
		if (button.name == ("unitBuild23")){
			eventManager.FactoryOrder (fManager.gameObject, unitSettings.missile3);
		}
		if (button.name == ("unitBuild33")){
			eventManager.FactoryOrder (fManager.gameObject, unitSettings.repair3);
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
	void initFactoryUI(GameObject factory, int ID){
		
		fManager = factory.GetComponent<factoryManager> ();
		factoryGUI.SetActive (true);
		factoryTech1.SetActive (true);
		factoryTech2.SetActive (false);
		factoryTech3.SetActive (false);

		// Setup Current Factory Variables

		if (fManager.buildList.Count < 1) {
			foreach (GameObject icon in buildIcons) {
				icon.SetActive (false);
			}
		} else {
			Debug.Log ("Stage 3");
			int i = 0;
			foreach (Image icon in GUIIcons) {
				if (i < fManager.buildList.Count) {
					icon.gameObject.SetActive (true);
					icon.sprite = fManager.buildList [i].Type.icon;
					Debug.Log ("Icon Updated");
					i++;
				}
			}
		}

	}

	void updateIcons(GameObject factory, buildType type){

		if(factory.GetComponent<buildLogic>().playerID == playerID){

		factoryManager tFManager = factory.GetComponent<factoryManager> ();

		if (tFManager.buildList.Count < 1) {
			foreach (GameObject icon in buildIcons) {
				icon.SetActive (false);
			}
		} else {
			Debug.Log ("Stage 3");
			int i = 0;
			foreach (Image icon in GUIIcons) {
				if (i < tFManager.buildList.Count) {
					icon.gameObject.SetActive (true);
					icon.sprite = tFManager.buildList [i].Type.icon;
					Debug.Log ("Icon Updated");
					i++;
				} else {
					icon.gameObject.SetActive (false);
				}
			}
		}
	}
	}

	void factoryGUIQuit(){
		factoryGUI.SetActive (false);
		eventManager.ButtonExit (this.gameObject);
	}
}
