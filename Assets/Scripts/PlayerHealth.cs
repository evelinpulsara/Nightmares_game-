using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 👈 Necesario para cargar escenas

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public Slider healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (healthBar != null)
            healthBar.value = currentHealth;

        // ✅ Verificar si el jugador murió
        if (currentHealth <= 0)
        {
            Debug.Log("GAME OVER");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            // 🚀 Cargar la escena de Game Over
            SceneManager.LoadScene("GameOverScene");
        }
    }
}