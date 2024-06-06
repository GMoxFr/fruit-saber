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

			// Increment the score
			GameManager.instance.IncrementScore();
		}
		else if ((gameObject.CompareTag("BlueSaber") && collision.gameObject.CompareTag("RedFruit")) ||
			(gameObject.CompareTag("RedSaber") && collision.gameObject.CompareTag("BlueFruit")))
		{
			// Decrement the score
			GameManager.instance.DecrementScore(2);
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
			// AudioManager.instance.PlayFruitSlash(fruit);
			// Perform the slice using EzySlice
			SlicedHull slicedHull = fruit.Slice(slicePlane, fruit.GetComponent<Renderer>().material);

			if (slicedHull != null)
			{
				// Create sliced halves
				GameObject upperHull = slicedHull.CreateUpperHull(fruit, fruit.GetComponent<Renderer>().material);
				GameObject lowerHull = slicedHull.CreateLowerHull(fruit, fruit.GetComponent<Renderer>().material);

				// Add necessary components to the sliced halves
				AddComponentsToSlicedHull(upperHull, slicePlane.normal);
				AddComponentsToSlicedHull(lowerHull, -slicePlane.normal);

				// Disable and destroy the original fruit after a short delay
				fruit.GetComponent<Collider>().enabled = false;
				fruit.GetComponent<Renderer>().enabled = false;
				Destroy(fruit, 1f);
			}
		}

		void AddComponentsToSlicedHull(GameObject hull, Vector3 forceDirection)
		{
			MeshCollider meshCollider = hull.AddComponent<MeshCollider>();
			meshCollider.convex = true;
			Rigidbody rb = hull.AddComponent<Rigidbody>();

			// Apply a small force to the sliced halves
			float forceMagnitude = 1.0f; // Adjust the force magnitude as needed
			rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);

			// Remove collider after applying force
			Destroy(meshCollider, 0.1f);

			Destroy(hull, 2f); // Adjust the delay as needed
		}
	}
}
