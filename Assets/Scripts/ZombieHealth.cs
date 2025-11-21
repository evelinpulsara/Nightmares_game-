using UnityEngine;
using UnityEngine.UI;

public class ZombieHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth; // ← mejor 'private' (no necesitas modificarlo desde el inspector)

    [Header("UI")]
    public Slider healthBar; // ← arrastra aquí el Slider en el Inspector

    void Start()
    {
        // Asegúrate de que maxHealth sea positivo
        if (maxHealth <= 0) maxHealth = 1;

        currentHealth = maxHealth;

        // Actualiza la barra de salud al inicio (incluso si es null, no rompe)
        UpdateHealthBar();
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0) return; // Evita daño cero o negativo (mejora de seguridad)

        currentHealth -= amount;

        // Actualiza la UI
        UpdateHealthBar();

        // Verifica muerte
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        // Solo actualiza si el slider está asignado
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth; // ← opcional, pero útil si cambias maxHealth dinámicamente
            healthBar.value = currentHealth;
            // Unity Slider acepta valores enteros directamente; no necesitas normalizar si usas max = maxHealth
        }
    }

    void Die()
    {
        // 👉 Recomendado: desactivar collider & renderer antes de destruir (evita glitches visuales)
        var rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true; // evita fuerzas residuales

        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false; 

        // Destruir después de un pequeño delay (opcional, si quieres animación de muerte)
        Destroy(gameObject);
    }
}