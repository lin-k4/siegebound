using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float cameraSpeed = 5f;
    public float rotationSpeed = 100f;

    //handles inputs for camera control
    void Update()
    {
        float rotationInput = 0f;
        if (Input.GetKey(KeyCode.Q)) rotationInput -= 1f;
        if (Input.GetKey(KeyCode.E)) rotationInput += 1f;

        transform.Rotate(Vector3.up, rotationInput * rotationSpeed * Time.deltaTime, Space.World);

        float pitch = 0f;
        if (Input.GetKey(KeyCode.C)) pitch -= 1f;
        if (Input.GetKey(KeyCode.Z)) pitch += 1f;
        transform.Rotate(Vector3.right, pitch * rotationSpeed * Time.deltaTime, Space.Self);

        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = (transform.forward * vertical) + (transform.right * horizontal);

        float verticalFly = 0f;
        if (Input.GetKey(KeyCode.Space))
        {
            verticalFly += 1f;
        }
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            verticalFly -= 1f;
        }
        moveDir.y = verticalFly;

        transform.position += moveDir.normalized * cameraSpeed * Time.deltaTime;
    }
}
