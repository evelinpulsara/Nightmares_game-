using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement1 : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 2f;

    CharacterController controller;
    Vector3 velocity;
    float verticalRotation = 0f;
    public Transform cam; // arrastra la cámara hijo aquí

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // --- movimiento ---
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // --- gravedad simple ---
        if (!controller.isGrounded)
            velocity.y += gravity * Time.deltaTime;
        else
            velocity.y = -1f; // ligero empuje hacia abajo para estabilizar

        controller.Move(velocity * Time.deltaTime);

        // --- mouse look ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f);
        cam.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
}
