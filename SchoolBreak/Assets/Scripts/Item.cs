using UnityEngine;

public class Item : MonoBehaviour
{
    // Velocidade de rota��o (em graus por segundo) nos eixos X, Y e Z
    public Vector3 rotationSpeed = new Vector3(0f, 100f, 0f);

    // Som que ser� tocado ao pegar o item
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
            // Toca o som na posi��o do item
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            // Aqui voc� pode adicionar pontua��o, efeitos, etc.
            Destroy(gameObject); // Remove o item da cena
        }
    }
}