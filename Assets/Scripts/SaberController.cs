using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaberController : MonoBehaviour
{
	public Transform handTransform; // Assign the hand transform in the Inspector

	private Rigidbody rb;
	private Vector3 previousPosition;
	private Vector3 saberVelocity;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		if (rb == null)
		{
			Debug.LogError("No Rigidbody found on the saber.");
			return;
		}

		previousPosition = handTransform.position;
	}

	void FixedUpdate()
	{
		if (handTransform != null)
		{
			// Manually calculate the velocity
			saberVelocity = (handTransform.position - previousPosition) / Time.fixedDeltaTime;
			previousPosition = handTransform.position;
		}
	}

	public Vector3 GetSaberVelocity()
	{
		return saberVelocity;
	}
}
