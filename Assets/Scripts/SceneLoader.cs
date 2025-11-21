using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Necesario para IEnumerator

public class SceneLoader : MonoBehaviour
{
    // Desde el menú principal → carga inmediatamente
    public void LoadGameScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // Desde Game Over → carga con retraso (opcional)
    public void LoadGameSceneWithDelay()
    {
        StartCoroutine(LoadAfterDelay(2f)); // 2 segundos de espera
    }

    IEnumerator LoadAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("SampleScene");
    }
}