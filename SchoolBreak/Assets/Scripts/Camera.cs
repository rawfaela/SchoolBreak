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
        // Verificações de segurança
        if (target == null || playerScript == null) return;

        // Rotação da câmera
        if (!playerScript.isCollidingObstacle)
        {
            float horizontalInput = Input.GetAxis("Mouse X");
            currentRotation += horizontalInput * rotationSpeed;
        }

        Quaternion rotation = Quaternion.Euler(-50, currentRotation, 0);
        Vector3 direction = new Vector3(0, 0, -distance);
        Vector3 rotatedDirection = rotation * direction;
        Vector3 desiredPosition = target.position + rotatedDirection + Vector3.up * height;

        // Detecção de colisão simples - do jogador para a posição desejada da câmera
        Vector3 rayOrigin = target.position + Vector3.up * 1.5f;
        Vector3 rayDirection = (desiredPosition - rayOrigin).normalized;
        float rayDistance = Vector3.Distance(rayOrigin, desiredPosition);

        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, rayDistance))
        {
            // Se colidir, posicionar a câmera um pouco antes do ponto de colisão
            desiredPosition = hit.point - rayDirection * 0.5f;
        }

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target.position + Vector3.up * 2f);
    }
}