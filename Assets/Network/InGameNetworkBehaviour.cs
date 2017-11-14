using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InGameNetworkBehaviour : NetworkBehaviour {

	NetworkManager networkManager;
	NetworkInfo networkInfo;
	Dropdown dropdown;

	public SyncListString nicknames = new SyncListString ();

	public List<int> connections = null;

	public void OnNickGet (NetworkMessage nickMsg)
	{
		nicknames.Add (nickMsg.ReadMessage<UnityEngine.Networking.NetworkSystem.StringMessage> ().value);
		connections.Add (nickMsg.conn.connectionId);
	}

	[Command]
	void CmdApplyName(string n)
	{
		Debug.Log (n);
	}

	void OnNicknamesChanged(SyncListString.Operation op, int idx)
	{
		List<string> l = new List<string> ();
		foreach (string name in nicknames) 
		{
			//Debug.Log ("list elemnts: " + name);
			l.Add (name);
		}
		dropdown.ClearOptions ();
		dropdown.AddOptions (l);
	}

	public override void OnStartServer()
	{
		base.OnStartServer ();
		Debug.Log ("msg handled");
		NetworkServer.RegisterHandler (NetworkInfo.MsgNick, OnNickGet);
		connections = new List<int> ();
	}

	public override void OnStartClient()
	{
		//Debug.Log ("Start called in Ingame");
		base.OnStartClient();

		nicknames.Callback = OnNicknamesChanged;

		GameObject[] goArr = GameObject.FindGameObjectsWithTag ("NetworkObject"); 
		foreach (GameObject go in goArr)
		{
			if (go.name == "NetworkManagerObject") 
			{ 
				networkManager = go.GetComponent<NetworkManager> ();
				networkInfo = go.GetComponent<NetworkInfo> ();
				break;
			} 
		}

		goArr = GameObject.FindGameObjectsWithTag ("UserIngameUI"); 
		foreach (GameObject go in goArr) 
			if (go.name == "NicknamesDropDown") 
			{ 
				dropdown = go.GetComponent<Dropdown> (); 
				break; 
			}
	}

	void OnDisconnectedFromServer(NetworkDisconnection info) {
		if (Network.isServer)
			Debug.Log("Local server connection disconnected");
		else
			if (info == NetworkDisconnection.LostConnection)
				Debug.Log("Lost connection to the server");
			else
				Debug.Log("Successfully diconnected from the server");
	}

	public void TryDisconnect()
	{
		//Debug.Log ("Hello");
		if (isServer) 
		{
			Debug.Log ("Server disconnect");
			networkManager.StopHost ();
			return;
		}
		Debug.Log ("client disconnect");
		//Network.Disconnect(500);
		//MasterServer.UnregisterHost();
		networkManager.StopClient ();
		/*switch (networkInfo.CurStatus) 
		{
		case (int)NetworkInfo.Status.Host:
			networkManager.StopHost ();
			break;
		case (int)NetworkInfo.Status.Client:
			networkManager.StopClient ();
			break;
		case (int)NetworkInfo.Status.NetworkHost:
			networkManager.StopHost ();
			break;
		case (int)NetworkInfo.Status.NetworkClient:
			networkManager.StopClient ();
			break;
		}*/
	}
}
