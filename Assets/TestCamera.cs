using UnityEngine;
using System.Collections;

public class TestCamera : MonoBehaviour {
	private Vector3 screenPoint;
	private Vector3 offset;
	void OnMouseDown(){
		print ("cc!");
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}
	void OnMouseDrag(){
		Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 cursorPosition = this.transform.position + offset;
		cursorPosition = new Vector3 (cursorPosition.x, cursorPosition.y, -10);
		Camera.main.transform.position = cursorPosition;
	}
}
