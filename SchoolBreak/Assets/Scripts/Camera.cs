using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;
    public float distance = 16.0f;
    public float height = 16.0f;
    public float rotationSpeed = 5.0f;
    public float smoothSpeed = 0.125f;

    private float currentRotation = 0f;

    void LateUpdate()
    {
        float horizontalInput = Input.GetAxis("Mouse X");
        currentRotation += horizontalInput * rotationSpeed;

        Quaternion rotation = Quaternion.Euler(-30, currentRotation, 0);
        Vector3 direction = new Vector3(0, 0, -distance);
        Vector3 rotatedDirection = rotation * direction;

        Vector3 desiredPosition = target.position + rotatedDirection + Vector3.up * height;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target.position + Vector3.up * 2f); // Leve ajuste para olhar mais ao centro do corpo
    }
}
