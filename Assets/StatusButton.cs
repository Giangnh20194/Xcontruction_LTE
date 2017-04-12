using UnityEngine;
using System.Collections;

public class StatusButton : MonoBehaviour {
	public GameObject statusOn,statusOff;
	public void Status(bool status){
		this.statusOn.SetActive (status);
		this.statusOff.SetActive (!status);
	}
}
