using UnityEngine;

public class Enemy : MonoBehaviour
{
    CharacterController controller;
    Animator anim;

    public Transform player;
    public float moveSpeed = 3f;
    public float stoppingDistance = 2f;
    public float rotationSpeed = 5f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        Move();
    }
  
    void Move()
    {
        if (Vector3.Distance(transform.position, player.position) > stoppingDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Rotate(direction);
            controller.Move(direction * moveSpeed * Time.deltaTime);

            anim.SetInteger("transition", 1);  
        }
        else
        {
            anim.SetInteger("transition", 0);  
        }
    }
    void Rotate(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
