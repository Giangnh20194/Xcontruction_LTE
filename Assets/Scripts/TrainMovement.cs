using UnityEngine;
using System.Collections;
using CnControls;
public class TrainMovement : MonoBehaviour
{
	public Rigidbody2D rg2d;
	public float moveSpeed;
	bool move = false;
	public Transform checkGround;
	LayerMask maskGround;
	Vector3 posStart;
	TrainChil[] trainChils;
	bool grounded = false;
	void Start ()
	{
		this.posStart = transform.position;
		maskGround = LayerMask.NameToLayer ("RAILPHYSICS");
		BadLogic.playing = false;
		trainChils = this.transform.GetComponentsInChildren<TrainChil> ();
	}
	void Update ()
	{
		if (CnInputManager.GetButtonDown ("Jump")) {
			if (!GUIManager.Instance.modePreview) {
				move = true;
				BadLogic.playing = true;
				GUIManager.Instance._ModePreView ();
			}
		}
	}
	void FixedUpdate ()
	{
		if (move) {
			this.grounded = Physics2D.Linecast (this.transform.position, checkGround.position, 1 << maskGround);
			if (grounded) {
				rg2d.velocity = new Vector2 (moveSpeed, rg2d.velocity.y);
			}
		}
	}
	void OnTriggerEnter2D(Collider2D coll){
		if (coll.tag == "PointEndTrain") {
			GUIManager.Instance.OnWin ();
		} else if (coll.tag == "CheckGameOver") {
			if (move) {
				print("Stop!!!!!!!!!!!!");
				move = false;
			}
		}
	}
	public void _Start(bool moveStatus){
		rg2d.velocity = Vector2.zero;

		this.transform.position = posStart;
		this.transform.localRotation = Quaternion.Euler (Vector3.zero);
		BadLogic.playing = false;
		move = moveStatus;
		this.grounded = false;
		foreach (TrainChil temp in trainChils) {
			temp._resetTranform ();
		}
		Time.timeScale = 1;
	}
}
