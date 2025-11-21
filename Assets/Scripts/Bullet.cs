using UnityEngine;

public class Bullet : MonoBehaviour
{
    // ✅ Cambiado a 'int' porque el daño suele ser entero (10, 20, 50...)
    public int damage = 10;

    public float destroyTime = 3f;

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Si choca con el zombie
        if (collision.gameObject.CompareTag("Zombie"))
        {
            ZombieHealth zombie = collision.gameObject.GetComponent<ZombieHealth>();
            if (zombie != null)
            {
                zombie.TakeDamage(damage); // ✅ Ahora ambos son 'int' → ¡sin error!
            }
        }

        // Destruye la bala al chocar (incluso si no impacta en zombie)
        Destroy(gameObject);
    }
}