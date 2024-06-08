using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaberPlanetCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected between " + gameObject.name + " and " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Easy") || collision.gameObject.CompareTag("Normal") || collision.gameObject.CompareTag("Hard"))
        {
            HandlePlanetCollision(collision);
        }
    }

    void HandlePlanetCollision(Collision collision)
    {
        Vector3 collisionPoint = collision.contacts[0].point;
        Vector3 saberVelocity = collision.relativeVelocity.normalized;

        PlanetSelector planetSelector = collision.gameObject.GetComponent<PlanetSelector>();
        if (planetSelector != null)
        {
            planetSelector.SlicePlanet(collisionPoint, saberVelocity);
            planetSelector.SelectDifficultyAndChangeScene();
        }
    }
}
