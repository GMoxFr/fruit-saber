using UnityEngine;

public class FloatInPlace : MonoBehaviour
{
    [SerializeField] float floatSpeed = 1f;  // Vitesse du mouvement de flottement
    [SerializeField] float floatAmount = 0.5f;  // Amplitude du mouvement de flottement

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        // Calculer les déplacements en utilisant des fonctions sinusoïdales pour chaque axe
        float offsetX = Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        float offsetY = Mathf.Cos(Time.time * floatSpeed) * floatAmount;
        float offsetZ = Mathf.Sin(Time.time * floatSpeed * 0.5f) * floatAmount; // Différente fréquence pour varier le mouvement

        // Appliquer le déplacement à la position initiale
        transform.position = initialPosition + new Vector3(offsetX, offsetY, offsetZ);
    }
}
