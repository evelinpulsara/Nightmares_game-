using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int objetosRecolectados = 0;
    public int totalObjetos = 5;

    void Awake()
    {
        instance = this;
    }

    public void SumarObjeto()
    {
        objetosRecolectados++;

        if (objetosRecolectados >= totalObjetos)
        {
            Debug.Log("Nivel completado!!!");
            SceneManager.LoadScene("SiguienteNivel");
        }
    }
}
