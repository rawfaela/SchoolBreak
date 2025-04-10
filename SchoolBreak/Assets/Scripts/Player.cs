using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public float Speed = 10f;
    public float RotSpeed = 500f;
    private float Rotation;
    public float Gravity = 10f;

    Vector3 MoveDirection;
    CharacterController controller;
    Animator anim;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Rotate();
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
        float mouseX = Input.GetAxis("Mouse X") * RotSpeed * Time.deltaTime;

        transform.Rotate(0, mouseX * RotSpeed * Time.deltaTime, 0);
    }
}
