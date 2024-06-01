using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaberCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        print("Collision detected between " + gameObject.name + " and " + collision.gameObject.name);

        if (gameObject.CompareTag("BlueSaber") && collision.gameObject.CompareTag("BlueFruit"))
        {
            // Add your logic here for when the BlueSaber hits a BlueFruit
            Destroy(collision.gameObject);
        }
        else if (gameObject.CompareTag("RedSaber") && collision.gameObject.CompareTag("RedFruit"))
        {
            // Add your logic here for when the RedSaber hits a RedFruit
            Destroy(collision.gameObject);
        }
    }
}
