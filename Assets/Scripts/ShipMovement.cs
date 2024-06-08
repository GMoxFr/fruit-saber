using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public float speed = 3f;
    public Vector3 stopPosition;

    void Update()
    {
        if (Vector3.Distance(transform.position, stopPosition) > 0.1f)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            Debug.Log("Ship has reached the stop position.");
        }
    }
}
