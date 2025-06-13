using UnityEngine;

public class Item : MonoBehaviour
{
    // Velocidade de rotação (em graus por segundo) nos eixos X, Y e Z
    public Vector3 rotationSpeed = new Vector3(0f, 100f, 0f);

    // Som que será tocado ao pegar o item
    public AudioClip pickupSound;

    void Update()
    {
        // Rotaciona o objeto com base na velocidade e no tempo entre os frames
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            // Toca o som na posição do item
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            // Aqui você pode adicionar pontuação, efeitos, etc.
            Destroy(gameObject); // Remove o item da cena
        }
    }
}