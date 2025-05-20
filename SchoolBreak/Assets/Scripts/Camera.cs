using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;
    public float distance = 16.0f;
    public float height = 16.0f;
    public float rotationSpeed = 5.0f;
    public float smoothSpeed = 0.125f;
    public LayerMask collisionLayers; // camadas com obstáculos

    private float currentRotation = 0f;

    // Referência ao script do jogador
    public Player playerScript;

    void LateUpdate()
    {
        if (!playerScript.isCollidingObstacle)
        {
            float horizontalInput = Input.GetAxis("Mouse X");
            currentRotation += horizontalInput * rotationSpeed;
        }

        // Cálculo da rotação e direção da câmera
        Quaternion rotation = Quaternion.Euler(-30, currentRotation, 0);
        Vector3 direction = new Vector3(0, 0, -distance);
        Vector3 rotatedDirection = rotation * direction;
        Vector3 targetPosition = target.position + Vector3.up * height;

        // Posição ideal da câmera (sem colisão)
        Vector3 desiredCameraPos = target.position + rotatedDirection + Vector3.up * height;

        // Verifica colisão entre o personagem e a posição desejada da câmera
        RaycastHit hit;
        if (Physics.Raycast(targetPosition, rotatedDirection.normalized, out hit, distance, collisionLayers))
        {
            desiredCameraPos = hit.point - rotatedDirection.normalized * 0.5f; // recuo para evitar ficar grudado
        }

        // Suaviza o movimento da câmera
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredCameraPos, smoothSpeed);
        transform.position = smoothedPosition;

        // Olhar para o personagem
        transform.LookAt(target.position + Vector3.up * 2f);
    }
}
