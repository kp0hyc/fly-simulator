using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FixTransform : MonoBehaviour {

	public NetworkTransform networkTransformChild;

	void Start()
	{
		
		networkTransformChild.clientMoveCallback3D = func;
	}

	bool func (ref Vector3 a, ref Vector3 b, ref Quaternion c)
	{
		Debug.Log ("hello");
		return true;
	}
}
