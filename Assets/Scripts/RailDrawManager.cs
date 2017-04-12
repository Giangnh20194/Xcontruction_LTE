using UnityEngine;
using System.Collections;


public class RailDrawManager : MonoBehaviour
{
	public TrainMovement trainMovement;
	public int maxRail;
	RailPhysicsScript[] Rails;
	public TouchControl touchControl;
	public static RailDrawManager Manager;
	public GameObject RailSprite;
	public GameObject PointRailTemp;
	private float _scaleX, _posX;
	private Vector3 _posEnd, _pos;
	private Quaternion _quaTemp;
	public GameObject TilePointCur;
	public bool DupPoint = false;
	public Rigidbody2D rigidTempB;
	public int instanceIDTemp;
	public int countRail;
	LayerMask maskRailTemp;
	public LineRailUI lineRailUI;
	int indexCurrent;
	int sttRail = 0;
	public bool canDraw = false;

	void Start ()
	{
		Manager = this;
		countRail = 0;
		sttRail = 0;
		maskRailTemp = LayerMask.NameToLayer ("RAILTEMP");
		Rails = new RailPhysicsScript[maxRail];
		GUIManager.Instance.setInfoDraw (this.maxRail.ToString());
	}

	public void SetInfoDrawRail (int index, Vector3 pos, Quaternion quaTemp, Vector3 posEnd)
	{

		float posX = ((float)index) * 0.5f;
		float scaleX = 2.5f * (index + 1);
		this.indexCurrent = index;
		this._scaleX = scaleX;
		this._posX = posX;
		this._posEnd = posEnd;
		this._pos = pos;
		this._quaTemp = quaTemp;
	}

	RailPhysicsScript railPhysicsTemp, railPhysicsBefore;
	PointRailTempScript pointRailTempsc;

	public void AddRail (RailPhysicsScript temp)
	{
		for (int i = 0; i < Rails.Length; i++) {
			if (Rails [i] == null) {
				Rails [i] = temp;
				break;
			}
		}
	}

	public void checkDraw ()
	{
		touchControl.isDraw = (countRail < maxRail) ? true : false;
		int temp = maxRail - countRail;
		GUIManager.Instance.setInfoDraw (temp.ToString ());
	}

	public void _DrawOption (bool status)
	{
		this.lineRailUI.gameObject.SetActive (status);
	}

	public void DrawRailPhysics ()
	{
		if (canDraw) {
			if (!this.check_OverlapPoint ()) {
				if (this.railPhysicsTemp) {
					this.railPhysicsBefore = this.railPhysicsTemp;
				}
				if (this.checkSpawnRail () == null) {
					this.transform.position = this._pos;
					this.transform.rotation = this._quaTemp;
					GameObject railTemp = Instantiate (this.RailSprite) as GameObject;
					railTemp.transform.parent = this.transform;

					railTemp.transform.localPosition = new Vector3 (_posX, 0, 0);
					railTemp.transform.localScale = new Vector3 (_scaleX, 2.5f, 0);
					railTemp.transform.rotation = this._quaTemp;
					railTemp.name += "" + countRail;

					this.railPhysicsTemp = railTemp.GetComponent<RailPhysicsScript> ();


					this.railPhysicsTemp._positionStart = this.railPhysicsTemp.transform.position;
					this.railPhysicsTemp._quaternionStart = this.railPhysicsTemp.transform.rotation;

					this.railPhysicsTemp.SetInfoFixedJoint (this.touchControl.rgCurrent);
					this.railPhysicsTemp.InstanceIDofOrigin = this.touchControl.rgCurrent.transform.GetInstanceID ();
					this.railPhysicsTemp.posOfAnchor [0] = _pos;
					this.railPhysicsTemp.posOfAnchor [1] = _posEnd;
					this.railPhysicsTemp.sttRail = sttRail;
					this.railPhysicsTemp._Start ();


					this.AddRail (this.railPhysicsTemp);

					if (this.DupPoint) {
						this.railPhysicsTemp.SetInfoFixedJoint_B (this.rigidTempB);
					}
					this.touchControl.rgCurrent = null;
					if (!this.DupPoint) {
						GameObject pointRailTemp = Instantiate (this.PointRailTemp) as GameObject;
						pointRailTemp.transform.parent = this.transform;
						this.pointRailTempsc = pointRailTemp.GetComponent<PointRailTempScript> ();
						this.pointRailTempsc.pos = _posEnd;
						this.pointRailTempsc._Start ();
						this.pointRailTempsc.TiltePointStatus (this.TilePointCur, false);
						pointRailTemp.transform.parent = null;

						this.railPhysicsTemp.SetInfoFixedJoint_B (pointRailTemp.GetComponent<Rigidbody2D> ());
					}

					railTemp.transform.parent = null;
					this.DupPoint = false;
					this.rigidTempB = null;
					this.countRail += 1;
					sttRail += 1;
					GUIManager.Instance.SetTextCount (this.countRail.ToString ());
					this.checkDraw ();
				} else {
				
					this.transform.position = this._pos;
					this.transform.rotation = this._quaTemp;

					this.railPhysicsTemp = this.checkSpawnRail ();


					this.railPhysicsTemp.transform.parent = this.transform;


					this.railPhysicsTemp.transform.localPosition = new Vector3 (_posX, 0, 0);
					this.railPhysicsTemp.transform.localScale = new Vector3 (_scaleX, 2.5f, 0);
					this.railPhysicsTemp.transform.rotation = this._quaTemp;





					this.railPhysicsTemp._positionStart = this.railPhysicsTemp.transform.position;
					this.railPhysicsTemp._quaternionStart = this.railPhysicsTemp.transform.rotation;

					this.railPhysicsTemp.SetInfoFixedJoint (this.touchControl.rgCurrent);

					this.railPhysicsTemp.InstanceIDofOrigin = this.touchControl.rgCurrent.transform.GetInstanceID ();

					this.railPhysicsTemp.posOfAnchor [0] = _pos;
					this.railPhysicsTemp.posOfAnchor [1] = _posEnd;

					this.railPhysicsTemp.sttRail = sttRail;
					this.railPhysicsTemp._Start ();

					this.railPhysicsTemp.transform.parent = null;

					if (this.DupPoint) {
						this.railPhysicsTemp.SetInfoFixedJoint_B (this.rigidTempB);
					}
					this.touchControl.rgCurrent = null;
					if (!this.DupPoint) {
						GameObject pointRailTemp = Instantiate (this.PointRailTemp) as GameObject;
						pointRailTemp.transform.parent = this.transform;
						this.pointRailTempsc = pointRailTemp.GetComponent<PointRailTempScript> ();
						this.pointRailTempsc.pos = _posEnd;
						this.pointRailTempsc._Start ();
						this.pointRailTempsc.TiltePointStatus (this.TilePointCur, false);
						pointRailTemp.transform.parent = null;
						this.railPhysicsTemp.SetInfoFixedJoint_B (pointRailTemp.GetComponent<Rigidbody2D> ());
					}

					this.DupPoint = false;
					this.rigidTempB = null;


					this.sttRail += 1;
					this.railPhysicsTemp.gameObject.SetActive (true);

					this.countRail += 1;
					GUIManager.Instance.SetTextCount (this.countRail.ToString ());
					this.checkDraw ();
				}


				this.railPhysicsTemp.railPhysicsBefore = this.railPhysicsBefore;

			}
			this._DrawOption (false);
		} else {
			this._DrawOption (false);
		}
	}

	public RailPhysicsScript checkSpawnRail ()
	{
		RailPhysicsScript temp = null;
		foreach (RailPhysicsScript railTemp in Rails) {
			if (railTemp != null) {
				if (!railTemp.gameObject.activeInHierarchy) {
					temp = railTemp;
					break;
				}
			}
		}
		return temp;
	}

	public bool check_OverlapPoint ()
	{
		bool temp = false;
		Vector2 direction = this._pos - this._posEnd;
		RaycastHit2D hit = Physics2D.Raycast (this._posEnd, -direction.normalized, _posX, 1 << maskRailTemp);
		//RaycastHit2D hit = Physics2D.Linecast (_posEnd,_pos, 1 << maskRailTemp);
		if (hit != null && hit.collider != null) {
			if (hit.collider.tag == "RailTemp") {
				if (hit.collider.GetComponent<RailPhysicsScript> ().InstanceIDofOrigin == this.touchControl.rgCurrent.transform.GetInstanceID ()) {
					temp = true;
					//print ("Xong nhe !!!!" + hit.collider.name);
				}
			}
		}
		return temp;
	}
	//
	//	public RailPhysicsScript ChooseRail_Undo(int){
	//		RailPhysicsScript temp;
	//		foreach (RailPhysicsScript railTemp in Rails) {
	//			if (railTemp != null) {
	//
	//			}
	//		}
	//	}

	public bool checkRemovePointTemp (int id)
	{
		bool temp = false;
		int count = 0;
		foreach (RailPhysicsScript railTemp in Rails) {
			if (railTemp != null) {
				if (railTemp.gameObject.activeInHierarchy) {
					if (railTemp.checkOnRemovePointTemp (id)) {
						count += 1;
					}
				}
			}
		}
		if (count > 1) {
			temp = true;
		}
		return temp;
	}

	public void RemoveRail ()
	{
		this.countRail -= 1;
		GUIManager.Instance.SetTextCount (this.countRail.ToString ());
		this.checkDraw ();
	}


	public void _StopTrain ()
	{
		//this.trainMovement.rg2d ;
	}

	public void ReplayScene (bool status)
	{
		this.trainMovement._Start (status);
		foreach (RailPhysicsScript railTemp in Rails) {
			if (railTemp != null) {
				if (railTemp.gameObject.activeInHierarchy) {
					railTemp._Start ();
					railTemp._LoadPointTemp ();
					railTemp._fixReplay (status);
				}
			}
		}

	}

	public void clickUndoBuild ()
	{
		if (touchControl.isBuild) {
			if (this.railPhysicsTemp) {
				this.railPhysicsTemp.RemoveRailPhysics ();
				this.railPhysicsTemp = this.railPhysicsTemp.railPhysicsBefore;
			}
		} else if (touchControl.isRemove) {
			if (this.railPhysicsTemp) {
				this.railPhysicsTemp.UndoRemoveRailPhysics ();
				this.railPhysicsTemp = this.railPhysicsTemp.railPhysicsBefore;
			}
		}
	}

	//	IEnumerator delay_DrawRailPhysics(ref GameObject rail,ref GameObject point){
	//		yield return new WaitForEndOfFrame ();
	//	}
}
