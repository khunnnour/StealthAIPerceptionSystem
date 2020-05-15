using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class DoorController : MonoBehaviour
{
	public int id;

	private Vector3 oPos, tPos;
	bool open;
	int steps;
	float step;
	// Start is called before the first frame update
	void Start()
	{
		CustomEventSystem.current.onDoorwayTriggerEnter += OnDoorwayOpen;
		CustomEventSystem.current.onDoorwayTriggerExit += OnDoorwayClose;

		oPos = transform.position;
		tPos = transform.position + (Vector3.up * 3.5f);
		open = false;

		steps = 15;
		step = 1f / steps;
	}

	private void Update()
	{
		if(open)
		{
			if (transform.position != tPos)
				transform.position = Vector3.Lerp(transform.position, tPos, step);
		}
		else
		{
			if (transform.position != oPos)
				transform.position = Vector3.Lerp(transform.position, oPos, step);
		}
	}

	private void OnDoorwayOpen(int id)
	{
		if (id == this.id)
			open = true;
	}

	private void OnDoorwayClose(int id)
	{
		if (id == this.id)
			open = false;
	}

	private void OnDestroy()
	{
		CustomEventSystem.current.onDoorwayTriggerEnter -= OnDoorwayOpen;
		CustomEventSystem.current.onDoorwayTriggerExit -= OnDoorwayClose;
	}
}
