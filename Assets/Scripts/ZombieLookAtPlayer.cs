using UnityEngine;

public class ZombieLookAtPlayer : MonoBehaviour
{
    public Transform player; // Asigna el jugador en el Inspector

    void Update()
    {
        if (player != null)
        {
            // Mira hacia el jugador (solo en el eje Y, para que no se incline)
            Vector3 direction = player.position - transform.position;
            direction.y = 0; // Ignorar altura para que no se incline
            if (direction.magnitude > 0.1f) // Evitar rotación cuando esté muy cerca
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
}