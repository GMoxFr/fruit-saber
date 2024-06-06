using UnityEngine;

public class RotatePlanet : MonoBehaviour
{
    // Speed of rotation
    [SerializeField] float rotationSpeed = 1;

    void Update()
    {
        // Rotate the planet around its Y axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
