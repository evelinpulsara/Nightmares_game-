using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject bulletPrefab;    // arrastra prefab Bullet
    public Transform firePoint;        // arrastra FirePoint
    public Camera cam;                 // arrastra la Camera del player
    public float bulletSpeed = 50f;
    public float fireRate = 0.25f;
    private float nextFire = 0f;
    public GameObject owner; // opcional: arrastra el Player para ignorar colisiones

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFire)
        {
            nextFire = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // Ray desde el cursor para saber hacia dónde apuntar
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit, 1000f))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * 100f; // punto lejano
        }

        // Instanciar bala en firePoint, orientada hacia targetPoint
        Vector3 direction = (targetPoint - firePoint.position).normalized;
        Quaternion rot = Quaternion.LookRotation(direction);
        GameObject b = Instantiate(bulletPrefab, firePoint.position, rot);

        // configurar el owner y velocidad
        BulletProjectile bp = b.GetComponent<BulletProjectile>();
        if (bp != null)
        {
            bp.owner = owner;
            bp.speed = bulletSpeed;
            // la velocidad en Start() del bullet ya aplicará rb.velocity
        }
    }
}
