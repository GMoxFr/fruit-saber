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
        // Calculer les d�placements en utilisant des fonctions sinuso�dales pour chaque axe
        float offsetX = Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        float offsetY = Mathf.Cos(Time.time * floatSpeed) * floatAmount;
        float offsetZ = Mathf.Sin(Time.time * floatSpeed * 0.5f) * floatAmount; // Diff�rente fr�quence pour varier le mouvement

        // Appliquer le d�placement � la position initiale
        transform.position = initialPosition + new Vector3(offsetX, offsetY, offsetZ);
    }
}
