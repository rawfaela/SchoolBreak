using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;
    public float distance = 16.0f;
    public float height = 16.0f;
    public float rotationSpeed = 5.0f;
    public float smoothSpeed = 0.125f;
    public LayerMask collisionLayers; // camadas com obst�culos

    private float currentRotation = 0f;

    // Refer�ncia ao script do jogador
    public Player playerScript;

    void LateUpdate()
    {
        if (!playerScript.isCollidingObstacle)
        {
            float horizontalInput = Input.GetAxis("Mouse X");
            currentRotation += horizontalInput * rotationSpeed;
        }

        // C�lculo da rota��o e dire��o da c�mera
        Quaternion rotation = Quaternion.Euler(-30, currentRotation, 0);
        Vector3 direction = new Vector3(0, 0, -distance);
        Vector3 rotatedDirection = rotation * direction;
        Vector3 targetPosition = target.position + Vector3.up * height;

        // Posi��o ideal da c�mera (sem colis�o)
        Vector3 desiredCameraPos = target.position + rotatedDirection + Vector3.up * height;

        // Verifica colis�o entre o personagem e a posi��o desejada da c�mera
        RaycastHit hit;
        if (Physics.Raycast(targetPosition, rotatedDirection.normalized, out hit, distance, collisionLayers))
        {
            desiredCameraPos = hit.point - rotatedDirection.normalized * 0.5f; // recuo para evitar ficar grudado
        }

        // Suaviza o movimento da c�mera
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredCameraPos, smoothSpeed);
        transform.position = smoothedPosition;

        // Olhar para o personagem
        transform.LookAt(target.position + Vector3.up * 2f);
    }
}
