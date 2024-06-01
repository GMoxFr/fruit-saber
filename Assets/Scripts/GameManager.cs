using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager instance { get; private set; }

    // Variables
    //// Player Transform
    public Transform playerTransform;

    //// Game Zone
    public GameObject gameZone;
    private BoxCollider gameZoneCollider;

    //// Prefabs for fruits
    public GameObject[] fruitPrefabs;

    //// Available colors for fruits (one for each saber color)
    public Material blueSaberMaterial;
    public Material redSaberMaterial;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this) Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameZoneCollider = gameZone.GetComponent<BoxCollider>();

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

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            // Calculate the horizontal forward direction from the camera (ignoring the vertical component)
            Vector3 horizontalForward = new Vector3(playerTransform.forward.x, 0, playerTransform.forward.z).normalized;

            // Calculate the position in front of the camera, maintaining the fixed horizontal distance
            Vector3 forwardPosition = playerTransform.position + horizontalForward;

            // Set the y component to the same as the gameZone's current y component to ignore vertical movement
            forwardPosition.y = gameZone.transform.position.y;

            gameZone.transform.position = forwardPosition;

            // Only consider horizontal rotation (y-axis rotation) for the gameZone
            Quaternion horizontalRotation = Quaternion.Euler(0, playerTransform.eulerAngles.y, 0);
            gameZone.transform.rotation = horizontalRotation;
        }
    }

    IEnumerator SpawnFruits()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            if (fruitPrefabs.Length == 0)
            {
                Debug.LogError("Fruit prefabs array is empty at runtime!");
                continue;
            }

            // Randomly select a fruit prefab
            GameObject fruitPrefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];

            // Calculate a random position within the BoxCollider bounds
            Vector3 spawnPosition = new Vector3(
                Random.Range(gameZoneCollider.bounds.min.x, gameZoneCollider.bounds.max.x),
                Random.Range(gameZoneCollider.bounds.min.y, gameZoneCollider.bounds.max.y),
                Random.Range(gameZoneCollider.bounds.min.z, gameZoneCollider.bounds.max.z)
            );

            GameObject fruit = Instantiate(fruitPrefab, spawnPosition, Quaternion.identity);

            // Ensure the fruit has a Renderer component
            Renderer fruitRenderer = fruit.GetComponent<Renderer>();
            if (fruitRenderer != null)
            {
                // Randomly assign a material and tag
                if (Random.value > 0.5f)
                {
                    fruitRenderer.material = blueSaberMaterial;
                    fruit.tag = "BlueFruit";
                }
                else
                {
                    fruitRenderer.material = redSaberMaterial;
                    fruit.tag = "RedFruit";
                }

                // Add Rigidbody to the fruit
                Rigidbody fruitRb = fruit.AddComponent<Rigidbody>();
                fruitRb.useGravity = false; // or false, depending on your game logic
                fruitRb.isKinematic = false;
            }
            else
            {
                Debug.LogError("Fruit prefab does not have a Renderer component!");
            }
        }
    }
}
