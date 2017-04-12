using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	public ButtonLevel[] btnsLevel;
	void Awake(){
		for (int i = 0; i < btnsLevel.Length; i++) {
			btnsLevel [i]._SetInfoLevel (i + 1);
		}
	}
}
