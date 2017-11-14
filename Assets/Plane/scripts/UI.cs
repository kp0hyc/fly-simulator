using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public Text text;
    public Moving mov;
	public ParamsScript paramsScript;

	// Use this for initialization
	void Start () {

		GameObject[] goArr = GameObject.FindGameObjectsWithTag ("PlaneUI");
		foreach (GameObject go in goArr)
			if (go.name == "SpeedText") 
			{
				//Debug.Log ("found");
				text = go.GetComponent<Text> ();
				break;
			}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		text.text = "Thrust: " + (mov.thrust/1000).ToString("###") + 
			"KH\nSpeed: " + mov.speed.ToString("####") + 
			"Km/h\nHeight: " + mov.height.ToString("#####") + 
			"m\nHP: " + paramsScript.hp.ToString("##");

	}
}
