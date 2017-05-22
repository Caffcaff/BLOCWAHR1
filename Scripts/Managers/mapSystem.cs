using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mapSystem : MonoBehaviour {


	public Camera mainCam;
	public float trackSpeed = 500;
	public bool isTracking = false;
	public Vector3 requestLocation = new Vector3(0,0,0);
	public Vector2 mapCoords;

	public float terrainX = 10000;
	public float terrainY = 10000;
	public float mapX = 100;
	public float mapY = 100;
	private float modifierX;
	private float modifierY;
	public float heightCap = 600;


	public void Start(){
		mainCam = Camera.main;
		modifierX = terrainX / mapX;
		modifierY = terrainY / mapY;
	}

	public void Update(){
		if (Input.anyKeyDown) {
			isTracking = false;
		}
		if (isTracking == true && Vector3.Distance (mainCam.transform.position, requestLocation) > 10) {
			moveCam ();
		}
			else{
				isTracking = false;
			}
	}

		public void clicked () {

		Vector2 mp = Input.mousePosition;
		CanvasScaler canvasScale = GetComponentInParent<CanvasScaler> ();

		Vector3[] corners = new Vector3[4];
		GetComponent<RawImage> ().rectTransform.GetWorldCorners (corners);
		Rect newRect = new Rect (corners [0], corners [2] - corners [0]);
		Debug.Log (newRect.Contains (Input.mousePosition));

		float xPositionDeltaPoint = mp.x - newRect.x;
		float yPositionDeltaPoint = mp.y - newRect.y;

		//The value "170" is the raw image size.
		float compensateForScalingX = mapX * canvasScale.scaleFactor;
		//"600" is the current reference resolution height on the Canvas Scaler script.
		float compensateForScalingY = mapY * /*(Screen.height / 100)* */  canvasScale.scaleFactor;
	 
		//	if (compensateForScalingY == 0)
		//	{
		//		compensateForScalingY = mapY;
		//	}

		float xPositionCameraCoordinates = (xPositionDeltaPoint / compensateForScalingX);
		float yPositionCameraCoordinates = (yPositionDeltaPoint / compensateForScalingY);

		float tempX = ((xPositionCameraCoordinates * 200) * modifierX);
		float tempY = ((yPositionCameraCoordinates * 200) * modifierY);

		requestLocation = new Vector3(tempX,0,tempY);

		float camY = Terrain.activeTerrain.SampleHeight(mainCam.transform.position);
		float terrainY = Terrain.activeTerrain.SampleHeight(requestLocation);
		requestLocation.y = camY + terrainY;

		if (terrainY > heightCap) {
			return;
		} else {
			moveCam ();
			isTracking = true;
		}			
	}

	public void moveCam(){
			Vector3 curPos = Vector3.Lerp(mainCam.transform.position, requestLocation, (trackSpeed * Time.deltaTime));
			mainCam.transform.position = curPos;
	}
	
	
	
	
	}
