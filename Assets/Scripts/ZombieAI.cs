using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public Transform target; 
    public float damage = 10f;
    public float attackDistance = 1.5f;
    public float attackCooldown = 1f;

    NavMeshAgent agent;
    float lastAttack = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        agent.SetDestination(target.position);

        float dist = Vector3.Distance(transform.position, target.position);

        if (dist <= attackDistance && Time.time > lastAttack)
        {
            target.GetComponent<PlayerHealth>().TakeDamage(damage);
            lastAttack = Time.time + attackCooldown;
        }
    }
}
