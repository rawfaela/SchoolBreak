using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;
    public float distance = 15.0f;
    public float height = 16.0f;
    public float rotationSpeed = 5.0f;
    public float smoothSpeed = 0.125f;

    private float currentRotation = 0f;
    private Vector3 velocity = Vector3.zero;

    public Player playerScript;

    void LateUpdate()
    {
        if (target == null || playerScript == null) return;

        // Rotação da câmera com o mouse (desativada quando em pergunta)
        if (!playerScript.isCollidingObstacle)
        {
            float horizontalInput = Input.GetAxis("Mouse X");
            currentRotation += horizontalInput * rotationSpeed;
        }

        Quaternion rotation = Quaternion.Euler(-50, currentRotation, 0);
        Vector3 direction = new Vector3(0, 0, -distance);
        Vector3 rotatedDirection = rotation * direction;
        Vector3 desiredPosition = target.position + rotatedDirection + Vector3.up * height;

        Vector3 rayOrigin = target.position + Vector3.up * 1.5f;
        Vector3 rayDirection = (desiredPosition - rayOrigin).normalized;
        float rayDistance = Vector3.Distance(rayOrigin, desiredPosition);

        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, rayDistance))
        {
            desiredPosition = hit.point - rayDirection * 0.5f;
        }

        float currentSmoothSpeed = playerScript.Speed > 10f ? 0.1f : smoothSpeed;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, currentSmoothSpeed);

        transform.LookAt(target.position + Vector3.up * 2f);
    }
}
