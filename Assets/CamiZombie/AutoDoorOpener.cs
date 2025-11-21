using UnityEngine;
using System.Collections;

/// <summary>
/// Abre automáticamente puertas cuando enemigos (zombies) se acercan
/// Da la ilusión de que los zombies pueden abrir puertas
/// </summary>
public class AutoDoorOpener : MonoBehaviour
{
    [Header("Configuración de Detección")]
    [Tooltip("Tag de los enemigos que pueden abrir la puerta")]
    public string enemyTag = "Enemy";

    [Tooltip("Distancia a la que los enemigos activan la puerta")]
    public float detectionRadius = 3f;

    [Tooltip("Tiempo que la puerta permanece abierta después de detectar un enemigo")]
    public float openDuration = 5f;

    [Tooltip("¿Cerrar automáticamente la puerta después del tiempo?")]
    public bool autoClose = true;

    [Header("Animación de Apertura")]
    [Tooltip("Animator de la puerta (si existe)")]
    public Animator doorAnimator;

    [Tooltip("Nombre del parámetro bool en el Animator")]
    public string animationParameter = "IsOpen";

    [Tooltip("Si no hay Animator, rotar la puerta manualmente")]
    public bool useManualRotation = true;

    [Tooltip("Ángulo de apertura (grados)")]
    public float openAngle = 90f;

    [Tooltip("Velocidad de rotación (grados por segundo)")]
    public float rotationSpeed = 90f;

    [Header("Efectos")]
    [Tooltip("Sonido al abrir")]
    public AudioClip openSound;

    [Tooltip("Sonido al cerrar")]
    public AudioClip closeSound;

    [Tooltip("Mostrar mensajes de debug")]
    public bool showDebugMessages = true;

    // Variables privadas
    private bool isOpen = false;
    private bool isAnimating = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private AudioSource audioSource;
    private Coroutine closeCoroutine;
    private DoorController doorController;

    void Start()
    {
        // Guardar rotaciones
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));

        // Configurar AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && (openSound != null || closeSound != null))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f; // 3D sound
            audioSource.maxDistance = 20f;
        }

        // Buscar DoorController si existe
        doorController = GetComponent<DoorController>();

        if (showDebugMessages)
        {
            Debug.Log($"🚪 AutoDoorOpener inicializado en {gameObject.name}");
        }
    }

    void Update()
    {
        // Buscar enemigos cercanos
        CheckForEnemies();

        // Actualizar rotación manual si está animando
        if (isAnimating && useManualRotation && doorAnimator == null)
        {
            UpdateManualAnimation();
        }
    }

    void CheckForEnemies()
    {
        if (isOpen) return; // Ya está abierta

        // Buscar todos los objetos con el tag de enemigo
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance <= detectionRadius)
            {
                if (showDebugMessages)
                {
                    Debug.Log($"🧟 Zombie detectado cerca de {gameObject.name} - Abriendo puerta");
                }
                OpenDoor();
                break;
            }
        }
    }

    public void OpenDoor()
    {
        if (isOpen) return;

        isOpen = true;
        isAnimating = true;

        if (showDebugMessages)
        {
            Debug.Log($"✅ Abriendo puerta: {gameObject.name}");
        }

        // Reproducir sonido
        PlaySound(openSound);

        // Animar con Animator si existe
        if (doorAnimator != null)
        {
            doorAnimator.SetBool(animationParameter, true);
            isAnimating = false; // El Animator maneja la animación
        }

        // Si hay DoorController, usarlo también
        if (doorController != null)
        {
            doorController.OpenDoor();
        }

        // Programar cierre automático
        if (autoClose)
        {
            if (closeCoroutine != null)
            {
                StopCoroutine(closeCoroutine);
            }
            closeCoroutine = StartCoroutine(AutoCloseAfterDelay());
        }
    }

    public void CloseDoor()
    {
        if (!isOpen) return;

        isOpen = false;
        isAnimating = true;

        if (showDebugMessages)
        {
            Debug.Log($"🔒 Cerrando puerta: {gameObject.name}");
        }

        // Reproducir sonido
        PlaySound(closeSound);

        // Animar con Animator si existe
        if (doorAnimator != null)
        {
            doorAnimator.SetBool(animationParameter, false);
            isAnimating = false;
        }
    }

    void UpdateManualAnimation()
    {
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        // Verificar si terminó la animación
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            transform.rotation = targetRotation;
            isAnimating = false;
        }
    }

    IEnumerator AutoCloseAfterDelay()
    {
        yield return new WaitForSeconds(openDuration);

        // Verificar si todavía hay enemigos cerca antes de cerrar
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        bool enemyNearby = false;

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= detectionRadius)
                {
                    enemyNearby = true;
                    break;
                }
            }
        }

        if (!enemyNearby)
        {
            CloseDoor();
        }
        else
        {
            // Si todavía hay enemigos, esperar más tiempo
            closeCoroutine = StartCoroutine(AutoCloseAfterDelay());
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // Visualización en el editor
    void OnDrawGizmosSelected()
    {
        // Dibujar radio de detección
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Dibujar dirección de apertura
        Gizmos.color = Color.cyan;
        Vector3 direction = Quaternion.Euler(0, openAngle, 0) * transform.forward;
        Gizmos.DrawRay(transform.position, direction * 2f);
    }
}
