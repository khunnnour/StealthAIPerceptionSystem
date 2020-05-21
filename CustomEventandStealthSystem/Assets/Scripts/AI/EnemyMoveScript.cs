using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveScript : MonoBehaviour
{
	// Variables
	public bool walk = true;

	[Header("Speeds")]
	public float walkSpeed;
	public float sprintSpeed;

	//[Header("Whiskers")]
	private float whiskerAngle = 30f;
	private float whiskerLength;

	private Vector3 moveDir;
	private Quaternion mTargetLook;
	private Ray lRay, rRay;
	private bool inited = false;

	// Start is called before the first frame update
	void Start()
	{
		// TODO: Replace with velocity
		// makes whiskers that feel out to a distance of the average of the two speeds (0.5 puts it outside of the capsule radius)
		whiskerLength = 0.5f + (walkSpeed + sprintSpeed) / 2f;

		// create whiskers
		lRay = new Ray(transform.position, transform.forward);
		rRay = new Ray(transform.position, transform.forward);
		UpdateWhiskers();

		// sets initial direction to forward
		moveDir = transform.forward;
	}

	public void Init(float wSpeed, float sSpeed)
	{
		if (!inited)
		{
			walkSpeed = wSpeed;
			sprintSpeed = sSpeed;

			inited = true;
		}
	}

	// faces direction and moves in direction on call
	public void UpdateMove()
	{
		UpdateWhiskers();
		Face();
		Move();

		Debug.DrawRay(transform.position, moveDir * whiskerLength * 1.5f, Color.cyan);
	}

	private void SetMoveDir(Vector3 dir)
	{
		// set new wander vec
		moveDir = dir;

		// set new target look rotation
		mTargetLook = Quaternion.LookRotation(moveDir);
	}

	// This basically uses the unit forward/right vectors and the cos/sin results to make a unit vector in a random direciton
	public void FindRNGDir()
	{
		// random float between 0 and 2pi
		float rng = UnityEngine.Random.Range(0f, 6.1415f);

		// turn that into a unit vector coefficients
		float cos = Mathf.Cos(rng);
		float sin = Mathf.Sin(rng);

		// set new wander vec
		moveDir = new Vector3(transform.forward.x * cos - transform.forward.z * sin,
							  0f,
							  transform.forward.x * sin + transform.forward.z * cos);

		// set new target look rotation
		mTargetLook = Quaternion.LookRotation(moveDir);
	}

	// Update the status of the whiskers
	private void UpdateWhiskers()
	{
		Vector3 rayD = Vector3.zero;

		// update ray origins (just outside the capsule
		lRay.origin = transform.position + transform.forward * 0.51f;
		rRay.origin = transform.position + transform.forward * 0.51f;

		// update ray directions
		float rot = Mathf.Deg2Rad * (-whiskerAngle);
		float cos = Mathf.Cos(rot);
		float sin = Mathf.Sin(rot);
		rayD.x = transform.forward.x * cos + transform.forward.z * sin;
		rayD.y = 0f;
		rayD.z = -transform.forward.x * sin + transform.forward.z * cos;
		lRay.direction = rayD.normalized * whiskerLength;

		rot = Mathf.Deg2Rad * (whiskerAngle);
		cos = Mathf.Cos(rot);
		sin = Mathf.Sin(rot);
		rayD.x = transform.forward.x * cos + transform.forward.z * sin;
		rayD.y = 0f;
		rayD.z = -transform.forward.x * sin + transform.forward.z * cos;
		rRay.direction = rayD.normalized * whiskerLength;

		// TODO: remove debug
		//Debug.DrawRay(lRay.origin, lRay.direction * whiskerLength, Color.green);
		//Debug.DrawRay(rRay.origin, rRay.direction * whiskerLength, Color.green);

		// cast rays
		bool lHit = Physics.Raycast(lRay, whiskerLength);
		bool rHit = Physics.Raycast(rRay, whiskerLength);

		// if both hit skrrt 180 backwards
		if (lHit && rHit)
		{
			Debug.Log("Both whiskers hit");
			SetMoveDir(-transform.forward);
		}
		else if (lHit)
		{
			Debug.Log("Left whisker hit");
			// turn right if left whisker is hitting something
			SetMoveDir(transform.right);
		}
		else if (rHit)
		{
			Debug.Log("Right whisker hit");
			// turn left if right whisker is hitting something
			SetMoveDir(-transform.right);
		}
	}

	// Face function
	private void Face()
	{
		// Slerp towards wander direction
		transform.rotation = Quaternion.Slerp(transform.rotation, mTargetLook, Time.deltaTime * walkSpeed);
	}

	// Move function
	private void Move()
	{
		Vector3 newPos = gameObject.transform.position;

		// move in direction you're facing
		if (walk)
			newPos += transform.forward * walkSpeed * Time.deltaTime;
		else
			newPos += transform.forward * sprintSpeed * Time.deltaTime;

		gameObject.transform.position = newPos;
	}
}
