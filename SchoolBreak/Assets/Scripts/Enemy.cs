using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    CharacterController controller;
    Animator anim;

    public Transform player;
    public float moveSpeed = 3f;
    public float stoppingDistance = 2f;
    public float rotationSpeed = 5f;
    private bool isAttacking = false;

    //perguntas
    public Player playerScript;

    public ChangeScenes changeScenes;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (playerScript.isCollidingObstacle)
        {
            anim.SetInteger("transition", 0);
        }
        else  
        { 
            if (distance > stoppingDistance)
            {
                isAttacking = false;
                Move();
            }
            else
            {
                if (!isAttacking)
                {
                    Attack();
                }
            }
        }
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
    void Attack()
    {
        isAttacking = true;
        anim.SetInteger("transition", 2);
        StartCoroutine(WaitAnimation());
    }
    IEnumerator WaitAnimation()
    {
        yield return new WaitForSeconds(1.5f); //espera a anima��o
        changeScenes.SceneGameOver();
    }
}
