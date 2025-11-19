using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float health = 100f;
    public Slider healthBar; // Asigna una barra en UI

    private bool isDead = false;

    void Start()
    {
        health = Mathf.Clamp(health, 0f, maxHealth);
        if (healthBar != null)
        {
            healthBar.minValue = 0f;
            healthBar.maxValue = maxHealth;
            UpdateHealthUI();
        }
        else
        {
            Debug.LogWarning("PlayerHealth: 'healthBar' no asignado en el Inspector");
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return; // No recibir daño si ya está muerto

        health = Mathf.Clamp(health - amount, 0f, maxHealth);
        Debug.Log("Player Health: " + health);
        if (healthBar != null)
        {
            UpdateHealthUI();
        }
        else
        {
            Debug.LogWarning("PlayerHealth: 'healthBar' no asignado en el Inspector");
        }
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return; // Evitar llamar Die() múltiples veces

        isDead = true;
        Debug.Log("💀 PLAYER DEAD");

        // Activar Game Over si existe el manager
        if (GameOverManager.Instance != null)
        {
            GameOverManager.Instance.ShowGameOver();
        }
        else
        {
            Debug.LogWarning("⚠️ GameOverManager no encontrado en la escena. Asegúrate de tener un GameObject con el script GameOverManager.");
        }
    }

    private void UpdateHealthUI()
    {
        if (healthBar == null) return;
        healthBar.value = Mathf.Clamp(health, 0f, maxHealth);
    }
}
