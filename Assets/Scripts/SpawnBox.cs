using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBox : MonoBehaviour
{
	public GameObject[] fruitPrefabs;
	public Material blueSaberMaterial;
	public Material redSaberMaterial;

	void Start()
	{
		if (fruitPrefabs == null || fruitPrefabs.Length == 0)
		{
			Debug.LogError("Fruit prefabs array is not assigned or empty!");
		}
		else
		{
			Debug.Log("Fruit prefabs loaded successfully.");
			StartCoroutine(SpawnFruits());
		}
	}

	IEnumerator SpawnFruits()
	{
		while (GameManager.instance.GetTimeRemaining() > 0.0f)
		{
			yield return new WaitForSeconds(Random.Range(0.5f, 1.5f)); // Random spawn interval between 0.5 and 1.5 seconds

			// Randomly select a fruit prefab
			GameObject fruitPrefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];

			// Randomly select a spawn position (on the x axis of the box)
			Vector3 spawnPosition = new Vector3(
				Random.Range(transform.position.x - transform.localScale.x / 2, transform.position.x + transform.localScale.x / 2),
				transform.position.y + transform.localScale.y / 2,
				transform.position.z);

			// Randomly select a spawn rotation
			Quaternion spawnRotation = Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f);

			// Instantiate the fruit prefab
			GameObject fruit = Instantiate(fruitPrefab, spawnPosition, spawnRotation);
			AudioManager.instance.PlayFruitSpawn(fruit);

			// Assign a random color to the fruit
			if (Random.value < 0.5f)
			{
				fruit.GetComponent<Renderer>().material = blueSaberMaterial;
				fruit.tag = "BlueFruit";
			}
			else
			{
				fruit.GetComponent<Renderer>().material = redSaberMaterial;
				fruit.tag = "RedFruit";
			}

			// Add a Rigidbody component to the fruit
			Rigidbody rb = fruit.AddComponent<Rigidbody>();
			rb.useGravity = true;
			rb.isKinematic = false;
			rb.mass = 0.1f;

			float upwardForce = Random.Range(0.35f, 0.5f);
			rb.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);

			float horizontalForceMagnitude = 0.1f; // Default horizontal force magnitude
			float boxHalfWidth = transform.localScale.x / 2;
			float distanceFromCenter = Mathf.Abs(spawnPosition.x - transform.position.x);

			// Increase the horizontal force based on the distance from the center
			horizontalForceMagnitude += (distanceFromCenter / boxHalfWidth) * Random.Range(0.05f, 0.15f);

			if (spawnPosition.x > transform.position.x) // Spawned on the right side
			{
				rb.AddForce(Vector3.left * horizontalForceMagnitude, ForceMode.Impulse);
			}
			else // Spawned on the left side
			{
				rb.AddForce(Vector3.right * horizontalForceMagnitude, ForceMode.Impulse);
			}

			// Safeguard to make sure the fruit is destroyed if it falls out of bounds
			Destroy(fruit, 20.0f);
		}
	}
}
