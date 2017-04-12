using UnityEngine;
using System.Collections;

public class TrainChil : MonoBehaviour {
	public Vector3 posStart;
	Rigidbody2D rg2d;
	void Awake(){
		posStart = transform.position;
		rg2d = this.GetComponent<Rigidbody2D> ();
	}
	public void _resetTranform(){
		this.transform.position = posStart;
		this.transform.localRotation = Quaternion.Euler (Vector3.zero);
		this.rg2d.velocity = Vector2.zero;
	}
}
