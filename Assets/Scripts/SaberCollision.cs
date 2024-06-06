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

            // Slice the fruit
            SliceFruit(collision.gameObject, collisionPoint, saberVelocity);

            // Increment the score
            GameManager.instance.IncrementScore();
        }
        else if ((gameObject.CompareTag("BlueSaber") && collision.gameObject.CompareTag("RedFruit")) ||
                 (gameObject.CompareTag("RedSaber") && collision.gameObject.CompareTag("BlueFruit")))
        {
            // Decrement the score
            GameManager.instance.DecrementScore(2);
        }
    }

    EzySlice.Plane CalculateSlicePlane(Vector3 collisionPoint, Vector3 saberVelocity, GameObject fruit)
    {
        // Define the slice direction based on the saber's movement direction
        Vector3 slicePlaneNormal = Vector3.Cross(saberVelocity, Vector3.forward).normalized;

        // Create the initial slicing plane
        EzySlice.Plane initialPlane = new EzySlice.Plane(collisionPoint, slicePlaneNormal);

        // Check if the initial plane intersects the fruit
        if (DoesPlaneIntersect(fruit, initialPlane))
        {
            return initialPlane;
        }
        else
        {
            // Create a placeholder plane that intersects the fruit
            Vector3 placeholderNormal = Vector3.up; // Or any direction you know will intersect
            return new EzySlice.Plane(collisionPoint, placeholderNormal);
        }
    }

    bool DoesPlaneIntersect(GameObject fruit, EzySlice.Plane plane)
    {
        Mesh mesh = fruit.GetComponent<MeshFilter>().mesh;
        Bounds bounds = mesh.bounds;

        Vector3[] vertices = {
            bounds.min,
            new Vector3(bounds.min.x, bounds.min.y, bounds.max.z),
            new Vector3(bounds.min.x, bounds.max.y, bounds.min.z),
            new Vector3(bounds.min.x, bounds.max.y, bounds.max.z),
            new Vector3(bounds.max.x, bounds.min.y, bounds.min.z),
            new Vector3(bounds.max.x, bounds.min.y, bounds.max.z),
            new Vector3(bounds.max.x, bounds.max.y, bounds.min.z),
            bounds.max
        };

        foreach (Vector3 vertex in vertices)
        {
            if (plane.SideOf(fruit.transform.TransformPoint(vertex)) == EzySlice.SideOfPlane.ON)
            {
                return true;
            }
        }
        return false;
    }

    void SliceFruit(GameObject fruit, Vector3 collisionPoint, Vector3 saberVelocity)
    {
        EzySlice.Plane slicePlane = CalculateSlicePlane(collisionPoint, saberVelocity, fruit);

        AudioManager.instance.PlayFruitSlash(fruit);

        Debug.Log($"Slicing {fruit.name} with plane normal: {slicePlane.normal} and point: {collisionPoint}");

        // Perform the slice using EzySlice
        SlicedHull slicedHull = fruit.Slice(slicePlane, fruit.GetComponent<Renderer>().material);

        if (slicedHull != null)
        {
            Debug.Log("Slicing successful. Creating upper and lower hulls.");
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
        else
        {
            Debug.LogWarning("Slicing failed. SlicedHull is null.");
            fruit.GetComponent<Collider>().enabled = false;
            fruit.GetComponent<Renderer>().enabled = false;
            Destroy(fruit, 1f);
        }
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
