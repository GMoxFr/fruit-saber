using UnityEngine;

public class MoveSpaceShip : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;  // Vitesse de d�placement

    void Update()
    {
        // D�placer l'objet vers l'avant � une vitesse constante
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
