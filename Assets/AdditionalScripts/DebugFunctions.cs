using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugFunctions : MonoBehaviour 
{
	public GameObject PlanePrefab;

	public void OnClick()
	{
		GameObject go = GameObject.FindGameObjectWithTag ("LocalPlayer");
		GameObject plane = (GameObject) Instantiate (PlanePrefab, go.transform.localPosition + new Vector3 (20, 0, 0), go.transform.rotation);
		Moving mov1 = go.GetComponent<Moving> ();
		Moving mov2 = plane.GetComponent<Moving> ();
		//mov2.cmdChangeSpeed (mov1.SyncSpeed);
		//mov2.thrust = mov1.thrust;
	}
}
