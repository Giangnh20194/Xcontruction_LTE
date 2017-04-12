using UnityEngine;
using System.Collections;

public class PinchZoom : MonoBehaviour
{
	public float perspectiveZoomSpeed = 0.5f;
	// The rate of change of the field of view in perspective mode.
	public float orthoZoomSpeed = 0.5f;
	// The rate of change of the orthographic size in orthographic mode.
	Camera camera;
	public float ortsMax, ortsMin;
	public TouchControl touchControl;
	bool zooming = false;

	void Start ()
	{
		this.camera = Camera.main;
	}

	void Update ()
	{
		// If there are two touches on the device...
		if (Input.touchCount == 2) {
			if (!zooming) {
				touchControl.blockDrag = true;
				zooming = true;
			}
			// Store both touches.
			Touch touchZero = Input.GetTouch (0);
			Touch touchOne = Input.GetTouch (1);

			// Find the position in the previous frame of each touch.
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			// Find the magnitude of the vector (the distance) between the touches in each frame.
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

			// Find the difference in the distances between each frame.
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

			// If the camera is orthographic...
			if (this.camera.orthographic) {
				// ... change the orthographic size based on the change in distance between the touches.
				this.camera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

				// Make sure the orthographic size never drops below zero.
				this.camera.orthographicSize = Mathf.Clamp (camera.orthographicSize, ortsMin, ortsMax);
				touchControl._takeLimitCam ();
				//		touchControl._Dragcamera ();
			} else {
				// Otherwise change the field of view based on the change in distance between the touches.
				this.camera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

				// Clamp the field of view to make sure it's between 0 and 180.
				this.camera.fieldOfView = Mathf.Clamp (camera.fieldOfView, 0.1f, 179.9f);
			}
		} else {
			if (zooming) {
				StartCoroutine (delay_endZoom ());
				zooming = false;
			}
		}
	}
	IEnumerator delay_endZoom(){
		yield return new WaitForSecondsRealtime (2f);
		if (!zooming) {
			touchControl.blockDrag = false;
		}
	}

}
