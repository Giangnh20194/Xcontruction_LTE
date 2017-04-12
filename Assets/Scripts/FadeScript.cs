using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
	//public static FadeScript Instance;
	public GameObject blockClick;
	public Image img;
	float timer = 0, timerII = 0;
	float timeMax = 0;
	bool isLoadIn = false;
	bool isLoadOut = false;
	bool isPlay = false;
	public Color32 colorNormal;
	Color32 colorTemp;

	void Awake ()
	{
	//	Instance = this;
		this.colorTemp = this.colorNormal;
		this.offblock ();
	}
	void Start ()
	{
		
	}
	public void LoadIn (float duration)
	{
		print ("vc");
		this.isLoadIn = true;
		this.isLoadOut = false;
		this.isPlay = true;
		this.setAlpha (255);
		this.timeMax = duration;
		this.blockClick.SetActive (true);
		timer = 0;
	}

	public void LoadOut (float duration)
	{
		this.isLoadIn = false;
		this.isLoadOut = true;
		this.isPlay = false;
		this.setAlpha (0);
		this.timeMax = duration;
		this.blockClick.SetActive (true);
		timerII = 0;
	}
	public void Update ()
	{
		if (isLoadIn) {
			timer += Time.unscaledDeltaTime;
			if (timer < timeMax) {
				float rate = (timeMax - timer) / timeMax;
				this.setAlpha ((byte)(rate * 255));
			} else {
				isLoadIn = false;
				timer = 0;
				this.offblock ();
			}
			if (isLoadOut) {
				isLoadIn = false;
			}
		} else if (isLoadOut) {
			timerII += Time.unscaledDeltaTime;
			if (timerII < timeMax) {
				float rate = timerII / timeMax;
				this.setAlpha ((byte)(rate * 255));
			} else {
				isLoadOut = false;
				timerII = 0;
				this.offblock ();
			}
			if (isLoadIn) {
				isLoadOut = false;
			}
		}
//		if (Input.GetKeyDown (KeyCode.P)) {
//			this.LoadIn (1);
//		}
//		if (Input.GetKeyDown (KeyCode.L)) {
//			this.LoadOut (1);
//		}
	}

	public void setAlpha (byte value)
	{
		colorTemp.a = value;
		this.img.color = this.colorTemp;
	}

	public void offblock ()
	{
		this.blockClick.SetActive (false);
	}
}
