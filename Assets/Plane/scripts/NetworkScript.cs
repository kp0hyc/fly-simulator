using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkScript : NetworkBehaviour 
{
	/*class SyncListNickname : SyncList<string>
	{
		protected override void SerializeItem (NetworkWriter writer, string item)
		{
			writer.Write (item);
		}
		protected override string DeserializeItem (NetworkReader reader)
		{
			return reader.ReadString ();
		}
	}*/

	public GameObject cam;
	public UI uiscript;
	public controlsScript _controlsScript;
	public FixTransform fixTransform;
	public Rigidbody rb;

	[SyncVar]
	string nick;
	[SyncVar]
	public float Thrust;
	[SyncVar]
	public Vector3 Speed;

	NetworkInfo networkInfo;
	//Dropdown dropdown;
	InGameNetworkBehaviour inGameNetworkBehaviour;

	// Use this for initialization
	void Start () 
	{
		//Debug.Log (nick);
	}

	public override void OnStartClient()
	{
		//ebug.Log ("Start called");
		base.OnStartClient();

		GameObject[] goArr = GameObject.FindGameObjectsWithTag ("NetworkObject"); 
		bool found = false;
		foreach (GameObject go in goArr)
		{
			if (go.name == "NetworkManagerObject") 
			{ 
				networkInfo = go.GetComponent<NetworkInfo> (); 
				if (found)
					break;
				found = true;
			} 
			if (go.name == "NetworkLogic") 
			{ 
				inGameNetworkBehaviour = go.GetComponent<InGameNetworkBehaviour> (); 
				if (found)
					break;
				found = true;
			} 
		}

		/*goArr = GameObject.FindGameObjectsWithTag ("UserIngameUI"); 
		foreach (GameObject go in goArr) 
			if (go.name == "NicknamesDropDown") 
			{ 
				dropdown = go.GetComponent<Dropdown> (); 
				break; 
			}*/
	}

	[Command]
	void CmdApplyName (string n)
	{
		nick = n;
		inGameNetworkBehaviour.nicknames.Add (n);
		//Debug.Log ("command" + n);
	}

	/*[ClientRpc]
	void RpcNickNamesChanged()
	{
		Debug.Log ("Value changed");
		List<string> l = new List<string>();
		GameObject[] goArr = GameObject.FindGameObjectsWithTag ("Player"); 
		foreach (GameObject go in goArr)
		{
			//NetworkScript networkScript = go.GetComponent<NetworkScript>();
			l.Add (go.GetComponent<NetworkScript>().nick);
		}
		dropdown.ClearOptions ();
		dropdown.AddOptions (l);
	}*/

	public override void OnStartLocalPlayer()
	{
		//Debug.Log ("new loacl player");

		uiscript.enabled = true; 
		cam.SetActive(true); 
		_controlsScript.enabled = true;
		fixTransform.enabled = false;
		rb.isKinematic = false;

		tag = "LocalPlayer";

		CmdApplyName (networkInfo.nick);
	}
}
