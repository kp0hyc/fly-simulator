using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamsScript : MonoBehaviour {
	public float hp;
	// Use this for initialization
	void Start () {
		hp = 100;
	}
	
	// Update is called once per frame
	void Update () {
		if (hp <= 0)
			Debug.Log ("you dead");
			//Destroy (gameObject.transform.parent);
	}
}
