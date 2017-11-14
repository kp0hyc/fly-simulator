using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections.Generic;
using UnityEngine.UI;
//using NetworkInfo.Status;

public class HostGame : MonoBehaviour
{
	List<MatchInfoSnapshot> matchList = new List<MatchInfoSnapshot>();
	NetworkManager networkManager;
	NetworkInfo networkInfo;
	//bool matchCreated;

	public Dropdown list;
	public Text IPAddress;
	public Text RoomName;
	public Text NickName;
	public Canvas onlineCanvas;
	public Canvas offlineCanvas;

	void Start()
	{
		GameObject[] goArr = GameObject.FindGameObjectsWithTag ("NetworkObject");
		foreach (GameObject go in goArr)
			if (go.name == "NetworkManagerObject") 
			{
				networkManager = go.GetComponent<NetworkManager> ();
				networkInfo = go.GetComponent<NetworkInfo> ();
				break;
			}
		list.onValueChanged.AddListener (delegate
			{
				OnMatchClicked();
			});
	}

	public void TryCreateMatch()
	{
		uint matchSize = 4;
		bool matchAdvertise = true;
		string matchPassword = "";

		NetworkManager.singleton.matchMaker.CreateMatch(RoomName.text, matchSize, matchAdvertise, matchPassword, "", "", 0, 0, OnMatchCreate);
	}

	public void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
	{
		if (success) 
		{
			PrepareBattle ();
			networkInfo.CurStatus = (int)NetworkInfo.Status.NetworkHost;
			NetworkManager.singleton.OnMatchCreate (success, extendedInfo, matchInfo);
		}
	}

	public void ShowRooms()
	{
		NetworkManager.singleton.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
	}

	/*public void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
	{
		if (success)
		{
			Debug.Log("Create match succeeded");
			//matchCreated = true;
			NetworkServer.Listen(matchInfo, 9000);
			Utility.SetAccessTokenForNetwork(matchInfo.networkId, matchInfo.accessToken);
		}
		else
		{
			Debug.LogError("Create match failed: " + extendedInfo);
		}
	}*/

	public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
	{
		if (success) 
		{
			matchList = matches;
		}
		if (success && matches != null && matches.Count > 0)
		{
			List<string> l = new List<string>();
			foreach (MatchInfoSnapshot mis in matches)
				l.Add (mis.name);
			list.ClearOptions ();
			list.AddOptions (l);
			//networkMatch.JoinMatch(matches[0].networkId, "", "", "", 0, 0, OnMatchJoined);
		}
		else if (!success)
		{
			Debug.LogError("List match failed: " + extendedInfo);
		}
	}

	public void PrepareBattle ()
	{
		if (NickName.text == "") 
		{
			networkInfo.nick = "default";
			return;
		}
		networkInfo.nick = NickName.text;
	}

	public void OnMatchClicked()
	{
		Debug.Log ("Clicked");
		NetworkManager.singleton.matchMaker.JoinMatch(matchList[list.value].networkId, "", "", "", 0, 0, OnMatchJoined);
	}

	public void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
	{
		if (success) 
		{
			PrepareBattle ();
			networkInfo.CurStatus = (int)NetworkInfo.Status.NetworkClient;
			NetworkManager.singleton.OnMatchJoined (success, extendedInfo, matchInfo);
		}
	}

	public void StartHost()
	{
		PrepareBattle ();
		//Debug.Log ("Trying to host");
		networkManager.StartHost ();
	}

	public void ConnectByIP()
	{
		PrepareBattle ();
		if (IPAddress.text == "")
			networkManager.networkAddress = "127.0.0.1";
		else
			networkManager.networkAddress = IPAddress.text;
		networkManager.StartClient ();
	}

	public void SwitchOnline(bool toOnline)
	{
		onlineCanvas.enabled = toOnline;
		offlineCanvas.enabled = !toOnline;
		if (toOnline) 
		{
			NetworkManager.singleton.StartMatchMaker ();
			return;
		}
		NetworkManager.singleton.StopMatchMaker ();
	}
}