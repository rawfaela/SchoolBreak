using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed = 10f;
    public Transform cameraTransform;
    public float Gravity = 10f;
    public float jumpForce = 3f;

    private Vector3 MoveDirection;
    private CharacterController controller;
    private Animator anim;

    public bool isCollidingObstacle = false;
    public ChangeScenes changeScenes;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!isCollidingObstacle)
        {
            Move();
            Rotate();
        }
    }

    private void Move()
    {
        if (controller.isGrounded)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            bool jump = Input.GetButtonDown("Jump");
            Vector3 move = transform.right * horizontal + transform.forward * vertical;
            MoveDirection = move * Speed;

            anim.SetInteger("transition", (vertical != 0 || horizontal != 0) ? 1 : 0);
            if (jump)
            {
                anim.SetInteger("transition", 2);
                MoveDirection.y = jumpForce;
            }
        }
        else
        {
            MoveDirection.y -= Gravity * Time.deltaTime;
        }

        controller.Move(MoveDirection * Time.deltaTime);
    }

    private void Rotate()
    {
        Vector3 lookDirection = cameraTransform.forward;
        lookDirection.y = 0f;

        if (lookDirection.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Questions question = other.GetComponent<Questions>();
            question.ShowQuestion(this);
        }

        if (other.CompareTag("Clock"))
        {
            Questions question = other.GetComponent<Questions>();
            if (question != null)
                question.AddExtraTime(5f);
        }

        if (other.CompareTag("Boost"))
        {
            StartCoroutine(BoostSpeed(2.5f, 5));
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            isCollidingObstacle = false;
        }
    }

    private IEnumerator BoostSpeed(float multiplier, float duration)
    {
        Speed *= multiplier;
        yield return new WaitForSeconds(duration);
        Speed /= multiplier;
    }
}
