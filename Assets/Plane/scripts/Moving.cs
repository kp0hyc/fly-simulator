using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Moving : NetworkBehaviour
{
	public Rigidbody rb;
	public Transform tr;
    public float speed = 0;
	[SyncVar]
	public float thrust = 10800;
	[SyncVar]
	public Quaternion CurRot;
	[SyncVar]
	public Vector3 CurPos;
	[SyncVar]
	public Vector3 SyncSpeed;
    public float torqueBank = 0;
    public float torquePitch = 0;
	public float torqueYaw = 0;
	public float height = 0;
	float Y = 0;
	float S = 38;
	public float maxThrust = 108000;
	public float accelerate = 0;
	public float pitch = 0;
	public float bank = 0;
	public float yaw = 0;

	[SyncVar]
	float SyncDifTime;

	float Offset;

	public void GetPing (NetworkMessage msg)
	{
		Offset -= Time.time;
	}

    void Start()
    {
		if (isLocalPlayer) 
		{
			NetworkServer.RegisterHandler (NetworkInfo.MsgAskPing, GetPing);
			//CmdSendTime ((float) System.DateTime.UtcNow.TimeOfDay.TotalSeconds);
			Offset = Time.time;
			connectionToServer.Send (NetworkInfo.MsgAskPing, new EmptyMessage ());
		}
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
		//thrust = 21000;
		//rb.velocity = new Vector3 (0, 0, 50);
		//Debug.Log(tr.name);
    }

	public void ResendPing (NetworkMessage msg)
	{
		nickMsg.conn.Send (NetworkInfo.MsgAskPing, new EmptyMessage ());
	}

	public override void OnStartServer ()
	{
		base.OnStartServer ();
		NetworkServer.RegisterHandler (NetworkInfo.MsgAskPing, ResendPing);
	}

	public override void OnStartClient ()
	{
		base.OnStartClient ();
		Debug.Log ("Client ip: " + connectionToClient.address.ToString());
	}

    void FixedUpdate()
    {
		//AuthorityMove ();

		/*if (!isLocalPlayer) 
		{ 
			Debug.Log ("Cur time:" + Time.time.ToString());
			Debug.Log ("my pos: " + transform.position.ToString()); 
			Debug.Log ("need pos: " + CurPos.ToString()); 
			Debug.Log ("my rot: " + transform.eulerAngles.ToString()); 
			Debug.Log ("need rot: " + CurRot.eulerAngles.ToString()); 
		}*/
		if (isLocalPlayer)
			AuthorityMove ();
		else
			FixMove ();

   }

	void FixMove()
	{
		Vector3 direction = CurPos + SyncSpeed * (((float) System.DateTime.UtcNow.TimeOfDay.TotalSeconds) - SyncDifTime + 0.02f) ;
		float range = direction.magnitude;
		//float deltaTime = range / SyncSpeed.magnitude;
		tr.position = Vector3.Lerp (tr.position, direction, 0.02f);
		tr.rotation = Quaternion.Slerp (tr.rotation, CurRot, 0.02f);
		//tr.position = Vector3.Lerp (tr.position, CurPos, deltaTime/1000);
		//Debug.Log ("Cur time:" + Time.time.ToString());
		/*Debug.Log ("Sync cur time:" + SyncDifTime.ToString());
		Debug.Log ("Cur time:" + ((float) System.DateTime.UtcNow.TimeOfDay.TotalSeconds).ToString());
		Debug.Log ("my pos: " + transform.position.ToString()); 
		Debug.Log ("need pos: " + CurPos.ToString()); */
		//Debug.Log ("CUrSpeed: " + SyncSpeed.ToString());
		//Debug.Log ("my rot: " + transform.eulerAngles.ToString()); 
		//Debug.Log ("need rot: " + CurRot.eulerAngles.ToString()); 
	}

	void AuthorityMove()
	{
		Vector3 eulerAngles = transform.rotation.eulerAngles;

		Vector3 locVel = tr.InverseTransformDirection(rb.velocity);
		speed = locVel.z;
		height = rb.position.y;

		if (accelerate != 0) 
			thrust += accelerate * maxThrust/20 * Time.deltaTime;
		if (thrust < 0)				thrust = 0;
		if (thrust > maxThrust)		thrust = maxThrust - 1;

		Y = (1.162f - 0.00007f * height) * speed * speed * S/15 * (1 - pitch); //
		//Debug.Log (eulerAngles.x);

		rb.AddRelativeTorque(Vector3.forward * torqueBank * bank);
		rb.AddRelativeTorque(Vector3.right * torquePitch * pitch); 
		rb.AddRelativeTorque(Vector3.up * torqueYaw * yaw);

		//Debug.Log ("thrust: " + thrust.ToString());

		if(Y > 0)	
			rb.AddRelativeForce(Vector3.up * Y);
		if((1.162f - 0.00007f * height) > 0)	
			rb.AddRelativeForce(Vector3.forward * thrust * (1.162f - 0.00007f * height));

		if (isLocalPlayer) { 
			CmdChange(); 
/*SyncSpeed = rb.velocity; 
CurPos = transform.position; 
CurRot = transform.rotation;*/ 
} 
} 

	[ClientRpc]
	public void RpcSendTime(float time)
	{
		Debug.Log ("time got:" + time.ToString());
		Debug.Log ("time now:" + ((float) System.DateTime.UtcNow.TimeOfDay.TotalSeconds).ToString());
	}

	[Command] 
	public void CmdSendTime (float time)
	{
		RpcSendTime (time);
	}

[Command] 
	public void CmdChangeSpeed(Vector3 speed, Vector3 pos, Quaternion rot) 
{ 
//Debug.Log (gameObject.name); 
SyncSpeed = speed; 
CurPos = pos; 
CurRot = rot; 

} 

[Command] 
public void CmdChange() 
{ 
		SyncSpeed = rb.velocity; 
		CurPos = transform.position; 
		CurRot = transform.rotation; 
		SyncDifTime = (float) System.DateTime.UtcNow.TimeOfDay.TotalSeconds;
		//CmdChangeSpeed(rb.velocity, transform.position, transform.rotation.eulerAngles); 
}

}
