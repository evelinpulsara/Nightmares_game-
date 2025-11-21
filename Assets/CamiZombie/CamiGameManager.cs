using UnityEngine;
using UnityEngine.SceneManagement;

public class CamiGameManager : MonoBehaviour
{
    public static CamiGameManager instance;
    public int score = 0;
    public int scoreToWin = 4;
    public void AddPoints(int points) => AddScore(points);
    public void IncrementScore(int points) => AddScore(points);

    private void Awake()
    {
        if (instance == null)
        {
           instance = this;
           DontDestroyOnLoad(gameObject);  // opcional, para persistir entre escenas
        }
        else
       {
           Destroy(gameObject);
       }
    }

    public void AddScore(int value)
    {
        score += value;
        Debug.Log("Puntaje: " + score);

        if (score >= scoreToWin)
        {
            Debug.Log("Nivel completado ✅");
            SceneManager.LoadScene("World_2_Asylum"); // siguiente nivel
        }
    }
}
