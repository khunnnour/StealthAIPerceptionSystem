using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
	public int id;

	private void OnTriggerEnter(Collider other)
	{
		CustomEventSystem.current.DoorwayTriggerEnter(id);
	}

	private void OnTriggerExit(Collider other)
	{
		CustomEventSystem.current.DoorwayTriggerExit(id);
	}
}
