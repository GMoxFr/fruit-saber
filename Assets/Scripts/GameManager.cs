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
    public GameObject gameZone;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            // Calculate the horizontal forward direction from the camera (ignoring the vertical component)
            Vector3 horizontalForward = new Vector3(playerTransform.forward.x, 0, playerTransform.forward.z).normalized;

            // Calculate the position in front of the camera, maintaining the fixed horizontal distance
            Vector3 forwardPosition = playerTransform.position + horizontalForward * 5;

            // Set the y component to the same as the gameZone's current y component to ignore vertical movement
            forwardPosition.y = gameZone.transform.position.y;

            gameZone.transform.position = forwardPosition;

            // Only consider horizontal rotation (y-axis rotation) for the gameZone
            Quaternion horizontalRotation = Quaternion.Euler(0, playerTransform.eulerAngles.y, 0);
            gameZone.transform.rotation = horizontalRotation;
        }
    }
}