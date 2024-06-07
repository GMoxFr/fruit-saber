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
		while (GameManager.instance.GetTimeRemaining() > 0.0f && GameManager.instance.gameState == GameState.Playing)
		{
			float waitTime = 0.0f;
			// Change spawn rate based on difficulty and time remaining (goes faster as time runs out) 
			switch (GameManager.instance.GetDifficulty())
			{
				case Difficulty.Easy:
					waitTime = Random.Range(1.0f, 2.0f);
					break;
				case Difficulty.Normal:
					waitTime = Random.Range(0.5f, 1.5f);
					break;
				case Difficulty.Hard:
					waitTime = Random.Range(0.25f, 0.75f);
					break;
			}

			if (GameManager.instance.GetDifficulty() != Difficulty.Easy)
			{
				float time = GameManager.instance.GetTimeRemaining();
				if (time < GameManager.instance.setTimer * 0.8f && time > GameManager.instance.setTimer * 0.5f)
					waitTime *= 0.75f;
				else if (time > GameManager.instance.setTimer * 0.2f)
					waitTime *= 0.5f;
			}

			yield return new WaitForSeconds(waitTime);

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

			float upwardForce = Random.Range(0.30f, 0.45f);
			rb.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);

			float horizontalForceMagnitude = 0.1f; // Default horizontal force magnitude
			float boxHalfWidth = transform.localScale.x / 2;
			float distanceFromCenter = Mathf.Abs(spawnPosition.x - transform.position.x);

			// Increase the horizontal force based on the distance from the center
			horizontalForceMagnitude += (distanceFromCenter / boxHalfWidth) * Random.Range(0.05f, 0.15f);

			Vector3 horizontalForceDirection;
			Vector3 torqueDirection;

			if (spawnPosition.x > transform.position.x) // Spawned on the right side
			{
				horizontalForceDirection = Vector3.left;
				torqueDirection = Vector3.forward; // Rotate around the Z-axis
			}
			else // Spawned on the left side
			{
				horizontalForceDirection = Vector3.right;
				torqueDirection = Vector3.back; // Rotate around the Z-axis
			}

			// Apply horizontal force
			rb.AddForce(horizontalForceDirection * horizontalForceMagnitude, ForceMode.Impulse);

			// Apply torque to make the fruit spin
			float torqueMagnitude = Random.Range(0.0001f, 0.001f);
			rb.AddTorque(torqueDirection * torqueMagnitude, ForceMode.Impulse);

			// Safeguard to make sure the fruit is destroyed if it falls out of bounds
			Destroy(fruit, 20.0f);
		}
	}
}
