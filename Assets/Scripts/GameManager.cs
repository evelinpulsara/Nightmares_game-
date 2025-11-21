using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int objetosRecolectados = 0;
    public int totalObjetos = 5; // ← ajusta si no son 5 ositos

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void SumarObjeto()
    {
        objetosRecolectados++;
        Debug.Log($"🧸 Recolectado: {objetosRecolectados}/{totalObjetos}");

        if (objetosRecolectados >= totalObjetos)
        {
            Debug.Log("✅ Nivel completado");
            // Evita llamadas múltiples
            objetosRecolectados = int.MaxValue;
            // Inicia la transición
            StartCoroutine(CargarEscenaAsilo());
        }
    }

    System.Collections.IEnumerator CargarEscenaAsilo()
    {
        yield return new WaitForSeconds(2f); // ⏳ 2 segundos
        SceneManager.LoadScene("Jesus_SampleScene"); // 🚪 escena de Camila
    }
}