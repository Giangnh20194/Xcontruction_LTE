using UnityEngine;
using System.Collections;

public class PointRailTempScript : MonoBehaviour
{
	public Rigidbody2D rg2d;
	GameObject[] tilePoint = new GameObject[9];
	public Vector3 pos;
	public CircleCollider2D circleCollider2D;
	LayerMask mask;

	void Awake ()
	{
		this.mask = LayerMask.NameToLayer ("POINT_RAIL");
	}

	public void _Start ()
	{
		this.transform.position = pos;
		rg2d.isKinematic = true;
		_RaycastAll ();
	}
	public void TiltePointStatus (GameObject go, bool state)
	{
		/// <summary>
		/// Update this instance.
		/// </summary>
	}

	void Update ()
	{
		if (BadLogic.playing) {
			if (rg2d.isKinematic) {
				rg2d.isKinematic = false;
			}
		}
	}

	public void destroyThisGo ()
	{
		//	this.tilePoint.SetActive (true);

		this._TiltePointStatus (true);
		Destroy (this.gameObject);
	}

	int count = 0;

	public void _RaycastAll ()
	{
		count = 0;
		RaycastHit2D[] hits = Physics2D.CircleCastAll (this.transform.position, circleCollider2D.radius, Vector2.zero, 0.1f, 1 << mask);
		foreach (RaycastHit2D hit in hits) {
			if (hit.collider.tag == "PointRail") {
				tilePoint [count] = hit.collider.gameObject;
				count += 1;
				print (count);
			}
		}
		this._TiltePointStatus (false);
	}
	public void _TiltePointStatus (bool status)
	{
		foreach (GameObject go in tilePoint) {
			if (go != null) {
				go.SetActive (status);
			}
		}
	}
}
