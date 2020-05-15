using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
	private AI_StateMachine machine;
	private bool inited = false;

	// Start is called before the first frame update
	void Start()
	{
		machine = GetComponent<AI_StateMachine>();
	}

	// Update is called once per frame
	void Update()
	{
		if (!inited)
			machine.Init();

		machine.UpdateMachine();
	}
}
