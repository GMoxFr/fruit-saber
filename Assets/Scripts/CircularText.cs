using UnityEngine;
using TMPro;

public class CircularTextRotation : MonoBehaviour
{
    public GameObject sphere; // Référence à la sphère
    public string[] texts; // Tableau des textes à afficher
    public float radius = 2.0f; // Rayon autour de la sphère
    public float rotationSpeed = 10f; // Vitesse de rotation

    private Transform[] textTransforms;

    void Start()
    {
        Debug.Log("CircularTextRotation script started");
        if (sphere == null)
        {
            Debug.LogError("Sphere is not assigned!");
            return;
        }

        if (texts == null || texts.Length == 0)
        {
            Debug.LogError("Texts are not assigned!");
            return;
        }

        CreateCircularText();
    }

    void Update()
    {
        RotateTextAroundSphere();
    }

    void CreateCircularText()
    {
        Debug.Log("Creating circular text");
        textTransforms = new Transform[texts.Length];
        float angleStep = 360.0f / texts.Length;
        for (int i = 0; i < texts.Length; i++)
        {
            // Créer un nouvel objet TextMeshPro 3D
            GameObject textObject = new GameObject("Text" + i);
            textObject.transform.SetParent(transform);
            TextMeshPro textMesh = textObject.AddComponent<TextMeshPro>();
            textMesh.text = texts[i];
            textMesh.fontSize = 5; // Ajustez la taille de la police selon vos besoins
            textMesh.alignment = TextAlignmentOptions.Center;

            // Calculer la position autour de la sphère
            float angle = i * angleStep * Mathf.Deg2Rad;
            float x = Mathf.Sin(angle) * radius;
            float z = Mathf.Cos(angle) * radius;

            textObject.transform.localPosition = new Vector3(x, 0, z);
            textObject.transform.LookAt(sphere.transform);
            textObject.transform.Rotate(0, 180, 0); // Ajuster la rotation pour que le texte soit lisible

            textTransforms[i] = textObject.transform;

            Debug.Log("Text " + i + " positioned at " + textObject.transform.localPosition);
        }
    }

    void RotateTextAroundSphere()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
