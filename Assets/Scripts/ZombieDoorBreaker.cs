using UnityEngine;
using System.Collections;

/// <summary>
/// Hace que los zombies puedan "romper" puertas con golpes
/// Añade sonidos de golpes y sacudidas a la puerta
/// </summary>
public class ZombieDoorBreaker : MonoBehaviour
{
    [Header("Configuración de Golpes")]
    [Tooltip("Tag de los enemigos que golpean la puerta")]
    public string enemyTag = "Enemy";

    [Tooltip("Distancia a la que los zombies pueden golpear")]
    public float knockRange = 2f;

    [Tooltip("Golpes necesarios para abrir/romper la puerta")]
    public int hitsToBreak = 5;

    [Tooltip("Tiempo entre golpes (segundos)")]
    public float timeBetweenKnocks = 1.5f;

    [Header("Efectos de Golpes")]
    [Tooltip("Sonidos de golpes aleatorios")]
    public AudioClip[] knockSounds;

    [Tooltip("Sonido cuando se rompe la puerta")]
    public AudioClip breakSound;

    [Tooltip("Intensidad de la sacudida de cámara")]
    public float shakeIntensity = 0.1f;

    [Tooltip("Duración de la sacudida")]
    public float shakeDuration = 0.2f;

    [Header("Efectos Visuales")]
    [Tooltip("Partículas al golpear (polvo, astillas, etc.)")]
    public GameObject hitParticles;

    [Tooltip("Partículas al romper la puerta")]
    public GameObject breakParticles;

    [Header("Estado de la Puerta")]
    [Tooltip("¿La puerta se puede romper completamente?")]
    public bool canBreak = true;

    [Tooltip("¿Solo abrir en lugar de romper?")]
    public bool onlyOpen = false;

    // Variables privadas
    private int currentHits = 0;
    private bool isBroken = false;
    private bool isKnocking = false;
    private AudioSource audioSource;
    private AutoDoorOpener autoDoorOpener;
    private DoorController doorController;
    private Vector3 originalPosition;

    void Start()
    {
        // Configurar AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f;
            audioSource.maxDistance = 25f;
        }

        // Buscar componentes de puerta
        autoDoorOpener = GetComponent<AutoDoorOpener>();
        doorController = GetComponent<DoorController>();

        originalPosition = transform.position;

        Debug.Log($"🚪 ZombieDoorBreaker inicializado en {gameObject.name}");
    }

    void Update()
    {
        if (isBroken) return;

        CheckForZombiesKnocking();
    }

    void CheckForZombiesKnocking()
    {
        if (isKnocking) return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance <= knockRange)
            {
                StartCoroutine(KnockSequence());
                break;
            }
        }
    }

    IEnumerator KnockSequence()
    {
        isKnocking = true;

        // Esperar un poco antes del primer golpe
        yield return new WaitForSeconds(Random.Range(0.3f, 0.8f));

        // Golpear la puerta
        PerformKnock();

        // Esperar antes de permitir otro golpe
        yield return new WaitForSeconds(timeBetweenKnocks);

        isKnocking = false;
    }

    void PerformKnock()
    {
        currentHits++;

        Debug.Log($"🧟 Zombie golpeó la puerta! ({currentHits}/{hitsToBreak})");

        // Reproducir sonido de golpe
        if (knockSounds != null && knockSounds.Length > 0)
        {
            AudioClip randomKnock = knockSounds[Random.Range(0, knockSounds.Length)];
            if (randomKnock != null)
            {
                audioSource.PlayOneShot(randomKnock);
            }
        }

        // Crear efecto de partículas
        if (hitParticles != null)
        {
            Instantiate(hitParticles, transform.position, Quaternion.identity);
        }

        // Sacudir la puerta
        StartCoroutine(ShakeDoor());

        // Verificar si se rompe
        if (currentHits >= hitsToBreak)
        {
            BreakDoor();
        }
    }

    IEnumerator ShakeDoor()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-shakeIntensity, shakeIntensity);
            float y = Random.Range(-shakeIntensity, shakeIntensity);

            transform.position = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
    }

    void BreakDoor()
    {
        isBroken = true;

        Debug.Log($"💥 ¡Puerta rota por los zombies!");

        // Reproducir sonido de rotura
        if (breakSound != null)
        {
            audioSource.PlayOneShot(breakSound);
        }

        // Crear efecto de partículas
        if (breakParticles != null)
        {
            Instantiate(breakParticles, transform.position, Quaternion.identity);
        }

        if (onlyOpen)
        {
            // Solo abrir la puerta
            if (autoDoorOpener != null)
            {
                autoDoorOpener.OpenDoor();
            }
            else if (doorController != null)
            {
                doorController.OpenDoor();
            }
        }
        else if (canBreak)
        {
            // Destruir la puerta completamente
            StartCoroutine(DestroyDoorAfterDelay());
        }
    }

    IEnumerator DestroyDoorAfterDelay()
    {
        // Esperar un poco para que se vea el efecto
        yield return new WaitForSeconds(0.5f);

        // Hacer que la puerta caiga o desaparezca
        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.AddForce(transform.forward * 300f);
            rb.AddTorque(Random.insideUnitSphere * 100f);
        }

        // Desactivar colisiones con el jugador
        if (GetComponent<Collider>() != null)
        {
            GetComponent<Collider>().isTrigger = true;
        }

        // Destruir después de 3 segundos
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    /// <summary>
    /// Método público para reiniciar la puerta
    /// </summary>
    public void ResetDoor()
    {
        currentHits = 0;
        isBroken = false;
        isKnocking = false;
        transform.position = originalPosition;
    }

    void OnDrawGizmosSelected()
    {
        // Dibujar rango de golpes
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, knockRange);

        // Mostrar progreso de golpes
        if (Application.isPlaying && currentHits > 0)
        {
            Gizmos.color = Color.Lerp(Color.green, Color.red, (float)currentHits / hitsToBreak);
            Gizmos.DrawWireCube(transform.position + Vector3.up * 2f, Vector3.one * 0.5f);
        }
    }
}
