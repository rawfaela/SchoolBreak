using UnityEngine;

public class Item : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0f, 100f, 0f);

    public AudioClip pickupSound;

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }
            Destroy(gameObject);
        }
    }
}
