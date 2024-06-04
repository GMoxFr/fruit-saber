using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaberController : MonoBehaviour
{
	public Transform handTransform; // Assign the hand transform in the Inspector

	private Rigidbody rb;
	private Vector3[] previousPositions;
	private int positionIndex;
	private int positionCount;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		if (rb == null)
		{
			Debug.LogError("No Rigidbody found on the saber.");
			return;
		}

		// Initialize the previous positions array
		previousPositions = new Vector3[3]; // Tracking the last 3 positions
		positionIndex = 0;
		positionCount = 0;
	}

	void FixedUpdate()
	{
		if (handTransform != null)
		{
			// Update the previous positions array
			previousPositions[positionIndex] = handTransform.position;
			positionIndex = (positionIndex + 1) % previousPositions.Length;
			if (positionCount < previousPositions.Length)
			{
				positionCount++;
			}
		}
	}

	public Vector3 GetAverageVelocity()
	{
		if (positionCount < 2)
		{
			return Vector3.zero;
		}

		Vector3 totalDisplacement = Vector3.zero;
		for (int i = 0; i < positionCount - 1; i++)
		{
			int currentIndex = (positionIndex + i) % previousPositions.Length;
			int nextIndex = (currentIndex + 1) % previousPositions.Length;
			totalDisplacement += previousPositions[nextIndex] - previousPositions[currentIndex];
		}

		return totalDisplacement / ((positionCount - 1) * Time.fixedDeltaTime);
	}
}
