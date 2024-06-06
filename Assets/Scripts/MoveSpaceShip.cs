using UnityEngine;

public class MoveSpaceShip : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;  // Vitesse de déplacement

    void Update()
    {
        // Déplacer l'objet vers l'avant à une vitesse constante
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
