using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Evita que los zombies empujen al jugador
/// - Desactiva colisiones físicas entre zombie y jugador
/// - El zombie puede atacar pero no empujar
/// - Mantiene las colisiones con el entorno
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class ZombiePushFix : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Layer del jugador (debe ser 'Player')")]
    public string playerLayer = "Player";

    [Tooltip("Desactivar Rigidbody cuando está cerca del jugador")]
    public bool disableRigidbodyNearPlayer = true;

    [Tooltip("Distancia para detectar al jugador")]
    public float playerDetectionRange = 3f;

    private NavMeshAgent agent;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        // Buscar al jugador
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // Configurar Rigidbody para que no interfiera con NavMeshAgent
        if (rb != null)
        {
            rb.isKinematic = true; // IMPORTANTE: Debe ser kinematic
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        // Configurar Collider
        if (capsuleCollider != null)
        {
            // El collider debe ser trigger para ataques, pero no trigger para paredes
            // Solución: usar dos colliders
            capsuleCollider.isTrigger = false; // No trigger para colisiones con paredes
        }

        // Configurar NavMeshAgent para que no empuje
        if (agent != null)
        {
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            // Prioridad baja para que evite al jugador en lugar de empujarlo
            agent.avoidancePriority = 90; // Mayor número = menor prioridad
        }

        ConfigurePhysicsLayers();
    }

    void ConfigurePhysicsLayers()
    {
        // Opción 1: Usar layers para ignorar colisiones entre zombie y jugador
        int zombieLayer = gameObject.layer;
        int playerLayerInt = LayerMask.NameToLayer(playerLayer);

        if (playerLayerInt != -1)
        {
            // Ignorar colisiones físicas entre zombie y jugador
            Physics.IgnoreLayerCollision(zombieLayer, playerLayerInt, true);
        }
    }

    void Update()
    {
        // Opcional: Ajustar comportamiento cerca del jugador
        if (disableRigidbodyNearPlayer && player != null && rb != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance < playerDetectionRange)
            {
                // Muy cerca del jugador - asegurar que no empuje
                rb.isKinematic = true;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Si colisiona con el jugador, desactivar colisión física
        if (collision.gameObject.CompareTag("Player"))
        {
            // Ignorar esta colisión específica
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>(), true);
        }
    }
}
