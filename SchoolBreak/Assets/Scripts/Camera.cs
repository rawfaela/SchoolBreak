using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;
    public float distance = 16.0f;
    public float height = 16.0f;
    public float rotationSpeed = 5.0f;
    public float smoothSpeed = 0.125f;

    private float currentRotation = 0f;

    //perguntas
    public Player playerScript;

    void LateUpdate()
    {
        if (!playerScript.isCollidingObstacle)
        {
            float horizontalInput = Input.GetAxis("Mouse X");
            currentRotation += horizontalInput * rotationSpeed;
        }

        Quaternion rotation = Quaternion.Euler(-30, currentRotation, 0);
        Vector3 direction = new Vector3(0, 0, -distance);
        Vector3 rotatedDirection = rotation * direction;
        Vector3 desiredPosition = target.position + rotatedDirection + Vector3.up * height;

        float currentDistance = direction.magnitude;
        Ray ray = new Ray(target.position + Vector3.up * 1.5f, direction.normalized);
        if (Physics.Raycast(ray, out RaycastHit hit, currentDistance))
        {
            if (hit.collider.CompareTag("School"))
            {
                desiredPosition = hit.point - direction.normalized * 0.2f;
            }
        }
        

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target.position + Vector3.up * 2f);
    }
}


//using UnityEngine;

//public class Camera : MonoBehaviour
//{
//    public Transform target;
//    public float distance = 16.0f;
//    public float height = 16.0f;
//    public float rotationSpeed = 5.0f;
//    public float smoothSpeed = 0.125f;

//    private float currentRotation = 0f;

//    public Player playerScript;

//    void LateUpdate()
//    {
//        // Rotação da câmera
//        if (!playerScript.isCollidingObstacle)
//        {
//            float horizontalInput = Input.GetAxis("Mouse X");
//            currentRotation += horizontalInput * rotationSpeed;
//        }

//        Quaternion rotation = Quaternion.Euler(-30, currentRotation, 0);
//        Vector3 direction = new Vector3(0, 0, -distance);
//        Vector3 rotatedDirection = rotation * direction;

//        Vector3 desiredPosition = target.position + rotatedDirection + Vector3.up * height;

//        Vector3 rayOrigin = target.position + Vector3.up * 2f;
//        Vector3 rayDirection = (desiredPosition - rayOrigin).normalized;

//        float adjustedDistance = distance;
//        RaycastHit hit;

//        float cameraRadius = 0.5f; // raio da câmera para não atravessar cantos ou teto
//        if (Physics.SphereCast(rayOrigin, cameraRadius, rayDirection, out hit, distance))
//        {
//            adjustedDistance = hit.distance - 0.5f;
//            adjustedDistance = Mathf.Clamp(adjustedDistance, 2.0f, distance);

//            rotatedDirection = rotation * new Vector3(0, 0, -adjustedDistance);
//            desiredPosition = target.position + rotatedDirection + Vector3.up * height;
//        }

//        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
//        transform.position = smoothedPosition;

//        transform.LookAt(target.position + Vector3.up * 2f);
//    }
//}
