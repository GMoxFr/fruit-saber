using UnityEngine;

public class MoveMainShip : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;  // Vitesse de déplacement initiale
    [SerializeField] float stopTime = 10f;  // Temps d'arrêt initial
    [SerializeField] ParticleSystem trailParticleSystemPrefab; // Préfabriqué du système de particules à assigner dans l'inspecteur
    [SerializeField] Vector3 particleOffset = new Vector3(0, 0, -1); // Décalage de la position des particules par rapport à l'avion
    [SerializeField] Vector3 particleRotation = new Vector3(0, 0, 0); // Rotation des particules

    private float initialSpeed;
    private float accelerationTime = 5f;
    private float waitTime = 3f; // Temps d'attente avant l'accélération
    private float elapsedTime = 0f;
    private Vector3 spawnPosition;
    private bool isStopping = false;
    private bool isWaiting = false;
    private bool isAccelerating = false;
    private ParticleSystem trailParticleSystemInstance;

    void Start()
    {
        initialSpeed = moveSpeed;
        spawnPosition = transform.position;

        // Instancier le préfabriqué du système de particules
        if (trailParticleSystemPrefab != null)
        {
            trailParticleSystemInstance = Instantiate(trailParticleSystemPrefab, transform.position + particleOffset, Quaternion.Euler(particleRotation));
            trailParticleSystemInstance.Stop();
        }
        else
        {
            Debug.LogError("Le préfabriqué du système de particules n'est pas assigné !");
        }
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (!isStopping && !isWaiting && !isAccelerating)
        {
            if (elapsedTime <= stopTime)
            {
                // Phase de déplacement initial
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
            else
            {
                // Arrêter l'avion après le temps défini par stopTime
                moveSpeed = 0f;
                isStopping = true;
                elapsedTime = 0f; // Réinitialiser le temps pour la phase d'attente

                // Activer les particules
                if (trailParticleSystemInstance != null && !trailParticleSystemInstance.isPlaying)
                {
                    trailParticleSystemInstance.transform.position = transform.position + particleOffset; // Positionner les particules à l'arrière de l'avion
                    trailParticleSystemInstance.transform.rotation = Quaternion.Euler(particleRotation); // Ajuster la rotation des particules
                    trailParticleSystemInstance.Play();
                }
            }
        }
        else if (isStopping)
        {
            // Attente avant l'accélération
            if (elapsedTime >= waitTime)
            {
                isStopping = false;
                isAccelerating = true;
                elapsedTime = 0f; // Réinitialiser le temps pour la phase d'accélération

                // Désactiver les particules
                if (trailParticleSystemInstance != null && trailParticleSystemInstance.isPlaying)
                {
                    trailParticleSystemInstance.Stop();
                }
            }
        }
        else if (isAccelerating)
        {
            if (elapsedTime <= accelerationTime)
            {
                // Accélération exponentielle pendant 5 secondes
                moveSpeed = initialSpeed * Mathf.Exp(elapsedTime);
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
            else
            {
                // Réinitialiser l'avion à la position de spawn après l'accélération
                isAccelerating = false;
                transform.position = spawnPosition;
                elapsedTime = 0f; // Réinitialiser le temps pour recommencer le cycle
                moveSpeed = initialSpeed; // Réinitialiser la vitesse de déplacement
            }
        }
    }
}
