using UnityEngine;
using TMPro;

public class PlayerItemCollector : MonoBehaviour
{
    public int collected = 0;
    public int required = 5;

    public float collectDistance = 2f;
    public LayerMask itemLayer;

    public TextMeshProUGUI uiText;  // ← AQUÍ SE CONECTA EL TEXTO

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        Collider[] items = Physics.OverlapSphere(transform.position, collectDistance, itemLayer);

        foreach (Collider item in items)
        {
            collected++;
            Destroy(item.gameObject);
            UpdateUI();

            if (collected >= required)
            {
                uiText.text = "¡Completado!";
                Debug.Log("Nivel completado 🎉");
                Time.timeScale = 0;
            }
        }
    }

    void UpdateUI()
    {
        uiText.text = collected + "/" + required;  // ← AQUÍ SE ACTUALIZA EL 0/5
    }
}
