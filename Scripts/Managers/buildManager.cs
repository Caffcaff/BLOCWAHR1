using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildManager : MonoBehaviour {

	[Header("Manager Config")]
	public string groundTag = ("buildPlane");
	public gameManager gManager;
	public resourceManager rManager;
	public bool onPlane = false;
	public float rotationStep = 45;
	private GameObject buildCursor;
	private GameObject negBuildCursor;
	public bool canBuild;
	public bool hasCollision;
	public GameObject structContainer;


	[Header("Cursor Setup")]
	public GameObject buildCursorS;
	public GameObject buildCursorM;
	public GameObject buildCursorL;
	public GameObject buildCursorXL;
	public GameObject buildCursorWall;
	public GameObject buildCursorGate;
	public GameObject noBuildCursor;

	[Header("Event Variables")]
	public int buildType;
	public Vector3 buildLocation;
	public Vector3 requestLocation;

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

	[Header("Structure Prices")]
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
		BuildCheck
	}

	public State _state;

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
			}
			yield return 0;
		}	
	}
	
	// Update is called once per frame
	void initMe () {
		_state = State.Setup;
	}
	void setMeUp () {
		GameObject resourceM = GameObject.FindGameObjectWithTag ("resourceManager");
		rManager = resourceM.GetComponent<resourceManager> ();
		structContainer = GameObject.FindGameObjectWithTag ("StructContainer");
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
				if (buildType == 1 | buildType == 5 | buildType == 7 | buildType == 9) {
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
		if (buildType == 1) {
			if (Type1 <= rManager.playerResource) {
				eventManager.onSpend (Type1, 1);
				GameObject newBuild = Instantiate (bType1, requestLocation, buildCursor.transform.rotation);
				newBuild.transform.parent = structContainer.transform;
				eventManager.onBuildConfirm (requestLocation, buildType);
				Destroy (buildCursor);
				Destroy (negBuildCursor);
				_state = State.Idle;
			}
		}
		if (buildType == 2) {
			if (Type1 <= rManager.playerResource) {
				eventManager.onSpend (Type2, 1);
				GameObject newBuild = Instantiate (bType2, requestLocation, buildCursor.transform.rotation);
				newBuild.transform.parent = structContainer.transform;
				eventManager.onBuildConfirm (requestLocation, buildType);
				Destroy (buildCursor);
				Destroy (negBuildCursor);
				_state = State.Idle;

			}
		}
		if (buildType == 3) {
			if (Type1 <= rManager.playerResource) {
				eventManager.onSpend (Type3, 1);
				GameObject newBuild = Instantiate (bType3, requestLocation, buildCursor.transform.rotation);
				newBuild.transform.parent = structContainer.transform;
				eventManager.onBuildConfirm (requestLocation, buildType);
				Destroy (buildCursor);
				Destroy (negBuildCursor);
				_state = State.Idle;

			}
		}
		if (buildType == 5) {
			if (Type1 <= rManager.playerResource) {
				eventManager.onSpend (Type5, 1);
				GameObject newBuild = Instantiate (bType5, requestLocation, buildCursor.transform.rotation);
				newBuild.transform.parent = structContainer.transform;
				eventManager.onBuildConfirm (requestLocation, buildType);
				Destroy (buildCursor);
				Destroy (negBuildCursor);
				_state = State.Idle;

			}
		}
		if (buildType == 6) {
			if (Type1 <= rManager.playerResource) {
				eventManager.onSpend (Type6, 1);
				GameObject newBuild = Instantiate (bType6, requestLocation, buildCursor.transform.rotation);
				newBuild.transform.parent = structContainer.transform;
				eventManager.onBuildConfirm (requestLocation, buildType);
				Destroy (buildCursor);
				Destroy (negBuildCursor);
				_state = State.Idle;
			}
		}
		if (buildType == 7) {
			if (Type1 <= rManager.playerResource) {
				eventManager.onSpend (Type7, 1);
				GameObject newBuild = Instantiate (bType7, requestLocation, buildCursor.transform.rotation);
				newBuild.transform.parent = structContainer.transform;
				eventManager.onBuildConfirm (requestLocation, buildType);
				Destroy (buildCursor);
				Destroy (negBuildCursor);
				_state = State.Idle;
			}
		}
		if (buildType == 8) {
			if (Type1 <= rManager.playerResource) {
				eventManager.onSpend (Type8, 1);
				GameObject newBuild = Instantiate (bType8, requestLocation, buildCursor.transform.rotation);
				newBuild.transform.parent = structContainer.transform;
				eventManager.onBuildConfirm (requestLocation, buildType);
				Destroy (buildCursor);
				Destroy (negBuildCursor);
				_state = State.Idle;
			}
		}
		if (buildType == 9) {
			if (Type1 <= rManager.playerResource) {
				eventManager.onSpend (Type9, 1);
				GameObject newBuild = Instantiate (bType9, requestLocation, buildCursor.transform.rotation);
				newBuild.transform.parent = structContainer.transform;
				eventManager.onBuildConfirm (requestLocation, buildType);
				Destroy (buildCursor);
				Destroy (negBuildCursor);
				_state = State.Idle;
			}
		}
		if (buildType == 10) {
			if (Type1 <= rManager.playerResource) {
				eventManager.onSpend (Type10, 1);
				GameObject newBuild = Instantiate (bType10, requestLocation, buildCursor.transform.rotation);
				newBuild.transform.parent = structContainer.transform;
				eventManager.onBuildConfirm (requestLocation, buildType);
				Destroy (buildCursor);
				Destroy (negBuildCursor);
				_state = State.Idle;
			}
		}
		if (buildType == 11) {
			if (Type1 <= rManager.playerResource) {
				eventManager.onSpend (Type11, 1);
				GameObject newBuild = Instantiate (bType11, requestLocation, buildCursor.transform.rotation);
				newBuild.transform.parent = structContainer.transform;
				eventManager.onBuildConfirm (requestLocation, buildType);
				Destroy (buildCursor);
				Destroy (negBuildCursor);
				_state = State.Idle;
			}
		}
		if (buildType == 12) {
			if (Type1 <= rManager.playerResource) {
				eventManager.onSpend (Type12, 1);
				GameObject newBuild = Instantiate (bType12, requestLocation, buildCursor.transform.rotation);
				newBuild.transform.parent = structContainer.transform;
				eventManager.onBuildConfirm (requestLocation, buildType);
				Destroy (buildCursor);
				Destroy (negBuildCursor);
				_state = State.Idle;
			}

		}
		if (buildType == 15) {
			if (Type1 <= rManager.playerResource) {
				eventManager.onSpend (Type15, 1);
				GameObject newBuild = Instantiate (bType15, requestLocation, buildCursor.transform.rotation);
				newBuild.transform.parent = structContainer.transform;
				eventManager.onBuildConfirm (requestLocation, buildType);
				Destroy (buildCursor);
				Destroy (negBuildCursor);
				_state = State.Idle;
			}
		}
		if (buildType == 16) {
			if (Type1 <= rManager.playerResource) {
				eventManager.onSpend (Type16, 1);
				GameObject newBuild = Instantiate (bType16, requestLocation, buildCursor.transform.rotation);
				newBuild.transform.parent = structContainer.transform;
				eventManager.onBuildConfirm (requestLocation, buildType);
				Destroy (buildCursor);
				Destroy (negBuildCursor);
				_state = State.Idle;
			}
		}
		if (buildType == 17) {
			if (Type1 <= rManager.playerResource) {
				eventManager.onSpend (Type17, 1);
				GameObject newBuild = Instantiate (bType17, requestLocation, buildCursor.transform.rotation);
				newBuild.transform.parent = structContainer.transform;
				eventManager.onBuildConfirm (requestLocation, buildType);
				Destroy (buildCursor);
				Destroy (negBuildCursor);
				_state = State.Idle;
			}
		}
		if (buildType == 18) {
			if (Type1 <= rManager.playerResource) {
				eventManager.onSpend (Type18, 1);
				GameObject newBuild = Instantiate (bType18, requestLocation, buildCursor.transform.rotation);
				newBuild.transform.parent = structContainer.transform;
				eventManager.onBuildConfirm (requestLocation, buildType);
				Destroy (buildCursor);
				Destroy (negBuildCursor);
				_state = State.Idle;
			}
		}
		if (buildType == 19) {
			if (Type1 <= rManager.playerResource) {
				eventManager.onSpend (Type19, 1);
				GameObject newBuild = Instantiate (bType19, requestLocation, buildCursor.transform.rotation);
				newBuild.transform.parent = structContainer.transform;
				eventManager.onBuildConfirm (requestLocation, buildType);
				Destroy (buildCursor);
				Destroy (negBuildCursor);
				_state = State.Idle;
			}
		}
		if (buildType == 20) {
			if (Type1 <= rManager.playerResource) {
				eventManager.onSpend (Type20, 1);
				GameObject newBuild = Instantiate (bType20, requestLocation, buildCursor.transform.rotation);
				newBuild.transform.parent = structContainer.transform;
				eventManager.onBuildConfirm (requestLocation, buildType);
				Destroy (buildCursor);
				Destroy (negBuildCursor);
				_state = State.Idle;
			}
		}
	}
	// Event Based State Swtiches
	void bCursor (Vector3 Point, int Type) {
		buildType = Type;
		_state = State.SetBuildCursor;
	}
	void bCancel (Vector3 Point, int Type) {
		Destroy (buildCursor);
		Destroy (negBuildCursor);
		eventManager.onButtonExit(this.gameObject);
		_state = State.Idle;
	}
	void bSubmit(Vector3 point){
		if (canBuild == true && hasCollision == false) {
			eventManager.onBuildRequest (requestLocation, buildType);
		}
	}
	void bRotate(Vector3 point){
		buildCursor.transform.Rotate(0, rotationStep, 0);
	}
	void escCancel (Vector3 point) {
		Destroy (buildCursor);
		Destroy (negBuildCursor);
		eventManager.onButtonExit(this.gameObject);
		_state = State.Idle;
	}
	void bCheck (Vector3 Point, int Type) {
		_state = State.BuildCheck;
	}
}