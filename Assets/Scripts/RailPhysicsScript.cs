using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RailPhysicsScript : MonoBehaviour
{
	public Color[] colorNormal = new Color[2];
	public Rigidbody2D rg2d;
	public FixedJoint2D fixjoint2D, fixJoint2D_B;
	private float curForce;
	private float breakForce;
	private float frequency;
	public SpriteRenderer spr;
	Color32 _color;
	bool breaked = false, playing = false;
	public int InstanceIDofOrigin;
	public bool isRailPhysics = false;
	public Vector2[] posOfAnchor = new Vector2[2];
	public LineRenderer lineRenderer, lineOutline;
	public PointRailTempScript pointRailofLineA, pointRailofLineB;
	int[] rg_InstanceID = new int[2];
	public Vector3 _positionStart;
	public Quaternion _quaternionStart;
	public int sttRail;
	public RailPhysicsScript railPhysicsBefore;
	Rigidbody2D rgA, rgB;
	LayerMask maskRailTemp;

	void Awake ()
	{
		maskRailTemp = LayerMask.NameToLayer ("RAILTEMP");
		lineRenderer.sortingLayerName = "BG";
		lineOutline.sortingLayerName = "BG";
		this.GetComponent<SpriteRenderer> ().enabled = false;
		this.breakForce = BadLogic.breakForce;
		this.frequency = BadLogic.frequency;
	}

	public void _LoadPointTemp ()
	{
		if (pointRailofLineB) {
			pointRailofLineB._Start ();
		}
	}

	public void _Start ()
	{
		this.transform.position = _positionStart;
		this.transform.rotation = _quaternionStart;

		if (this.fixjoint2D == null) {
			this.fixjoint2D = this.gameObject.AddComponent<FixedJoint2D> ();
			if (this.fixjoint2D.connectedBody == null) {
				if (rgA != null) {
					this.fixjoint2D.connectedBody = this.rgA;
				}
			}
		}
		if (this.fixJoint2D_B == null) {
			this.fixJoint2D_B = this.gameObject.AddComponent<FixedJoint2D> ();
			if (this.fixJoint2D_B.connectedBody == null) {
				if (rgB != null) {
					this.fixJoint2D_B.connectedBody = this.rgB;
				}
			}
		}


		this.fixjoint2D.frequency = this.frequency;
		this.fixjoint2D.breakForce = this.breakForce;


		this.fixJoint2D_B.frequency = this.frequency;
		this.fixJoint2D_B.breakForce = this.breakForce;



		_color = spr.color;
		_color.a = 0;
		breaked = false;
		playing = false;
		if (!isRailPhysics) {
			if (this.transform.rotation == Quaternion.Euler (Vector3.zero) || this.transform.rotation == Quaternion.Euler (new Vector3 (0, 0, 180))) {
				if (this.transform.position.y < 4.6f) {
					if (this.transform.position.y > 4.4f) {
						isRailPhysics = true;
					}
				}
			}
		} else {
			if (this.transform.rotation != Quaternion.Euler (Vector3.zero) || this.transform.rotation != Quaternion.Euler (new Vector3 (0, 0, 180))) {
				if (this.transform.position.y > 4.6 || this.transform.position.y < 4.4f) {
					isRailPhysics = false;
				}
			}
		}
		Vector3 posOftempA = this.transform.InverseTransformPoint (posOfAnchor [0]);
		Vector3 posOftempB = this.transform.InverseTransformPoint (posOfAnchor [1]);

		this.lineRenderer.SetPosition (0, new Vector3 (posOftempA.x, posOftempA.y, 0));
		this.lineRenderer.SetPosition (1, new Vector3 (posOftempB.x, posOftempB.y, 0));

		this.lineOutline.SetPosition (0, new Vector3 (posOftempA.x, posOftempA.y, 0));
		this.lineOutline.SetPosition (1, new Vector3 (posOftempB.x, posOftempB.y, 0));

		this.fixjoint2D.anchor = posOftempA;
		this.fixJoint2D_B.anchor = posOftempB;

		this.rg2d.isKinematic = true;

		if (isRailPhysics) {
			this.transform.localScale = new Vector3 (this.transform.localScale.x, 1.6f, 1);
			this.lineRenderer.SetColors (colorNormal [1], colorNormal [1]);
			this.lineRenderer.SetWidth (1.6f / 2.5f, 1.6f / 2.5f);
			this.lineRenderer.sortingOrder = 4;

			this.lineOutline.SetWidth (1.8f / 2.5f, 1.8f / 2.5f);
			this.lineOutline.sortingOrder = 3;

		} else {
			this.transform.localScale = new Vector3 (this.transform.localScale.x, 2.5f, 1);
			this.lineRenderer.SetColors (colorNormal [0], colorNormal [0]);
			this.lineRenderer.SetWidth (1.6f / 2.5f, 1.6f / 2.5f);
			lineRenderer.sortingOrder = 7;


			this.lineOutline.SetWidth (1.8f / 2.5f, 1.8f / 2.5f);
			this.lineOutline.sortingOrder = 6;

		}
		this.gameObject.layer = maskRailTemp;

		this._color.a = 0;
		this.spr.color = _color;
	}

	public void SetInfoFixedJoint (Rigidbody2D rg)
	{
		this.rgA = rg;
		fixjoint2D.connectedBody = rg;
		this.rg_InstanceID [0] = rg.GetInstanceID ();
		this.pointRailofLineA = rg.GetComponent<PointRailTempScript> ();
	}

	public void SetInfoFixedJoint_B (Rigidbody2D rg)
	{
		this.rgB = rg;
		fixJoint2D_B.connectedBody = rg;
		this.rg_InstanceID [1] = rg.GetInstanceID ();
		this.pointRailofLineB = rg.GetComponent<PointRailTempScript> ();
	}

	float reactionForce_A, reactionForce_B;

	public void _fixReplay(bool status){
		if (isRailPhysics) {
			//this.gameObject.layer = LayerMask.NameToLayer ("RAILPHYSICS");
			this._color.a = 0;
			this.spr.color = _color;
			breaked = false;
			playing = false;
			BadLogic.playing = status;
		}
	}

	void Update ()
	{
		if (!this.breaked) {
			//	if (CnInputManager.GetButtonDown ("Jump")) {
			if (BadLogic.playing) {
				if (!playing) {
					if (rg2d.isKinematic) {
						rg2d.isKinematic = false;
						if (isRailPhysics) {
							this.gameObject.layer = LayerMask.NameToLayer ("RAILPHYSICS");
						}
					}
					playing = true;
				}
			}
			if (isRailPhysics) {
				if (playing) {
					if (fixjoint2D) {
						this.reactionForce_A = (this.fixjoint2D.reactionForce.magnitude);
					} else {
						breaked = true;
					}
					if (fixJoint2D_B) {
						this.reactionForce_B = (this.fixJoint2D_B.reactionForce.magnitude);
					} else {
						breaked = true;
					}
					if (!breaked) {
						curForce = (this.reactionForce_A > this.reactionForce_B) ? this.reactionForce_A : this.reactionForce_B;
						this._color.a = (byte)((curForce / breakForce) * 255f);
						this.spr.color = _color;
					} else {
						this._color.a = 0;
						this.spr.color = _color;
					}
				} 
			}
			//	
		}
	}

	public void RemoveRailPhysics ()
	{
		if (this.gameObject.activeInHierarchy) {
			if (this.pointRailofLineA) {
				if (!RailDrawManager.Manager.checkRemovePointTemp (this.rg_InstanceID [0])) {
					this.pointRailofLineA.destroyThisGo ();
				}
			}
			if (this.pointRailofLineB) {
				if (!RailDrawManager.Manager.checkRemovePointTemp (this.rg_InstanceID [1])) {
					this.pointRailofLineB.destroyThisGo ();
				}
			}
			this.destroyThisGo ();
			RailDrawManager.Manager.RemoveRail ();
		}
	}

	public void UndoRemoveRailPhysics ()
	{
		if (!this.gameObject.activeInHierarchy) {
			this.gameObject.SetActive (true);
			RailDrawManager.Manager.countRail += 1;
			GUIManager.Instance.SetTextCount (RailDrawManager.Manager.countRail.ToString ());
		}
	}

	private void destroyThisGo ()
	{
		this.gameObject.SetActive (false);
	}

	public bool checkOnRemovePointTemp (int id)
	{
		bool temp = false;
		foreach (int i in rg_InstanceID) {
			if (i == id) {
				temp = true;
				break;
			}
		}
		return temp;
	}
}
