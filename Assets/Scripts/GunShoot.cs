using UnityEngine;

public class GunShoot : MonoBehaviour
{
    [Header("Balística")]
    public GameObject bulletPrefab;       // ← Arrastra el prefab de la bala aquí (Inspector)
    public Transform firePoint;           // ← Arrastra el punto de disparo (ej: punta del arma) aquí
    public float bulletForce = 30f; // ✅ Cambia de 15f → 30f (o hasta 50f si el zombie está lejos)

    [Header("Daño")]
    public int damage = 20;

    [Header("Audio")]
    public AudioSource shootSound;        // ← Arrastra tu AudioSource aquí (con clip asignado)

    [Header("Cámara (opcional para debug)")]
    public Camera cam;                    // Solo para Debug.Log (opcional)

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // ✅ Reproducir sonido primero (da mejor feedback)
        if (shootSound != null)
            shootSound.Play();

        // ✅ Validar que los campos obligatorios estén asignados
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogError("¡bulletPrefab o firePoint no asignados en GunShoot!");
            return;
        }

        // ✅ Instanciar bala
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Aplicar fuerza si la bala tiene Rigidbody
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
        }
    }
}