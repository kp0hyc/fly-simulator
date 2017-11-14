using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlsScript : MonoBehaviour {

	public Moving mov;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float accelerate = Input.GetAxis("accelerate"); // +/-
		float bank = Input.GetAxis("Horizontal");	
		float pitch = Input.GetAxis("Vertical");
		float yaw = Input.GetAxis ("Yaw");
		mov.accelerate = accelerate;
		mov.bank = bank;
		mov.pitch = pitch;
		mov.yaw = yaw;
	}
}
