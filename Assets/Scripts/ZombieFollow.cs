using UnityEngine;

public class ZombieFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 100f;
    public float damage = 10f;
    public float attackDistance = 1.5f;
    float attackCooldown = 1f;
    float nextAttackTime = 0f;

    void Update()
    {
        if (player == null) return;

        // Seguir al jugador
        transform.LookAt(player);
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        // Atacar cuando esté cerca
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist < attackDistance && Time.time > nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(damage);
        }
    }
}
