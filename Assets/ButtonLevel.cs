using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ButtonLevel : MonoBehaviour {
	public int keyLevel = 0;
	public Button btn;
	public Text txtInfoLevel;
	// Use this for initialization
	public void _SetInfoLevel(int key){
		this.keyLevel = key;
		txtInfoLevel.text = "Level " + key;
	}
	void Start () {
		btn.onClick.AddListener (delegate {
			ClickLoadLevel ();
		});
	}
	// Update is called once per frame
	public void ClickLoadLevel(){
		BadLogic.CurrentLevel = keyLevel;
		BadLogic.playing = false;
		SceneManager.LoadScene ("Level" + keyLevel);
	}
}
