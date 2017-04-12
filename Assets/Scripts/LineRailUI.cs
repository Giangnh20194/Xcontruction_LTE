using UnityEngine;
using System.Collections;

public class LineRailUI : MonoBehaviour
{
	public LineRenderer lineRail, lineOutLine;
	public RailDrawManager RailManager;
	public Color[] colorRail = new Color[3];
	public SpriteRenderer spr, sprCircle;

	void Start ()
	{
		lineRail.sortingLayerName = "BG";
		lineRail.sortingOrder = 22;
		lineOutLine.sortingLayerName = "BG";
		lineOutLine.sortingOrder = 21;
	}

	public void drawLine (Vector3 pivot, Vector3 pointEnd, int indexColor)
	{
		int index = 0;
		float valueSS;
		this.transform.position = pivot;
		Vector3 direction = pointEnd - pivot;
		direction.Normalize ();
		this.transform.rotation = Quaternion.AngleAxis (
			Mathf.Atan2 (direction.y, direction.x) * 180 / Mathf.PI,
			new Vector3 (0, 0, 1));
		float distance = Vector3.Distance (pivot, pointEnd);
		if (distance > 11) {
			distance = 11;
		}
		Vector3 posEnd;
		posEnd = new Vector3 (0, distance, 0);
	
		valueSS = (indexColor != 2) ? 2 : 2.5f;
		if (distance >= valueSS) {
				if (!this.lineRail.enabled) {
					this.lineRail.enabled = true;
				}
				if (!this.lineOutLine.enabled) {
					this.lineOutLine.enabled = true;
				}
				this.lineRail.SetPosition (1, posEnd);
				this.lineRail.SetColors (this.colorRail [indexColor], this.colorRail [indexColor]);
				this.lineOutLine.SetPosition (1, posEnd);
		} else {
			this.lineRail.enabled = false;
			this.lineOutLine.enabled = false;
		}
		this.spr.transform.position = pointEnd;
		spr.color = this.colorRail [indexColor];
		sprCircle.color = this.colorRail [indexColor];
		index = Mathf.RoundToInt (distance - 0.25f);
		if (!this.gameObject.activeInHierarchy) {
			this.gameObject.SetActive (true);
		}
		RailManager.SetInfoDrawRail (index, pivot, this.transform.rotation, pointEnd);
	}
}
