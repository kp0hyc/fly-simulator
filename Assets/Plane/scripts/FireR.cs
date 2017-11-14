using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireR : MonoBehaviour {
	public GameObject[] rocket = new GameObject[6];
	public float RocketThrust = 0;
	float Timer = 1;
	int iterator = 0;
	public float  rangeOfExplosion = 10;

	public Transform target;
	public Rigidbody enemy;
	// скорость ракеты
	public float speed = 20;
	// скорость поворота ракеты
	public float turnSpeed = 20;

	Transform thisTransform;

	public Camera cam;
	void Start () {
	}

	IEnumerator TestCoroutine(int _iterator,Rigidbody _enemy, Transform _target, Transform _thisTransform)
	{
		while(_thisTransform)
		{
			yield return null;

			Vector3 movement = _thisTransform.forward * speed * Time.deltaTime;

			Vector3 direction = _target.position - _thisTransform.position;

			float maxAngle = turnSpeed * Time.deltaTime;

			float angle = Vector3.Angle (_thisTransform.forward, direction);

			if (angle <= maxAngle) 
			{
				_thisTransform.forward = direction.normalized;
			} else 
			{
				//сферическая интерполяция направления с использованием максимального угла поворота
				_thisTransform.forward = Vector3.Slerp (_thisTransform.forward, direction.normalized, maxAngle / angle);
			}
			float distanceToTarget = direction.magnitude;

			if (distanceToTarget < rangeOfExplosion)
			{
				Destroy (_enemy.gameObject, 0);
				Destroy (rocket[_iterator],0);
			}
			_thisTransform.Translate (movement);
		}
	}

	void FixedUpdate () 
	{
		float fireR = Input.GetAxis ("Fire1");

		RaycastHit hit;
		Ray ray = cam.ScreenPointToRay (new Vector3(cam.pixelWidth/2,cam.pixelHeight*2/3,0));
		if (Physics.Raycast (ray, out hit, 1000))
		if (hit.collider.tag == "planeBody") 
		{
			//Debug.Log (hit.collider);
			target = hit.transform;
			enemy = hit.rigidbody;
		}
		Debug.DrawRay (ray.origin, ray.direction * 1000, Color.yellow);


		if (fireR != 0 && iterator < 6 && Timer >= 1) 
		{
			if (target != null) 
			{
				thisTransform = rocket [iterator].transform;
				Destroy (rocket [iterator], 100);
				rocket [iterator].transform.parent = null;
				StartCoroutine(TestCoroutine(iterator, enemy, target, thisTransform));


				iterator++;
				Timer = 0;
			}
		}
		Timer += Time.deltaTime;

	}
}
