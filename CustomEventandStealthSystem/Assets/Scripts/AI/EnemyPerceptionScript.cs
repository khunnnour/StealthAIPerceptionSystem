using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

//struct feeler
//{
//	private Ray ray;
//	private RaycastHit hit;
//	private Vector3 origDir;
//
//	public void setRay(Ray r) { ray = r; }
//	public Ray getRay() { return ray; }
//
//	public void setHit(RaycastHit h) { hit = h; }
//	public RaycastHit getHit() { return hit; }
//
//	public void setLocalDir(Vector3 v) { origDir = v; }
//	public Vector3 getLocalDir() { return origDir; }
//}

public class EnemyPerceptionScript : MonoBehaviour
{
	// potentially public
	private Vector2 boxDim;
	private Vector2 fov;
	private float viewDist = 15f;

	private Transform player;
	private float viewDistSqr, inFrontThresh;
	private bool inited = false;
	private bool inRange = false;
	//private int numFeelers;
	//private List<feeler> feelers;

	// Start is called before the first frame update
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;

		boxDim = new Vector2(3f, 2f);
		fov = new Vector2(60f, 33f);

		viewDistSqr = viewDist * viewDist;
		inFrontThresh = Mathf.Cos(Mathf.Deg2Rad * fov.x * 0.5f);

		//numFeelers = (int)(boxDim.x * boxDim.y);
		//
		//feelers = new List<feeler>();
		//
		//// Casting rays in a 16:9 ratio at a depth of z
		//Vector3 scrnPt = Vector3.zero;
		//float xDist = 16f / (boxDim.x - 1f);
		//float yDist = 9f / (boxDim.y - 1f);
		//
		//for (int i = 0; i < numFeelers; i++)
		//{
		//	int x = (int)(i % boxDim.x);
		//	int y = (int)(i / boxDim.x);
		//
		//	feeler temp = new feeler();
		//	temp.setLocalDir(new Vector3(x * xDist, y * yDist, viewDist));
		//	temp.setRay(new Ray(transform.position, transform.forward));
		//	temp.setHit(new RaycastHit());
		//
		//	Debug.Log(temp.getRay());
		//
		//	feelers.Add(temp);
		//}
	}

	public void Init(float dist)
	{
		if (!inited)
		{
			viewDist = dist;

			inited = true;
		}
	}

	public void UpdatePerception()
	{
		//UpdateFeelers();

		CheckPlayerProximity();

		if(inRange)
		{
			CheckIfSeen();
		}
	}

	private void CheckPlayerProximity()
	{
		Vector3 diff = player.position - transform.position;

		if (diff.sqrMagnitude <= viewDistSqr)
			inRange = true;
		else
			inRange = false;
	}

	private void CheckIfSeen()
	{
		Vector3 diff = player.position - transform.position;

		// check if in front of enemy
		if (Vector3.Dot(diff.normalized, transform.forward) > inFrontThresh)
		{
			// if in front then check if actually LOS
			RaycastHit hit;
			Physics.Raycast(transform.position, diff, out hit);

			// check if ray hit player or another object
			if (hit.collider)
			{
				if (hit.collider.CompareTag("Player"))
				{
					Debug.DrawRay(transform.position, diff * viewDist, Color.green);
					Debug.Log("Sees player");
				}
				else
				{
					Debug.DrawRay(transform.position, diff * viewDist, Color.red);
				}
			}
		}
	}

	//private void UpdateFeelers()
	//{
	//	Vector3 rayD = Vector3.zero;
	//	float cosX,cosY;
	//	float sinX,sinY;
	//	int x;
	//	int y;
	//
	//	float xAng = fov.x / (boxDim.x - 1f);
	//	float yAng = fov.y / (boxDim.y - 1f);
	//	for (int i = 0; i < feelers.Count; i++)
	//	{
	//		x = (int)(i % boxDim.x);
	//		y = (int)(i / boxDim.x);
	//
	//		cosX = Mathf.Cos(Mathf.Deg2Rad*(transform.rotation.eulerAngles.x -(fov.x*0.5f)+ xAng * x));
	//		sinX = Mathf.Sin(Mathf.Deg2Rad*(transform.rotation.eulerAngles.x -(fov.x*0.5f)+ xAng * x));
	//		cosY = Mathf.Cos(Mathf.Deg2Rad*(transform.rotation.eulerAngles.y -(fov.y*0.5f)+ yAng * y));
	//		sinY = Mathf.Sin(Mathf.Deg2Rad*(transform.rotation.eulerAngles.y -(fov.y*0.5f)+ yAng * y));
	//
	//		// rotate the ray using the original ray direction
	//		rayD.x = cosY * transform.forward.x + sinX * transform.forward.z;
	//		rayD.y = sinX * sinY * transform.forward.x + cosX * transform.forward.y - sinX * cosY * transform.forward.z;
	//		rayD.z = -cosX * sinY * transform.forward.x + sinX * transform.forward.y + cosX * cosY * transform.forward.z;
	//
	//		Debug.Log(rayD.ToString("F3"));
	//
	//		// update feeler's ray
	//		feelers[i].setRay(new Ray(transform.position, rayD));
	//
	//		//Debug.Log(feelers[i].getRay());
	//		Debug.DrawRay(transform.position, rayD * viewDist, Color.red);
	//	}
	//}
}
