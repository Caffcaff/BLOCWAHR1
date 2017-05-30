using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildManager : MonoBehaviour {

	public int playerID;

	[Header("Manager Config")]
	public string groundTag = ("buildPlane");
	public gameManager gManager;
	public resourceManager rManager;
	public marketManager mManager;
	public bool onPlane = false;
	public float rotationStep = 45;
	private GameObject buildCursor;
	private GameObject negBuildCursor;
	public bool canBuild;
	public bool hasCollision;
	public GameObject structContainer;
	public GameObject wallContainer;


	// Camera Effectors: Height, Tilt Etc.
	private GameObject mCamera;
	private float initTilt;

	[Header("Cursor Setup")]
	public GameObject buildCursorS;
	public GameObject buildCursorM;
	public GameObject buildCursorL;
	public GameObject buildCursorXL;
	public GameObject buildCursorWall;
	public GameObject buildCursorGate;
	public GameObject noBuildCursor;
	public GameObject wallCursor;
	public GameObject wallNode;
	private GameObject wCursor;
	public GameObject wallMarker;

	[Header("Event Variables")]
	public int buildType;
	public Vector3 buildLocation;
	public Vector3 requestLocation;

	[Header("Wall Building")]
	public float snapDistance = 10;

	public List<Vector3> points = new List<Vector3>();
	public List<Vector3> nodePoints = new List<Vector3>();
	private List<GameObject> requestPoints = new List<GameObject> ();
	public Vector3 wallDims = new Vector3(30,1,1);
	public float wallLimit = 500.0f;
	private GameObject livePreviews;
	private GameObject allPreviews;
	public bool canWall = true;

	public float refreshRate = 0.2f;
	private float wallTick;

	[Header("Structure Prices")]
	[Tooltip("Headquarters")]
	public int Type1;
	[Tooltip("PowerStation")]
	public int Type2;
	[Tooltip("Refinery")]
	public int Type3;
	[Tooltip("Silo")]
	public int Type5;
	[Tooltip("Recycling Depot")]
	public int Type6;
	[Tooltip("Repair Depot")]
	public int Type7;
	[Tooltip("GigaFactory")]
	public int Type8;
	[Tooltip("Research Faciity")]
	public int Type9;
	[Tooltip("Aircraft Factory")]
	public int Type10;
	[Tooltip("Wall")]
	public int Type11;
	[Tooltip("Sentry Gun")]
	public int Type12;
	[Tooltip("Gate")]
	public int Type15;
	[Tooltip("unused")]
	public int Type16;
	[Tooltip("unused")]
	public int Type17;
	[Tooltip("unused")]
	public int Type18;
	[Tooltip("unused")]
	public int Type19;
	[Tooltip("unused")]
	public int Type20;

	[Header("Structure Prefabs")]
	[Tooltip("Headquarters")]
	public GameObject bType1;
	[Tooltip("PowerStation")]
	public GameObject bType2;
	[Tooltip("Refinery")]
	public GameObject bType3;
	[Tooltip("Silo")]
	public GameObject bType5;
	[Tooltip("Recycling Depot")]
	public GameObject bType6;
	[Tooltip("Repair Depot")]
	public GameObject bType7;
	[Tooltip("GigaFactory")]
	public GameObject bType8;
	[Tooltip("Research Faciity")]
	public GameObject bType9;
	[Tooltip("Aircraft Factory")]
	public GameObject bType10;
	[Tooltip("Wall")]
	public GameObject bType11;
	[Tooltip("Sentry Gun")]
	public GameObject bType12;
	[Tooltip("Gate")]
	public GameObject bType15;
	[Tooltip("unused")]
	public GameObject bType16;
	[Tooltip("unused")]
	public GameObject bType17;
	[Tooltip("unused")]
	public GameObject bType18;
	[Tooltip("unused")]
	public GameObject bType19;
	[Tooltip("unused")]
	public GameObject bType20;

	public enum State {
		Initialise,
		Setup,
		Idle,
		SetBuildCursor,
		BuildCursor,
		BuildCheck,
		Wall
	}

	public State _state;

	public enum wallState {
		
		setWallCursor,
		wallPreview,
		wallCheck,
		clear
	}

	public wallState _wall;

	void OnEnable(){
		eventManager.onBuildSelect += bCursor;
		eventManager.onBuildRequest += bCheck;
		eventManager.onBuildCancel += bCancel;
		eventManager.onLeftClick += bSubmit;
		eventManager.onRightClick += bRotate;
		eventManager.onEscapeKey += escCancel;

	}

	void OnDisable(){
		eventManager.onBuildSelect -= bCursor;
		eventManager.onBuildRequest += bCheck;
		eventManager.onBuildCancel -= bCancel;
		eventManager.onLeftClick += bSubmit;
		eventManager.onRightClick += bRotate;
		eventManager.onEscapeKey += escCancel;
	}

	// Use this for initialization
	IEnumerator Start(){
		_state = State.Initialise;

		while (true) {

			switch (_state) {
			case State.Initialise:
				initMe ();
				break;
			case State.Setup:
				setMeUp ();
				break;
			case State.Idle:
				idleState ();
				break;
			case State.SetBuildCursor:
				setBuildCursor ();
				break;
			case State.BuildCursor:
				inBuildCursor ();
				break;
			case State.BuildCheck:
				inBuildCheck ();
				break;
			case State.Wall:
				inWall ();
				break;
			}
			yield return 0;
		}	
	}
	
	// Update is called once per frame
	void initMe () {
		_state = State.Setup;
	}
	void setMeUp () {



		GameObject temp = GameObject.FindGameObjectWithTag ("playerSeed");
		playerID = temp.GetComponent<playerCommand>().playerID;

		GameObject resourceM = GameObject.FindGameObjectWithTag ("resourceManager");
		rManager = resourceM.GetComponent<resourceManager> ();
		mManager = resourceM.GetComponent<marketManager> ();
		structContainer = GameObject.FindGameObjectWithTag ("StructContainer");
		wallContainer = GameObject.FindGameObjectWithTag ("WallContainer");
		allPreviews = new GameObject ();
		livePreviews = new GameObject ();

		allPreviews.name = "allPreview";
		livePreviews.name = "livePreview";

		mCamera = GameObject.Find("RTS_Camera");
		initTilt = mCamera.transform.rotation.x;

		_state = State.Idle;
	}
	void idleState () {
		//Idle
	}
	void setBuildCursor (){
		RaycastHit hit; 
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); 
		if (Physics.Raycast (ray, out hit, 2000.0f)) {
			if (hit.collider.tag != null) {
				if (buildType == 8 | buildType == 10 | buildType == 6 | buildType == 2 | buildType == 3) {
					buildCursor = Instantiate (buildCursorM, hit.point, transform.rotation);
				}
				if (buildType == 1 | buildType == 5 | buildType == 7 | buildType == 9 | buildType == 12) {
					buildCursor = Instantiate (buildCursorS, hit.point, transform.rotation);
				}
				if (buildType == 11 | buildType == 15) {
					buildCursor = Instantiate (buildCursorWall, hit.point, transform.rotation);
				}
				buildCursor.SetActive (false);
				negBuildCursor = Instantiate (noBuildCursor, hit.point, transform.rotation);
				negBuildCursor.SetActive (false);
				_state = State.BuildCursor;
			}
		}
	}
	void inBuildCursor () {
		RaycastHit hit;
		var layerMask = ~(1 << 11);
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); 
		if (Physics.Raycast (ray, out hit, 2000.0f, layerMask)) {
			if (hit.collider.tag == groundTag) {
				canBuild = true;
				negBuildCursor.SetActive (false);
				buildCursor.SetActive (true);
				buildCursor.transform.position = hit.point;
				requestLocation = hit.point;

			} else {
				canBuild = false;
				buildCursor.SetActive (false);
				negBuildCursor.SetActive (true);
				negBuildCursor.transform.position = hit.point;
			}
		}
	}
	void inBuildCheck () {

		if (_state != State.Wall) {

			if (mManager.structs [buildType].cost <= rManager.resource [playerID]) {
				eventManager.Spend (mManager.structs [buildType].cost, playerID);
				GameObject newBuild = Instantiate (mManager.structs [buildType].prefab, requestLocation, buildCursor.transform.rotation);
				newBuild.transform.parent = structContainer.transform;
				newBuild.GetComponent<buildLogic> ().playerID = playerID;
				eventManager.onBuildConfirm (requestLocation, buildType);
				buildCursor.SetActive (false);
				negBuildCursor.SetActive (false);
				_state = State.Idle;
			} else {
				Debug.Log ("Not enough BW$");
				_state = State.BuildCursor;
			}
		}
	}
	// Event Based State Swtiches
	void bCursor (Vector3 Point, int Type) {
		if (Type != 11) {
			buildType = Type;
			_state = State.SetBuildCursor;
		} else {
			requestPoints.Clear();
			buildType = Type;
			_wall = wallState.setWallCursor;
			_state = State.Wall;
		}
	}


	void bCancel (Vector3 Point, int Type) {
		if (_state != State.Wall) {
			buildCursor.SetActive (false);
			negBuildCursor.SetActive (false);
			eventManager.onButtonExit (this.gameObject);
			_state = State.Idle;
		} else {
			_wall = wallState.clear;
		}
	}
	void bSubmit(Vector3 point){
		if (_state != State.Wall) {
			if (canBuild == true && hasCollision == false) {
				eventManager.BuildRequest (requestLocation, buildType);
			}
		} else {

			// in "Wall" State left click events.
			if(canBuild == true && hasCollision == false){
				points.Add (requestLocation);
				if (_wall == wallState.wallPreview) {
					staticPreview ();
				}
					if (_wall == wallState.setWallCursor) {
						_wall = wallState.wallPreview;
					}
				}
			}
	}

	void bRotate(Vector3 point){
		if (_state != State.Wall) {
			if (buildCursor != null) {
				buildCursor.transform.Rotate (0, rotationStep, 0);
			}
		} else {

			_wall = wallState.wallCheck;

		}
	}
	void escCancel (Vector3 point) {
		if (_state != State.Wall) {
			buildCursor.SetActive (false);
			negBuildCursor.SetActive (false);
			eventManager.ButtonExit (this.gameObject);
			_state = State.Idle;
		} else {
			_wall = wallState.clear;
		}
	}
	void bCheck (Vector3 Point, int Type) {
		_state = State.BuildCheck;
	}


	void inWall (){
	
	switch (_wall) {
	case wallState.setWallCursor:
			setWallCursor ();
			break;
	case wallState.wallPreview:
			wallPreview ();
			break;
	case wallState.wallCheck:
			wallCheck ();
			break;
		case wallState.clear:
			wallClear ();
			break;
			}
		}

	void onWallInit(){
		// _state = State.Wall;
	}

	void setWallCursor(){

		if(wCursor == null){
		wCursor = Instantiate(wallCursor, transform.position, transform.rotation);
			wCursor.transform.parent = allPreviews.transform;
		}
		if (negBuildCursor == null) {
			negBuildCursor = Instantiate (noBuildCursor, transform.position, transform.rotation);
			negBuildCursor.SetActive (false);
			wCursor.transform.parent = allPreviews.transform;
		}

		// Track Cursor

		RaycastHit hit;
		var layerMask = ~(1 << 11);
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); 
		if (Physics.Raycast (ray, out hit, 2000.0f, layerMask)) {
			if (hit.collider.tag == groundTag || hit.collider.tag == "Floor") {
				canBuild = true;
				canWall = true;
				negBuildCursor.SetActive (false);
				if (_wall != wallState.wallPreview) {
					wCursor.SetActive (true);
				} else {
					wCursor.SetActive (false);
				}
				wCursor.transform.position = hit.point;
				requestLocation = hit.point;

			} else {
				canBuild = false;
				canWall = false;
				wCursor.SetActive (false);
				negBuildCursor.SetActive (true);
				negBuildCursor.transform.position = hit.point;
			}
		}
	}

	void wallPreview(){

		bool refresh = false;

		setWallCursor ();
		checkSpan ();

		wallTick -= Time.deltaTime;
		if (wallTick <= 0) {
			refresh = true;
			wallTick = refreshRate;
		}

		if (refresh) {

			wallLimit = (mCamera.transform.position.y * 2);

			Destroy (livePreviews);
			livePreviews = new GameObject ();
			livePreviews.transform.parent = allPreviews.transform;

			float reach = 0;
			int thisPoint = (points.Count - 1);
			Vector3 point = points [thisPoint];
			Vector3 nextPoint = getMousePos ();
			Vector3 dir = (point - nextPoint);

			Quaternion direction = Quaternion.LookRotation (dir);
			List<Vector3> subPoints = new List<Vector3> ();
			float span = Vector3.Distance (point, nextPoint);

			int r = 0;
			int s = 0;

			if (span > wallLimit) {
				canBuild = false;
			} else {
				canBuild = true;
				bool genPreview = true;

				while (genPreview == true) {

					if (r == 0) {
						Vector3 tempPos = point + (dir.normalized * -wallDims.x);
						subPoints.Add (tempPos);
						reach += wallDims.x;
						r++;
					} else {
						if (reach < (span - wallDims.x)) {
							Vector3 tempPos = (subPoints [s] + (dir.normalized * -wallDims.x));
							subPoints.Add (tempPos);
							reach += wallDims.x;
							s++;
						} else {
							genPreview = false;
						}
					}
				}

				if (subPoints.Count > 0) {
					if (canWall) {
						foreach (Vector3 subPoint in subPoints) {
					
							GameObject wallSeg = Instantiate (wallMarker, subPoint, direction);
							wallSeg.transform.parent = livePreviews.transform;

						}
					}
				}
			}
		}
	}

	void wallCheck(){

		Destroy (livePreviews);
		livePreviews = new GameObject ();
		livePreviews.transform.parent = allPreviews.transform;

		foreach (GameObject point in requestPoints) {
			string pointTag = getGroundTag(point.transform.position);
			if (pointTag != groundTag && pointTag != "Floor") {
				Destroy (point, 0.2f);
			} else {
				if (_wall == wallState.wallCheck) {
					if (rManager.resource[playerID] > Type11) {
						eventManager.Spend (Type11, playerID);
						point.name = ("wall");
						point.transform.parent = wallContainer.transform;
						point.tag = ("betaStructure");
						point.layer = 9;
						nodePoints.Add (point.transform.position);
						point.GetComponent<buildLogic> ().playerID = playerID;
						eventManager.ConfirmWall (point);

					} else {
						Destroy (point, 0.2f);
					}
				}
			}
		}
		if (_wall == wallState.wallCheck) {

			Destroy (wCursor);
			buildCursor.SetActive (false);
			negBuildCursor.SetActive (false);


			eventManager.onBuildConfirm (requestLocation, buildType);

			requestPoints.Clear ();
			points.Clear ();

			_state = State.Idle;
		}
	}

	void wallClear(){

		Destroy (livePreviews);
		livePreviews = new GameObject ();
		livePreviews.transform.parent = allPreviews.transform;

		int i = 0;
		int nodes = requestPoints.Count;
	
		while (i < nodes) {
			Destroy (requestPoints[i]);
			i++;
		}

		Destroy (wCursor);
		buildCursor.SetActive (false);
		negBuildCursor.SetActive (false);
	

		eventManager.onButtonExit(this.gameObject);
		_state = State.Idle;

		requestPoints.Clear ();
		points.Clear ();

	}

	Vector3 getMousePos(){

		RaycastHit hit;
		var layerMask = ~(1 << 11);
		Vector3 tempPoint = transform.position;
		float pointDistance = Mathf.Infinity;
		Vector3 nodePos = transform.position;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); 

		if (Physics.Raycast (ray, out hit, 2000.0f, layerMask)) {
			tempPoint = hit.point;
		}

		foreach (Vector3 node in nodePoints) {
			float dis = Vector3.Distance (tempPoint, node);
			if (dis < pointDistance) {
				pointDistance = dis;
				nodePos = node;
			}
		}

		if(pointDistance < snapDistance) {
			return nodePos;
		}
		else {
			return tempPoint;
		}
	}


	void staticPreview (){

		int i = 0;
		int nodes = points.Count;

		foreach (Vector3 point in points) {

			float reach = 0;
			Vector3 nextPoint = point;

			if (i >= (nodes - 1)) {
				return;
			} 

			if (i == (nodes - 2)) {
				nextPoint = points [i + 1];
			} else {
				i++;
				continue;
			}

			Vector3 dir = (point - nextPoint);

			Quaternion direction = Quaternion.LookRotation (dir);
			List<Vector3> subPoints = new List<Vector3> ();
			float span = Vector3.Distance (point, nextPoint);

			int r = 0;
			int s = 0;

			bool genPreview = true;

			while (genPreview == true) {

				if (r == 0) {
					Vector3 tempPos = point + (dir.normalized * -wallDims.x);
					subPoints.Add (tempPos);
					reach += wallDims.x;
					r++;
				} else {
					if (reach < (span-wallDims.x)) {
						Vector3 tempPos = (subPoints [s] + (dir.normalized * -wallDims.x));
						subPoints.Add (tempPos);
						reach += wallDims.x;
						s++;
					} else {
						genPreview = false;
					}
				}
			}
				
			if (subPoints.Count > 0 && span < wallLimit) {
				Debug.Log ("Generated Span");
				foreach (Vector3 subPoint in subPoints) {

					if (i == 0) {
						GameObject initPillar = Instantiate(wallNode, point, direction);
						initPillar.transform.parent = allPreviews.transform;
						requestPoints.Add (initPillar);
					}

					GameObject wallSeg = Instantiate (wallMarker, subPoint, direction);
					wallSeg.transform.parent = allPreviews.transform;
					requestPoints.Add (wallSeg);

					GameObject wallPillar = Instantiate(wallNode, nextPoint, direction);
					wallPillar.transform.parent = allPreviews.transform;
					requestPoints.Add (wallPillar);
				}
			}
			i++;
		}
		wallCheck ();
	}

	void checkSpan(){

		if (points.Count > 0) {
			
		int count = points.Count;
		int lastIndex = (count - 1);
		Vector3 last = points [lastIndex];
		Vector3 mouse = getMousePos ();

			if (Vector3.Distance(last, mouse) > wallLimit) {
				canBuild = false;
				wCursor.SetActive (false);
				negBuildCursor.SetActive (true);
				negBuildCursor.transform.position = mouse;
			}
		}
	}

	string getGroundTag(Vector3 point){

		var layerMask = ~(1 << 11);
		Vector3 rayPos = new Vector3 (point.x, 600, point.z);
		RaycastHit hit;

		if (Physics.Raycast (rayPos, -Vector3.up, out hit, 1000,layerMask)) {
			return(hit.collider.tag);
		} else {
			return(null);
		}
}
}