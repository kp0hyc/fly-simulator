using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collision : MonoBehaviour {
    //UIScript textUI;
	void Start () {
		//Debug.Log ("i am here");
	}
	
	void Update () {

    }

    void OnCollisionEnter(Collision col)
    {
		if (col.gameObject.tag != "wheels" && col.gameObject.tag != "planeBody") {
			transform.GetComponentInParent<ParamsScript>().hp -= 3 * Time.deltaTime;

		}
    }
}
