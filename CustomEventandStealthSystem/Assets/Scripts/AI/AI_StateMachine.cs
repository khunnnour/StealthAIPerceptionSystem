using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateName
{
	INAVLID_STATE = -1,
	IDLE_WANDER,
	ALERTED,
	CHASE,
	ATTACK
}

public class AI_StateMachine : MonoBehaviour
{
	// Variables
	[Header("Wander Variables")]
	public float wanderSpeed;
	public float changeDirectionTime;

	private bool inited = false;

	// States
	StateName currState;
	Idle_State idleState;

    // Start is called before the first frame update
    void Start()
    {
		currState = StateName.IDLE_WANDER;
		
		idleState = gameObject.AddComponent<Idle_State>();
	}

	public void Init()
	{
		if (!inited)
		{
			inited = true;
			idleState.Init(wanderSpeed, changeDirectionTime);
			//idleState.mWanderSpeed = wanderSpeed;
			//idleState.mDirChangeTime = changeDirectionTime;
		}
	}

	// Update whole ass machine
	public void UpdateMachine()
    {
		UpdateState();
    }

	private void UpdateState()
	{
		switch (currState)
		{
			case StateName.INAVLID_STATE:
				Debug.LogError("INVALID CURRENT STATE");
				break;
			case StateName.IDLE_WANDER:
				idleState.StateUpdate();
				break;
			case StateName.ALERTED:
				break;
			case StateName.CHASE:
				break;
			case StateName.ATTACK:
				break;
			default:
				break;
		}
	}
}

// Base state class
public abstract class State:MonoBehaviour
{
	// Member variable
	public StateName mStateName;

	// State update funcitons
	public abstract void StateUpdate();
	public abstract void OnStateEnter();
	public abstract void OnStateExit();
}

// idle/wander state class
public class Idle_State : State
{
	// Member variable
	// public
	public float mWanderSpeed, mDirChangeTime;
	// private
	private Vector3 mWanderDir;
	private float mWanderTimer;
	private Quaternion mTargetLook;

	// Constuctor
	//public Idle_State()
	//{
	//	mStateName = StateName.IDLE_WANDER;
	//	mWanderDir = transform.forward;
	//	mWanderSpeed = 5f;
	//	mWanderTimer = 0f;
	//	mDirChangeTime = 1f;
	//}
	//public Idle_State(float speed/*wander speed*/, float time/*time between direciton changes*/)
	//{
	//	mStateName = StateName.IDLE_WANDER;
	//	mWanderDir = transform.forward;
	//	mWanderSpeed = speed;
	//	mWanderTimer = 0f;
	//	mDirChangeTime = time;
	//}
	public void Init(float speed/*wander speed*/, float time/*time between direciton changes*/)
	{
		mStateName = StateName.IDLE_WANDER;
		mWanderDir = transform.forward;
		mWanderSpeed = speed;
		mWanderTimer = 0f;
		mDirChangeTime = time;
	}


	/* - Wander functions - */
	// This basically uses the unit forward/right vectors and the cos/sin results to make a unit vector in a random direciton
	void FindNewDir()
	{
		// random float between 0 and 2pi
		float rng = UnityEngine.Random.Range(0f, 6.1415f);

		// turn that into a unit vector coefficients
		float cos = Mathf.Cos(rng);
		float sin = Mathf.Sin(rng);

		// set new wander vec
		mWanderDir = transform.forward * cos + transform.right * sin;

		// set new target look rotation
		mTargetLook = Quaternion.LookRotation(mWanderDir);
	}

	// Face function
	private void Face()
	{
		// Slerp towards wander direction
		transform.rotation = Quaternion.Slerp(transform.rotation, mTargetLook, Time.deltaTime * mWanderSpeed);
	}

	// Move function
	private void Move()
	{
		Vector3 newPos = gameObject.transform.position;

		// move in direction you're facing
		newPos += transform.forward * mWanderSpeed * Time.deltaTime;

		gameObject.transform.position = newPos;
	}

	// State update funcitons
	public override void StateUpdate()
	{
		// update timer
		mWanderTimer += Time.deltaTime;

		// check if time to change direction
		if (mWanderTimer > mDirChangeTime)
		{
			mWanderTimer = 0f;
			FindNewDir();
		}

		Face();

		Move();

	}

	public override void OnStateEnter()
	{

	}
	public override void OnStateExit()
	{

	}
}