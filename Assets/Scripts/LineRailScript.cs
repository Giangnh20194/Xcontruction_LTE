using UnityEngine;
using System.Collections;

public class LineRailScript : MonoBehaviour
{
	public CheckPointEnd[] checkPointEnd = new CheckPointEnd[8];
	public LineRailUI lineRailUI;

	public void setPivot (Vector3 pivot)
	{
		this.transform.position = pivot;
	}
	InfoCheckPointEnd infoTemp;
	int indexColor;
	public void SetInfo (Vector3 pivot, Vector3 pointEnd)
	{
		
		int index = 0;
		Vector3 direction = pointEnd - pivot;
		direction.Normalize ();
		this.transform.rotation = Quaternion.AngleAxis (
			Mathf.Atan2 (direction.y, direction.x) * 180 / Mathf.PI,
			new Vector3 (0, 0, 1));
		float distance = Vector3.Distance (pivot, pointEnd);
		if (distance <= 9) {
			index = Mathf.RoundToInt (distance);
//		this.lineRail.SetPosition (0, pivot);
		} else {
			index = 9;
		}
		if (index < 2) {
			index = 2;
		}
		for (int i = 0; i < 8; i++) {
			if (i == (index - 2)) {
				checkPointEnd [index - 2].gameObject.SetActive (true);
				infoTemp = checkPointEnd [index - 2].PointEndFix ();
				if (infoTemp.canDraw) {
					pointEnd = infoTemp.position;
				}
			} else {
				checkPointEnd [i].gameObject.SetActive (false);
			}
		}
		if (infoTemp.canDraw) {
			indexColor = (infoTemp.overlapPoint) ? 0 : 1;
		} else {
			indexColor = 2;
		}
		RailDrawManager.Manager.canDraw = infoTemp.canDraw;
		lineRailUI.drawLine (pivot, pointEnd,this.indexColor);
	}
}
