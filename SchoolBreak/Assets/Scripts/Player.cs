using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public TMP_Text questionText;
    public TMP_Text[] optionTexts; 
    public Button[] optionButtons;
    public bool isColliding = false;

    private int correctAnswerIndex = -1;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        question.gameObject.SetActive(false);

        foreach (Button btn in optionButtons)
        {
            btn.onClick.RemoveAllListeners();
        }
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
            question.gameObject.SetActive(true);
            isColliding = true;
            anim.SetInteger("transition", 0);

            Questions info = obstacle.GetComponent<Questions>();
            if (info != null)
            {
                var q = info.questionData;
                questionText.text = q.question;
                correctAnswerIndex = q.correctOptionIndex;

                for (int i = 0; i < optionTexts.Length; i++)
                {
                    optionTexts[i].text = q.options[i];

                    int index = i; // Captura o índice para o listener
                    optionButtons[i].onClick.RemoveAllListeners();
                    optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            isColliding = false;
            question.gameObject.SetActive(false);
        }
    }

    void OnOptionSelected(int selectedIndex)
    {
        if (selectedIndex == correctAnswerIndex)
        {
            Debug.Log("Resposta correta!");
            // Você pode fazer algo como dar pontos, liberar caminho, etc.
        }
        else
        {
            Debug.Log("Resposta incorreta!");
            // Penalidade, aviso, etc.
        }

        // Fecha o canvas após resposta
        question.gameObject.SetActive(false);
        isColliding = false;
    }
}
