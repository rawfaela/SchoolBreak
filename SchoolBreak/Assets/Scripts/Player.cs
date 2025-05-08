using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public float Speed = 10f;
    public Transform cameraTransform;
    public float Gravity = 10f;

    Vector3 MoveDirection;
    CharacterController controller;
    Animator anim;

    //perguntas
    public Canvas question;
    public bool isColliding = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        question.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isColliding)
        {
            Move();
            Rotate();
        }
    }

    void Move()
    {
        if (controller.isGrounded)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 move = transform.right * horizontal + transform.forward * vertical;

            MoveDirection = move * Speed;

            if (vertical != 0 || horizontal != 0)  
            {
                anim.SetInteger("transition", 1); 
            }
            else
            {
                anim.SetInteger("transition", 0);
            }
        }
        else
        {
            MoveDirection.y -= Gravity * Time.deltaTime;
        }

        controller.Move(MoveDirection * Time.deltaTime);
    }
    void Rotate()
    {
        Vector3 lookDirection = cameraTransform.forward;
        lookDirection.y = 0f;

        if (lookDirection.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        };
    }

    private void OnTriggerEnter(Collider obstacle)
    {
        if (obstacle.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("tocou");
            question.gameObject.SetActive(true);
            isColliding = true;
            anim.SetInteger("transition", 0);
        }

    }
}
