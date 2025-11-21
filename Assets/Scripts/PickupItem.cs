using UnityEngine;

public class PickupItem : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.SumarObjeto();
            Destroy(gameObject);
        }
    }
}
    