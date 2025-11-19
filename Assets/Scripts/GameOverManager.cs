using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla la pantalla de Game Over
/// - Muestra UI cuando el jugador muere
/// - Permite reiniciar o volver al menú
/// - Pausa el juego durante Game Over
/// </summary>
public class GameOverManager : MonoBehaviour
{
    [Header("Referencias UI")]
    [Tooltip("Canvas completo de Game Over")]
    public GameObject gameOverCanvas;

    [Tooltip("Texto opcional que muestra 'GAME OVER'")]
    public Text gameOverText;

    [Tooltip("Botón para reiniciar nivel")]
    public Button restartButton;

    [Tooltip("Botón para volver al menú principal")]
    public Button mainMenuButton;

    [Tooltip("Botón para salir del juego")]
    public Button quitButton;

    [Header("Configuración")]
    [Tooltip("Nombre de la escena del menú principal")]
    public string mainMenuSceneName = "MainMenu";

    [Tooltip("Pausar el juego en Game Over (Time.timeScale = 0)")]
    public bool pauseGameOnDeath = true;

    [Tooltip("Desactivar controles del jugador al morir")]
    public bool disablePlayerControls = true;

    [Header("Audio")]
    [Tooltip("Sonido de Game Over (opcional)")]
    public AudioClip gameOverSound;

    private AudioSource audioSource;
    private bool isGameOver = false;

    // Singleton para acceso global
    public static GameOverManager Instance { get; private set; }

    void Awake()
    {
        // Configurar singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Configurar AudioSource si hay sonido
        if (gameOverSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    void Start()
    {
        // Ocultar Canvas al inicio
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }

        // Configurar botones
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartLevel);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(GoToMainMenu);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }

        // Asegurar que el tiempo está normal al inicio
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Mostrar pantalla de Game Over
    /// </summary>
    public void ShowGameOver()
    {
        if (isGameOver) return; // Evitar llamadas múltiples

        isGameOver = true;

        // Mostrar Canvas
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }

        // Reproducir sonido
        if (gameOverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(gameOverSound);
        }

        // Pausar juego
        if (pauseGameOnDeath)
        {
            Time.timeScale = 0f;
        }

        // Desactivar controles del jugador
        if (disablePlayerControls)
        {
            DisablePlayerControls();
        }

        // Mostrar cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("🎮 GAME OVER - Juego pausado");
    }

    /// <summary>
    /// Reiniciar el nivel actual
    /// </summary>
    public void RestartLevel()
    {
        Debug.Log("🔄 Reiniciando nivel...");

        // Restaurar timeScale antes de cargar escena
        Time.timeScale = 1f;

        // Recargar escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Ir al menú principal
    /// </summary>
    public void GoToMainMenu()
    {
        Debug.Log("🏠 Volviendo al menú principal...");

        // Restaurar timeScale antes de cargar escena
        Time.timeScale = 1f;

        // Cargar escena del menú
        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            Debug.LogWarning("⚠️ No se especificó el nombre de la escena del menú principal");
            SceneManager.LoadScene(0); // Cargar primera escena (índice 0)
        }
    }

    /// <summary>
    /// Salir del juego
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("👋 Saliendo del juego...");

        // Restaurar timeScale
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// Desactivar controles del jugador
    /// </summary>
    private void DisablePlayerControls()
    {
        // Buscar el jugador
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        // Desactivar scripts de control comunes
        var firstPersonController = player.GetComponent<MonoBehaviour>();
        if (firstPersonController != null)
        {
            // Desactivar todos los MonoBehaviour del jugador (excepto este script)
            foreach (var script in player.GetComponents<MonoBehaviour>())
            {
                if (script != null && script.GetType().Name != "PlayerHealth")
                {
                    script.enabled = false;
                }
            }
        }

        // Desactivar cámara de primera persona si existe
        Camera playerCamera = player.GetComponentInChildren<Camera>();
        if (playerCamera != null)
        {
            var cameraController = playerCamera.GetComponent<MonoBehaviour>();
            if (cameraController != null)
            {
                cameraController.enabled = false;
            }
        }
    }

    /// <summary>
    /// Ocultar Game Over (útil para testing)
    /// </summary>
    public void HideGameOver()
    {
        isGameOver = false;

        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDestroy()
    {
        // Asegurar que el tiempo vuelva a la normalidad al destruir
        Time.timeScale = 1f;
    }
}
