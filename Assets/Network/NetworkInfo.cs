using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkInfo : MonoBehaviour {
	public enum Status { Offline, Host, Client, NetworkHost, NetworkClient };
	public int CurStatus = (int) Status.Offline;
	public const short MsgNick = UnityEngine.Networking.MsgType.Highest + 1;
	public const short MsgAskPing = UnityEngine.Networking.MsgType.Highest + 2;
	public string nick;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
