using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;
    public float distance = 15.0f;
    public float height = 16.0f;
    public float rotationSpeed = 5.0f;
    public float smoothSpeed = 0.125f;

    private float currentRotation = 0f;

    public Player playerScript;

    void LateUpdate()
    {
        // Verifica��es de seguran�a
        if (target == null || playerScript == null) return;

        // Rota��o da c�mera
        if (!playerScript.isCollidingObstacle)
        {
            float horizontalInput = Input.GetAxis("Mouse X");
            currentRotation += horizontalInput * rotationSpeed;
        }

        Quaternion rotation = Quaternion.Euler(-50, currentRotation, 0);
        Vector3 direction = new Vector3(0, 0, -distance);
        Vector3 rotatedDirection = rotation * direction;
        Vector3 desiredPosition = target.position + rotatedDirection + Vector3.up * height;

        // Detec��o de colis�o simples - do jogador para a posi��o desejada da c�mera
        Vector3 rayOrigin = target.position + Vector3.up * 1.5f;
        Vector3 rayDirection = (desiredPosition - rayOrigin).normalized;
        float rayDistance = Vector3.Distance(rayOrigin, desiredPosition);

        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, rayDistance))
        {
            // Se colidir, posicionar a c�mera um pouco antes do ponto de colis�o
            desiredPosition = hit.point - rayDirection * 0.5f;
        }

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target.position + Vector3.up * 2f);
    }
}