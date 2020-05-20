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

	private EnemyMoveScript moveComp;
	private bool inited = false;

	// States
	StateName currState;
	Idle_State idleState;

    // Start is called before the first frame update
    void Start()
    {
		currState = StateName.IDLE_WANDER;

		// add move script
		moveComp = gameObject.AddComponent<EnemyMoveScript>();
		// update its values
		moveComp.walkSpeed = wanderSpeed;

		// Add new states
		idleState = gameObject.AddComponent<Idle_State>();
	}

	public void Init()
	{
		if (!inited)
		{
			inited = true;
			idleState.Init(moveComp,changeDirectionTime);
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
	// Update the state (called every frame)
	public abstract void StateUpdate();
	// When you enter this state
	public abstract void OnStateEnter();
	// When you exit this state
	public abstract void OnStateExit();
}

// idle/wander state class
public class Idle_State : State
{
	// Member variable
	// public
	public float mDirChangeTime;
	// private
	EnemyMoveScript moveScript;
	private Vector3 mWanderDir;
	private float mWanderTimer;

	public void Init(EnemyMoveScript mover, float time)
	{
		mStateName = StateName.IDLE_WANDER;
		moveScript = mover;
		mWanderTimer = 0f;
		mDirChangeTime = time;
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
			moveScript.FindRNGDir();
		}

		moveScript.UpdateMove();
	}

	public override void OnStateEnter()
	{

	}
	public override void OnStateExit()
	{

	}
}