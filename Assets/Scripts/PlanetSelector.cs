using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.SceneManagement;

public class PlanetSelector : MonoBehaviour
{
	public Difficulty difficultyLevel;

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Saber"))
		{
			Vector3 collisionPoint = collision.GetContact(0).point;
			Vector3 saberVelocity = collision.relativeVelocity.normalized;

			SlicePlanet(collisionPoint, saberVelocity);
			SelectDifficultyAndChangeScene();
		}
	}

	public void SlicePlanet(Vector3 collisionPoint, Vector3 saberVelocity)
	{
		// Use EzySlice to cut the planet
		EzySlice.Plane slicePlane = new EzySlice.Plane(collisionPoint, saberVelocity);
		SlicedHull slicedHull = gameObject.Slice(slicePlane, GetComponent<Renderer>().material);

		if (slicedHull != null)
		{
			GameObject upperHull = slicedHull.CreateUpperHull(gameObject, GetComponent<Renderer>().material);
			GameObject lowerHull = slicedHull.CreateLowerHull(gameObject, GetComponent<Renderer>().material);

			AddComponentsToSlicedHull(upperHull, slicePlane.normal);
			AddComponentsToSlicedHull(lowerHull, -slicePlane.normal);

			// Deactivate the original planet
			GetComponent<Collider>().enabled = false;
			GetComponent<Renderer>().enabled = false;
			Destroy(gameObject, 1f);
		}
	}

	public void SelectDifficultyAndChangeScene()
	{
		GameManager.instance.SetDifficulty(difficultyLevel);
		Debug.Log("Selected difficulty: " + difficultyLevel);
		SceneManager.LoadScene("MathisScene");
	}

	void AddComponentsToSlicedHull(GameObject hull, Vector3 sliceNormal)
	{
		if (hull == null)
		{
			Debug.LogError("Hull is null.");
			return;
		}

		hull.AddComponent<MeshCollider>().convex = true;
		Rigidbody rb = hull.AddComponent<Rigidbody>();
		rb.useGravity = true;
		rb.AddForce(sliceNormal * Random.Range(5.0f, 10.0f), ForceMode.Impulse);
		Destroy(hull, 5f);
	}
}
