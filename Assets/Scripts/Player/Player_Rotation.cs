using UnityEngine;

public class Player_Rotation : MonoBehaviour
{
    public float mouseSensitivity = 200f;
    public bool rotateSettingWithMouse;

    private float yRotation = 0f;

    [SerializeField] private Transform visualTilt;
    [SerializeField] private Transform cameraTilt;

    [SerializeField] float tiltAmount;
    [SerializeField] float tiltSmooth = 5f;

    public Rigidbody rb;



    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleDroneTilt();

        if (rotateSettingWithMouse)
        {
            RotateWithMouse();
        }

        if (!rotateSettingWithMouse)
        {
            RotateWithKeyboard();
        }
    }
    void RotateWithMouse()
    {
        float mouseX = Input.GetAxis("Mouse X");
        yRotation = mouseX * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up, yRotation);
    }

    void RotateWithKeyboard()
    {
        float yRotation = 0f;
        float rotationSpeed = 50f; 

        if (Input.GetKey(KeyCode.Q))
        {
            yRotation = -1f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            yRotation = 1f;
        }

        if (Mathf.Abs(yRotation) > 0f)
        {
            transform.Rotate(Vector3.up, yRotation * rotationSpeed * Time.deltaTime);
        }
    }

    private void HandleDroneTilt()
    {

        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity); // Convierte un vector del espacio global (mundo) a local del objeto.

        float pitch = localVelocity.z * tiltAmount; // Inclinación hacia adelante/atrás
        float roll = -localVelocity.x * tiltAmount; // Inclinación hacia los costados 

        pitch = Mathf.Clamp(pitch, -tiltAmount, tiltAmount);
        roll = Mathf.Clamp(roll, -tiltAmount, tiltAmount);

        Quaternion targetTilt = Quaternion.Euler(pitch, 0f, roll);

        visualTilt.localRotation = Quaternion.Slerp(visualTilt.localRotation, targetTilt, Time.deltaTime * tiltSmooth);

        cameraTilt.localRotation = Quaternion.Slerp(cameraTilt.localRotation, targetTilt, Time.deltaTime * tiltSmooth);

    }
}
