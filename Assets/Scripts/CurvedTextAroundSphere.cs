using UnityEngine;
using TMPro;

public class SelectMode : MonoBehaviour
{
    public enum GameMode
    {
        Easy,
        Normal,
        Hard
    }

    [System.Serializable]
    public class PlanetInfo
    {
        public GameObject planet;
        public string text;
        public float textRadius;
        public float rotationSpeed;
        public TMP_FontAsset fontAsset;
        public Color textColor = Color.white;
        public float textSize = 2;
        public float letterSpacing = 0.1f;
        public float arcLength = 90f;
    }

    public PlanetInfo easyPlanet;
    public PlanetInfo normalPlanet;
    public PlanetInfo hardPlanet;
    public GameMode currentMode;

    void Start()
    {
        Debug.Log("SelectMode script started");

        CreateCurvedText(easyPlanet);
        CreateCurvedText(normalPlanet);
        CreateCurvedText(hardPlanet);
    }

    void Update()
    {
        RotateTextAroundPlanet(easyPlanet);
        RotatePlanet(easyPlanet);
        RotateTextAroundPlanet(normalPlanet);
        RotatePlanet(normalPlanet);
        RotateTextAroundPlanet(hardPlanet);
        RotatePlanet(hardPlanet);
    }

    void CreateCurvedText(PlanetInfo planetInfo)
    {
        if (planetInfo.planet == null)
        {
            Debug.LogError("Planet is not assigned!");
            return;
        }

        float angleStep = planetInfo.arcLength / (planetInfo.text.Replace(" ", "").Length - 1);
        int charIndex = 0;

        for (int i = 0; i < planetInfo.text.Length; i++)
        {
            if (planetInfo.text[i] == ' ')
            {
                charIndex++;
                continue;
            }

            GameObject textObject = new GameObject("Text" + i);
            textObject.transform.SetParent(planetInfo.planet.transform);

            TextMeshPro textMesh = textObject.AddComponent<TextMeshPro>();
            textMesh.text = planetInfo.text[i].ToString();
            textMesh.fontSize = planetInfo.textSize;
            textMesh.alignment = TextAlignmentOptions.Center;
            textMesh.font = planetInfo.fontAsset;
            textMesh.color = planetInfo.textColor;

            textMesh.outlineColor = Color.black;
            textMesh.outlineWidth = 0.3f;

            float angle = -planetInfo.arcLength / 2 + charIndex * angleStep;
            float x = Mathf.Sin(angle * Mathf.Deg2Rad) * planetInfo.textRadius;
            float z = Mathf.Cos(angle * Mathf.Deg2Rad) * planetInfo.textRadius;

            textObject.transform.localPosition = new Vector3(x, 0, z);

            Vector3 direction = (planetInfo.planet.transform.position - textObject.transform.position).normalized;
            textObject.transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180, 0);
            textObject.transform.Rotate(0, 180, 0);

            charIndex++;
        }
    }

    void RotateTextAroundPlanet(PlanetInfo planetInfo)
    {
        planetInfo.planet.transform.GetChild(0).Rotate(Vector3.down, planetInfo.rotationSpeed * Time.deltaTime);
    }

    void RotatePlanet(PlanetInfo planetInfo)
    {
        planetInfo.planet.transform.Rotate(Vector3.up, planetInfo.rotationSpeed * Time.deltaTime);
    }
}
