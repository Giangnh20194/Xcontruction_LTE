using UnityEngine;
using System.Collections;

//using UnityEngine.UI;
using System;

public class TouchControl : MonoBehaviour
{
	public RailDrawManager RailManager;
	public LineRailScript lineRailUI;
	bool selected = false;
	LayerMask maskPointRail, maskRailTemp;
	RaycastHit2D hit2d;
	Vector3 pointTouch;
	Vector3 pointStartRail;
	[HideInInspector]
	public Rigidbody2D rgCurrent;
	public bool isBuild = false;
	public bool isRemove = false;
	//	public Text txtCountTouch;
	bool draged = false;
	[HideInInspector]
	public Camera cameraMain;
	[Header ("Camera Max")]
	public Transform[] pointLimitCam = new Transform[4];
	public Transform[] pointCamBorder = new Transform[4];
	float xMax, xMin, yMax, yMin;

	void Start ()
	{
		maskPointRail = LayerMask.NameToLayer ("POINT_RAIL");
		maskRailTemp = LayerMask.NameToLayer ("RAILTEMP");
		cameraMain = Camera.main;
		this._takeLimitCam ();
		isDraw = true;
	}
	public bool isDraw = false;
	Vector3 pointEndRail;
	int countTouch = 0;
	public bool blockDrag = false;
	void Update ()
	{
		this.countTouch = Input.touchCount;
		if (countTouch <= 1) {
			if (isDraw) {
				if (isBuild) {
					if (!draged) {
						// Build mode

						if (!this.selected) {
							this.CheckPointBuild ();
						} else {
							if (Input.GetMouseButton (0)) {
								this.pointTouch = Camera.main.ScreenToWorldPoint (Input.mousePosition);
								this.pointTouch = new Vector3 (pointTouch.x, pointTouch.y, 0);
								this.pointEndRail = pointTouch;
								this.lineRailUI.SetInfo (pointStartRail, pointEndRail);
							} else {
								this.selected = false;
								RailManager.DrawRailPhysics ();
							}
						}
					}
				} else {
				
				}
			}
			if (isRemove) {
				if (!draged) {					// Remove Mode{
					this.CheckRailInRemoveMode ();
				}
			
			} else {
			}
			if (draged) {
				if (Input.GetMouseButton (0)) {
					this._Dragcamera ();
				} else {
					draged = false;
				}
			}

		} else {
			//txtCountTouch.text = "" + countTouch;
		}
	}

	private void CheckPointBuild ()
	{
		if (Input.GetMouseButton (0)) {
			pointTouch = cameraMain.ScreenToWorldPoint (Input.mousePosition);
			hit2d = Physics2D.Raycast (pointTouch, Vector2.zero, 1, 1 << maskPointRail);
			if (hit2d != null && hit2d.collider != null) {
				if (hit2d.collider.tag == "PointRailMain") {
					this.pointStartRail = hit2d.transform.position;
					this.rgCurrent = hit2d.transform.GetComponent<Rigidbody2D> ();
					lineRailUI.setPivot (pointStartRail);
					this.selected = true;
				}
				if (hit2d.collider.tag == "PointRailTemp") {
					this.pointStartRail = hit2d.transform.position;
					this.rgCurrent = hit2d.transform.GetComponent<Rigidbody2D> ();
					lineRailUI.setPivot (pointStartRail);
					this.selected = true;
				}
				if (hit2d.collider.tag == "PointRail") {
					this._startDrag ();
				}
			} else {
				this._startDrag ();
			}
		} 
	}

	private void _startDrag ()
	{
		if (!blockDrag) {
			if (!draged) {
				oldPos = cameraMain.transform.position;
				panOrigin = Camera.main.ScreenToViewportPoint (Input.mousePosition);
				draged = true;
			}
		}
	}

	public void _BuildMode (bool status)
	{
		this.isBuild = status;
		this.isRemove = !isBuild;
	}

	RailPhysicsScript railPhysicsScriptTemp;

	private void CheckRailInRemoveMode ()
	{
		if (Input.GetMouseButton (0)) {
			print ("Remove!!!!!!");
			this.pointTouch = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			hit2d = Physics2D.Raycast (pointTouch, Vector2.zero, 1, 1 << maskRailTemp);
			if (hit2d != null && hit2d.collider != null) {
				if (hit2d.collider.tag == "RailTemp") {
					this.railPhysicsScriptTemp = hit2d.collider.GetComponent<RailPhysicsScript> ();
					this.railPhysicsScriptTemp.RemoveRailPhysics ();
				}
				if (hit2d.collider.tag == "PointRail") {
					this._startDrag ();
				}
			} else {
				this._startDrag ();
			}

		}
		this.railPhysicsScriptTemp = null;
	}

	Vector3 oldPos, panOrigin;
	public float panSpeed = 10;
	bool[] drag = new bool[4];

	public void _Dragcamera ()
	{

//		Vector3 pointTouchCam = cameraMain.ScreenToWorldPoint (Input.mousePosition);
////		if(Vector3.Distance(pointTouchCam,pointStart) > 0.5f){
////		}
//		Vector3 newPoint = (pointTouchCam - this.pointStart);
//		pointStart = newPoint;
//		print (newPoint);
//		Vector3 newPointCam = cameraMain.transform.position + new Vector3 (newPoint.x, newPoint.y, -10);
//		newPointCam = new Vector3 (newPointCam.x, newPointCam.y, -10);
//		cameraMain.transform.position = newPointCam;
		Vector3 pos = Camera.main.ScreenToViewportPoint (Input.mousePosition) - panOrigin;   
		Vector3 position = oldPos + -pos * panSpeed; 
		//Get the difference between where the mouse clicked and where it moved
//		if ((pointLimitCam [0].position.y - pointCamBorder [0].position.y) <= 0) {
//			
//		} else if ((pointLimitCam [1].position.y - pointCamBorder [1].position.y) >= 0) {
//			print ("qua Bottom!");
//		}
//		if ((pointLimitCam [2].position.x - pointCamBorder [2].position.x) >= 0) {
//			print ("qua Left!");
//		} else if ((pointLimitCam [3].position.x - pointCamBorder [3].position.x) <= 0) {
//			print ("qua Right!");
//		}
		//Move the position of the camera to simulate a drag, speed * 10 for screen to worldspace conversion
		position = new Vector3 (Mathf.Clamp (position.x, (float)xMin, (float)xMax), Mathf.Clamp (position.y, (float)yMin, (float)yMax), -10);
		cameraMain.transform.position = position;
	}

	float worldScreenHeight, worldScreenWidth;
	public PinchZoom pinchZoom;

	public void _takeLimitCam ()
	{
		this.worldScreenHeight = Camera.main.orthographicSize * 2f;
		this.worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
//		Vector3 temp = cameraMain.transform.position;
//		pointCamBorder [0].position = temp + new Vector3 (0, worldScreenHeight / 2, 0);
//		pointCamBorder [1].position = temp + new Vector3 (0, -worldScreenHeight / 2, 0);
//		pointCamBorder [2].position = temp + new Vector3 (-worldScreenWidth / 2, 0, 0);
//		pointCamBorder [3].position = temp + new Vector3 (worldScreenWidth / 2, 0, 0);
		yMax = pointLimitCam [0].position.y - worldScreenHeight / 2;
		yMin = pointLimitCam [1].position.y - (-worldScreenHeight / 2);
		xMax = pointLimitCam [3].position.x - worldScreenWidth / 2;
		xMin = pointLimitCam [2].position.x - (-worldScreenWidth / 2);


		Vector3 posCam = Camera.main.transform.position;
		float x1 = Mathf.Abs (posCam.x - pointLimitCam [2].position.x);
		float x2 = Mathf.Abs (posCam.x - pointLimitCam [3].position.x);
		float xTemp = (x1 < x2) ? x1 : x2;
		float y1 = Mathf.Abs (posCam.y - pointLimitCam [0].position.y);
		float y2 = Mathf.Abs (posCam.y - pointLimitCam [1].position.y);
		float yTemp = (y1 < y2) ? y1 : y2;

		xTemp =((xTemp*2 / Screen.width) * Screen.height)/2;

		yTemp = yTemp;

		float temp = (xTemp < yTemp) ? xTemp : yTemp;

		pinchZoom.ortsMax = temp;

	}
}