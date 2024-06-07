using UnityEngine;

public class MoveMainShip : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;  // Vitesse de d�placement initiale
    [SerializeField] float stopTime = 10f;  // Temps d'arr�t initial
    [SerializeField] ParticleSystem trailParticleSystemPrefab; // Pr�fabriqu� du syst�me de particules � assigner dans l'inspecteur
    [SerializeField] Vector3 particleOffset = new Vector3(0, 0, -1); // D�calage de la position des particules par rapport � l'avion
    [SerializeField] Vector3 particleRotation = new Vector3(0, 0, 0); // Rotation des particules

    private float initialSpeed;
    private float accelerationTime = 5f;
    private float waitTime = 3f; // Temps d'attente avant l'acc�l�ration
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

        // Instancier le pr�fabriqu� du syst�me de particules
        if (trailParticleSystemPrefab != null)
        {
            trailParticleSystemInstance = Instantiate(trailParticleSystemPrefab, transform.position + particleOffset, Quaternion.Euler(particleRotation));
            trailParticleSystemInstance.Stop();
        }
        else
        {
            Debug.LogError("Le pr�fabriqu� du syst�me de particules n'est pas assign� !");
        }
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (!isStopping && !isWaiting && !isAccelerating)
        {
            if (elapsedTime <= stopTime)
            {
                // Phase de d�placement initial
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
            else
            {
                // Arr�ter l'avion apr�s le temps d�fini par stopTime
                moveSpeed = 0f;
                isStopping = true;
                elapsedTime = 0f; // R�initialiser le temps pour la phase d'attente

                // Activer les particules
                if (trailParticleSystemInstance != null && !trailParticleSystemInstance.isPlaying)
                {
                    trailParticleSystemInstance.transform.position = transform.position + particleOffset; // Positionner les particules � l'arri�re de l'avion
                    trailParticleSystemInstance.transform.rotation = Quaternion.Euler(particleRotation); // Ajuster la rotation des particules
                    trailParticleSystemInstance.Play();
                }
            }
        }
        else if (isStopping)
        {
            // Attente avant l'acc�l�ration
            if (elapsedTime >= waitTime)
            {
                isStopping = false;
                isAccelerating = true;
                elapsedTime = 0f; // R�initialiser le temps pour la phase d'acc�l�ration

                // D�sactiver les particules
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
                // Acc�l�ration exponentielle pendant 5 secondes
                moveSpeed = initialSpeed * Mathf.Exp(elapsedTime);
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
            else
            {
                // R�initialiser l'avion � la position de spawn apr�s l'acc�l�ration
                isAccelerating = false;
                transform.position = spawnPosition;
                elapsedTime = 0f; // R�initialiser le temps pour recommencer le cycle
                moveSpeed = initialSpeed; // R�initialiser la vitesse de d�placement
            }
        }
    }
}
