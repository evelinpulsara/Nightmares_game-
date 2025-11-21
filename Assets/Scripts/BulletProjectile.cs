using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    public int damage = 20;
    public float lifeTime = 4f;
    public float speed = 50f;
    public GameObject owner; // opcional: quien disparó para ignorar colisión

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);

        // mueve la bala (si quieres forzar velocidad inicial)
        rb.velocity = transform.forward * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Ignorar colisión con quien disparó (si owner tiene collider)
        if (owner != null)
        {
            Collider ownerCol = owner.GetComponent<Collider>();
            if (ownerCol != null)
            {
                Physics.IgnoreCollision(collision.collider, ownerCol);
            }
        }

        // Buscar ZombieHealth en el objeto golpeado o en sus padres
        ZombieHealth zh = collision.collider.GetComponent<ZombieHealth>();
        if (zh == null)
            zh = collision.collider.GetComponentInParent<ZombieHealth>();

        if (zh != null)
        {
            zh.TakeDamage(damage);
        }

        // Opcional: crear efecto de impacto aquí

        Destroy(gameObject);
    }
}
