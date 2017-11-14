using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MainNetworkManager : NetworkManager 
{

	InGameNetworkBehaviour inGameNetworkBehaviour;
	public NetworkInfo networkInfo;

	public override void OnStartHost ()
	{
		base.OnStartHost();
		networkInfo.CurStatus = (int)NetworkInfo.Status.Host;
	}

	public override void OnStartClient (NetworkClient client)
	{
		base.OnStartClient(client);
		//Debug.Log ("OnStartClient");

	}

	public override void OnClientError (NetworkConnection conn, int errorCode)
	{
		base.OnClientError(conn, errorCode);
		Debug.Log ("OnClientError");

	}

	public override void OnServerError (NetworkConnection conn, int errorCode)
	{
		base.OnServerError(conn, errorCode);
		Debug.Log ("OnServerError");

	}

	public override void OnClientDisconnect (NetworkConnection conn)
	{
		Debug.Log ("OnClientDisconnect");
		base.OnClientDisconnect (conn);
	}

	public override void OnServerDisconnect (NetworkConnection conn)
	{
		Debug.Log (conn.connectionId);
        inGameNetworkBehaviour.nicknames.RemoveAt(inGameNetworkBehaviour.connections.IndexOf(conn.connectionId));
        inGameNetworkBehaviour.connections.Remove(conn.connectionId);
		Debug.Log ("OnServerDisconnect");
		base.OnServerDisconnect (conn);
	}




	public override void OnClientConnect (NetworkConnection conn)
	{
		//Debug.Log (conn.address);
		//Debug.Log (conn.connectionId);
		//here is we are sending our message
		Debug.Log ("OnClientConnect");
		base.OnClientConnect (conn);
	}

	public override void OnClientSceneChanged (NetworkConnection conn)
	{
		//Debug.Log ("OnClientScene");
		base.OnClientSceneChanged (conn);
		if (networkInfo.CurStatus != (int)NetworkInfo.Status.Host)
			networkInfo.CurStatus = (int)NetworkInfo.Status.Client;
		GameObject[] goArr = GameObject.FindGameObjectsWithTag ("NetworkObject");
		foreach (GameObject go in goArr)
			if (go.name == "NetworkLogic") 
			{
				inGameNetworkBehaviour = go.GetComponent<InGameNetworkBehaviour> ();
				break;
			}
		//inGameNetworkBehaviour.applyName (networkInfo.name);
	}

}
