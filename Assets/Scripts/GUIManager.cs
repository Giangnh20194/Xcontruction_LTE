using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour
{
	public static GUIManager Instance;
	public FadeScript fadeMan;
	public bool modePreview = false;
	public StatusButton statusButtonBuild, statusButtonRemove;
	public TouchControl touchControl;
	public Text txtCount;
	bool isBuild;
	bool isRemove;
	public GameObject UIGPL, UIPreview;
	public Text txtDrawInfo;
	public int curLevel;
	void Awake(){
		Instance = this;
		Time.timeScale = 1;
	}
	private void Start ()
	{
		isBuild = true;
		isRemove = !isBuild;
		touchControl._BuildMode (isBuild);
		this.SetStatusButton ();
		this._ModeGPL ();
		curLevel = BadLogic.CurrentLevel;
//		fadeMan.LoadIn (1f);
	}
	public void SetTextCount (string text)
	{
		txtCount.text = text;
	}
	public void _clickButtonBuild ()
	{
		this._click (true);
	
	}
	public void _clickButtonRemove ()
	{
		this._click (false);
	}
	public void SetStatusButton ()
	{
		this.statusButtonBuild.Status (isBuild);
		this.statusButtonRemove.Status (!isBuild);
	}

	private void _click (bool statusBuild)
	{
		this.isBuild = statusBuild;
		this.isRemove = !isBuild;
		touchControl._BuildMode (this.isBuild);
		this.SetStatusButton ();
	}

	public void _ModePreView ()
	{
		this.modePreview = true;
		this.UIGPL.SetActive (false);
		this.UIPreview.SetActive (true);
	}

	public void _ModeGPL ()
	{
		this.modePreview = false;
		this.UIGPL.SetActive (true);
		this.UIPreview.SetActive (false);
	}

	public void ClickReplay ()
	{
		fadeMan.LoadOut (0.5f);
		this.StartCoroutine (delay_ClickReplay ());
		// Comming Soon
	}

	IEnumerator delay_ClickReplay ()
	{
	//	RailDrawManager.Manager._StopTrain ();
		yield return new WaitForSecondsRealtime (0.4f);
		fadeMan.LoadIn (0.6f);
		RailDrawManager.Manager.ReplayScene (true);
	}
	public void ClickBackPreview ()
	{
		// Comming soon
		fadeMan.LoadOut (0.5f);
		this.StartCoroutine (delay_ClickBackPreview());
	}

	IEnumerator delay_ClickBackPreview ()
	{
	//	RailDrawManager.Manager._StopTrain ();
		yield return new WaitForSecondsRealtime (0.4f);
		fadeMan.LoadIn (0.6f);
		this._ModeGPL ();
		RailDrawManager.Manager.ReplayScene (false);
	}
	public void ClickUndo ()
	{
		RailDrawManager.Manager.clickUndoBuild ();
	}
	public void OnWin ()
	{
		this.LoadNextLevel ();
		Time.timeScale = 0;
	}
	public void setInfoDraw(string text){
		txtDrawInfo.text = "" + text;
	}
	public void ClickloadMenu(){
		fadeMan.LoadOut (0.5f);
		this.StartCoroutine (delay_clickLoadMenu ());
	}
	IEnumerator delay_clickLoadMenu(){
		yield return new WaitForSecondsRealtime (0.5f);
		SceneManager.LoadScene ("Menu");
	}
	public void LoadNextLevel(){
		fadeMan.LoadOut (0.5f);
		this.StartCoroutine (delay_LoadNextLevel ());
	}
	IEnumerator delay_LoadNextLevel(){
		yield return new WaitForSecondsRealtime (0.5f);
		int nextLevel = curLevel + 1;
		BadLogic.CurrentLevel = nextLevel;
		SceneManager.LoadScene ("Level"+nextLevel);
	}
}
