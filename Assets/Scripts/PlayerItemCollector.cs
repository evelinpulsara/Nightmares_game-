using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerItemCollector : MonoBehaviour
{
    public int collected = 0;
    public int required = 5;

    public float collectDistance = 2f;
    public LayerMask itemLayer;

    public TextMeshProUGUI uiText;

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        Collider[] items = Physics.OverlapSphere(transform.position, collectDistance, itemLayer);

        foreach (Collider item in items)
        {
            // Evita recolectar el mismo ítem múltiples veces en un frame
            if (item == null) continue;

            collected++;
            Destroy(item.gameObject);
            UpdateUI();

            // Avisa al GameManager (esto activa la transición si es el último)
            GameManager.instance?.SumarObjeto();

            // Si completaste, mostramos mensaje (pero la transición ya la maneja GameManager)
            if (collected >= required)
            {
                uiText.text = "¡Completado!";
                Debug.Log("🧸 Nivel completado 🎉");
            }

            break; // Recoge 1 ítem por frame para evitar saltos
        }
    }

    void UpdateUI()
    {
        uiText.text = collected + "/" + required;
    }
} 