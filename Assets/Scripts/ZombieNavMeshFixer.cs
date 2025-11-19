using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Arregla problemas comunes de navegación de zombies
/// - Evita que se queden atascados en paredes
/// - Detecta cuando están bloqueados y busca rutas alternativas
/// - Mejora la navegación alrededor de obstáculos
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class ZombieNavMeshFixer : MonoBehaviour
{
    [Header("Detección de Atascamiento")]
    [Tooltip("Tiempo sin moverse para considerar que está atascado (segundos)")]
    public float stuckTime = 2f;

    [Tooltip("Velocidad mínima para considerar que se está moviendo")]
    public float minMovementSpeed = 0.1f;

    [Header("Solución de Atascamiento")]
    [Tooltip("Distancia para buscar una posición alternativa")]
    public float unstuckDistance = 3f;

    [Tooltip("Intentos máximos para desatascarse")]
    public int maxUnstuckAttempts = 5;

    [Tooltip("Tiempo entre intentos de desatascamiento")]
    public float unstuckCooldown = 1f;

    [Header("Optimización de Ruta")]
    [Tooltip("Recalcular ruta cada X segundos")]
    public float pathRecalculationInterval = 0.5f;

    [Tooltip("Distancia mínima al objetivo para recalcular")]
    public float minDistanceToRecalculate = 1f;

    [Header("Depuración")]
    [Tooltip("Mostrar mensajes de debug")]
    public bool showDebug = false;

    [Tooltip("Mostrar línea de ruta en Scene view")]
    public bool drawPath = false;

    // Variables privadas
    private NavMeshAgent agent;
    private Vector3 lastPosition;
    private float timeSinceLastMovement = 0f;
    private float lastUnstuckAttempt = 0f;
    private int unstuckAttempts = 0;
    private float lastPathRecalculation = 0f;
    private Transform target;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        lastPosition = transform.position;

        // Buscar al jugador como objetivo
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }

        // Configurar parámetros óptimos del NavMeshAgent
        ConfigureNavMeshAgent();

        if (showDebug)
        {
            Debug.Log($"🧟 ZombieNavMeshFixer inicializado en {gameObject.name}");
        }
    }

    void ConfigureNavMeshAgent()
    {
        if (agent == null) return;

        // Parámetros optimizados para evitar bugs
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        agent.avoidancePriority = Random.Range(30, 70); // Varía para evitar que se agrupen

        // Radio de evitación (ajustar según el tamaño del zombie)
        if (agent.radius < 0.3f) agent.radius = 0.3f;

        // Altura del agente
        if (agent.height < 1.5f) agent.height = 2f;

        // Velocidad y aceleración
        agent.acceleration = 8f;
        agent.angularSpeed = 120f;

        // Importante: permitir que pueda moverse fuera del NavMesh temporalmente
        agent.autoTraverseOffMeshLink = true;
        agent.autoRepath = true; // Recalcula automáticamente si se bloquea

        if (showDebug)
        {
            Debug.Log($"NavMeshAgent configurado: Speed={agent.speed}, Radius={agent.radius}, Height={agent.height}");
        }
    }

    void Update()
    {
        if (agent == null || !agent.enabled) return;

        // Detectar si está atascado
        CheckIfStuck();

        // Recalcular ruta periódicamente
        RecalculatePathPeriodically();
    }

    void CheckIfStuck()
    {
        // Calcular movimiento desde la última posición
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);

        // Si apenas se movió
        if (distanceMoved < minMovementSpeed * Time.deltaTime)
        {
            timeSinceLastMovement += Time.deltaTime;

            // Si lleva mucho tiempo sin moverse
            if (timeSinceLastMovement >= stuckTime)
            {
                // Intentar desatascar solo si ha pasado suficiente tiempo
                if (Time.time >= lastUnstuckAttempt + unstuckCooldown)
                {
                    if (showDebug)
                    {
                        Debug.LogWarning($"🧟 Zombie atascado en {gameObject.name} - Intentando desatascar ({unstuckAttempts + 1}/{maxUnstuckAttempts})");
                    }

                    Unstuck();
                    lastUnstuckAttempt = Time.time;
                    unstuckAttempts++;
                }
            }
        }
        else
        {
            // Se está moviendo normalmente
            timeSinceLastMovement = 0f;
            unstuckAttempts = 0;
        }

        lastPosition = transform.position;
    }

    void Unstuck()
    {
        if (unstuckAttempts >= maxUnstuckAttempts)
        {
            // Si falló muchas veces, simplemente resetear intentos y dejar que siga intentando
            if (showDebug)
            {
                Debug.LogWarning($"⚠️ Zombie {gameObject.name} no pudo desatascarse después de {maxUnstuckAttempts} intentos");
            }
            unstuckAttempts = 0;

            // Resetear ruta para que lo intente de nuevo
            if (agent.hasPath)
            {
                agent.ResetPath();
            }
            return;
        }

        // Método 1: Mover ligeramente hacia atrás
        Vector3 backwardDirection = -transform.forward;
        Vector3 unstuckPosition = transform.position + backwardDirection * 0.5f;

        if (NavMesh.SamplePosition(unstuckPosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);

            if (showDebug)
            {
                Debug.Log($"✅ Zombie movido hacia atrás");
            }

            // Forzar recalcular ruta después de mover
            if (target != null && agent.isOnNavMesh)
            {
                agent.SetDestination(target.position);
            }
            return;
        }

        // Método 2: Buscar posición aleatoria cercana (más lejos si está bloqueado)
        float searchRadius = unstuckDistance * (1 + unstuckAttempts * 0.5f); // Aumenta con intentos

        for (int i = 0; i < 12; i++) // Más intentos
        {
            Vector3 randomDirection = Random.insideUnitSphere * searchRadius;
            randomDirection += transform.position;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit randomHit, searchRadius, NavMesh.AllAreas))
            {
                // Verificar que la nueva posición tiene camino al jugador
                NavMeshPath testPath = new NavMeshPath();
                if (target != null && agent.CalculatePath(target.position, testPath) && testPath.status == NavMeshPathStatus.PathComplete)
                {
                    agent.Warp(randomHit.position);

                    if (showDebug)
                    {
                        Debug.Log($"✅ Zombie movido a posición alternativa con ruta válida");
                    }
                    return;
                }
            }
        }

        // Método 3: Si todo falló, resetear ruta y esperar
        if (agent.hasPath)
        {
            agent.ResetPath();

            if (showDebug)
            {
                Debug.Log($"🔄 Ruta reseteada - Zombie buscará nuevo camino");
            }
        }
    }

    void TeleportToValidPosition()
    {
        // Buscar la posición válida del NavMesh más cercana
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);

            if (showDebug)
            {
                Debug.LogWarning($"⚠️ Zombie teletransportado a posición válida del NavMesh");
            }
        }
        else
        {
            Debug.LogError($"❌ No se encontró posición válida del NavMesh cerca de {gameObject.name}");
        }
    }

    void RecalculatePathPeriodically()
    {
        if (target == null || agent.isStopped) return;

        // Solo recalcular si ha pasado suficiente tiempo
        if (Time.time < lastPathRecalculation + pathRecalculationInterval) return;

        // Solo recalcular si hay distancia suficiente al objetivo
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget < minDistanceToRecalculate) return;

        // Recalcular ruta
        if (!agent.pathPending && agent.isOnNavMesh)
        {
            agent.SetDestination(target.position);
            lastPathRecalculation = Time.time;

            if (showDebug && Time.frameCount % 60 == 0)
            {
                Debug.Log($"🔄 Ruta recalculada hacia el jugador");
            }
        }
    }

    /// <summary>
    /// Método público para establecer un nuevo objetivo
    /// </summary>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        if (agent != null && agent.enabled && agent.isOnNavMesh)
        {
            agent.SetDestination(target.position);
        }
    }

    /// <summary>
    /// Forzar desatascar manualmente
    /// </summary>
    public void ForceUnstuck()
    {
        unstuckAttempts = 0;
        Unstuck();
    }

    void OnDrawGizmosSelected()
    {
        if (!drawPath || agent == null || !agent.hasPath) return;

        // Dibujar la ruta del NavMesh
        Gizmos.color = Color.yellow;
        Vector3[] path = agent.path.corners;

        for (int i = 0; i < path.Length - 1; i++)
        {
            Gizmos.DrawLine(path[i], path[i + 1]);
            Gizmos.DrawSphere(path[i], 0.2f);
        }

        // Dibujar destino
        if (path.Length > 0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(path[path.Length - 1], 0.3f);
        }
    }
}
