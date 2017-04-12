using UnityEngine;
using System.Collections;

[System.Serializable]
public class InfoCheckPointEnd
{
	public Vector3 position;
	public bool overlapPoint;
	public bool canDraw;
}
public class CheckPointEnd : MonoBehaviour
{
	LayerMask maskPointRail;
	RaycastHit2D hit2d;
	InfoCheckPointEnd temp;
	GameObject tileTemp;
	bool dupPoint = false;
	Rigidbody2D rigiB;
	void Awake ()
	{
		this.maskPointRail = LayerMask.NameToLayer ("POINT_RAIL");
		temp = new InfoCheckPointEnd ();
	}

	public InfoCheckPointEnd PointEndFix ()
	{
		hit2d = Physics2D.Raycast (this.transform.position, Vector2.zero, 10, 1 << maskPointRail);
		if (hit2d != null && hit2d.collider != null) {
			if (hit2d.collider.tag == "PointRail") {
				this.temp.canDraw = true;
				this.temp.position = hit2d.transform.position;
				this.temp.overlapPoint = false;

				this.tileTemp = hit2d.collider.gameObject;
				this.dupPoint = false;
				//	print (pointEndRail);
			}
			if (hit2d.collider.tag == "PointRailMain") {
				this.temp.canDraw = true;
				this.temp.position = hit2d.transform.position;
				this.temp.overlapPoint = true;


				this.tileTemp = hit2d.collider.gameObject;
				this.dupPoint = true;
				this.rigiB = hit2d.transform.GetComponent<Rigidbody2D> ();
				//	print (pointEndRail);
			}
			if (hit2d.collider.tag == "PointRailTemp") {
				this.temp.canDraw = true;
				this.temp.position = hit2d.transform.position;
				this.temp.overlapPoint = true;


				this.tileTemp = hit2d.collider.gameObject;
				this.dupPoint = true;
				this.rigiB = hit2d.transform.GetComponent<Rigidbody2D> ();
				//	print (pointEndRail);
			}
		} else {
			this.temp.position = Vector2.zero;
			this.temp.canDraw = false;
			this.temp.overlapPoint = false;

		}
		RailDrawManager.Manager.TilePointCur = this.tileTemp;
		RailDrawManager.Manager.DupPoint = this.dupPoint;
		if (rigiB) {
			RailDrawManager.Manager.rigidTempB = rigiB;
		}
		rigiB = null;
		return this.temp;
	}
}
