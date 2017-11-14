using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autopilot : MonoBehaviour {

	public Rigidbody rb;
	public float startSpeed = 100;
	public float startSpeedUp = 100000;

	public Moving mov;

	void Start () {		
	}
	void FixedUpdate () {
		
		//transform.Translate (Vector3.forward * startSpeed * Time.deltaTime);
	}
	void OnBecameVisible() {
		
	}

}
