using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class SaberCollision : MonoBehaviour
{
	void OnCollisionEnter(Collision collision)
	{
		Debug.Log("Collision detected between " + gameObject.name + " and " + collision.gameObject.name);

		if ((gameObject.CompareTag("BlueSaber") && collision.gameObject.CompareTag("BlueFruit")) ||
			(gameObject.CompareTag("RedSaber") && collision.gameObject.CompareTag("RedFruit")))
		{
			// Get the point of impact and the direction of the saber's movement
			Vector3 collisionPoint = collision.contacts[0].point;
			Vector3 saberVelocity = GetComponent<SaberController>().GetAverageVelocity();

			Vector3 epsilonVector = new Vector3(0.0001f, 0.0001f, 0.0001f);

			if (saberVelocity.magnitude < epsilonVector.magnitude)
			{
				saberVelocity = transform.forward;
			}

			// Calculate the slice plane
			EzySlice.Plane slicePlane = CalculateSlicePlane(collisionPoint, saberVelocity);

			// Slice the fruit
			SliceFruit(collision.gameObject, slicePlane);
		}
	}

	EzySlice.Plane CalculateSlicePlane(Vector3 collisionPoint, Vector3 saberVelocity)
	{
		// Define the slice direction based on the saber's movement direction
		Vector3 slicePlaneNormal = Vector3.Cross(saberVelocity, Vector3.forward).normalized;

		// Create the slicing plane
		return new EzySlice.Plane(collisionPoint, slicePlaneNormal);
	}

	void SliceFruit(GameObject fruit, EzySlice.Plane slicePlane)
	{
		// Perform the slice using EzySlice
		SlicedHull slicedHull = fruit.Slice(slicePlane, fruit.GetComponent<Renderer>().material);

		if (slicedHull != null)
		{
			// Create sliced halves
			GameObject upperHull = slicedHull.CreateUpperHull(fruit, fruit.GetComponent<Renderer>().material);
			GameObject lowerHull = slicedHull.CreateLowerHull(fruit, fruit.GetComponent<Renderer>().material);

			// Add necessary components to the sliced halves
			AddComponentsToSlicedHull(upperHull);
			AddComponentsToSlicedHull(lowerHull);

			// Disable and destroy the original fruit after a short delay
			fruit.GetComponent<Collider>().enabled = false;
			fruit.GetComponent<Renderer>().enabled = false;
			// Destroy(fruit, 1f);
		}
	}

	void AddComponentsToSlicedHull(GameObject hull)
	{
		hull.AddComponent<MeshCollider>().convex = true;
		hull.AddComponent<Rigidbody>();
		Destroy(hull, 2f); // Adjust the delay as needed
	}
}
