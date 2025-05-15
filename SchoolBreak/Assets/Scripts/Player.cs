using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    public bool isCollidingObstacle = false;
    private int correctAnswerIndex = -1;
    private float questionTimer = 0f;
    private bool questionActive = false;
    private float extraTimeFromClocks = 0f;
    public TMP_Text questionTimerText;
    public TMP_Text ErrorsText;
    public int contErrors = 0;
    private Coroutine questionCoroutine; //tempo tava contando super rapido quando pegava o relogio
    private Collider obstacleCollider; //p desativar o collider do obstaculo dps de acertar a pergunta

    public ChangeScenes changeScenes;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        question.gameObject.SetActive(false);

        foreach (Button btn in optionButtons)
        {
            btn.onClick.RemoveAllListeners();
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!isCollidingObstacle)
        {
            Move();
            Rotate();
        }

        if (contErrors == 3)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            changeScenes.SceneGameOver();
            
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

    private void OnTriggerEnter(Collider other)
    {
        //perguntas
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            questionTimer = 25f + extraTimeFromClocks;
            extraTimeFromClocks = 0f; //reseta pq ja foi usado
            questionActive = true;

            if (obstacleCollider != null)
            {
                obstacleCollider.enabled = true; //ativa o collider do ultimo obstaculo (se tiver)
            }
            obstacleCollider = other; //salva o collider desse

            if (questionCoroutine != null) //tempo tava contando super rapido quando pegava o relogio
            {
                StopCoroutine(questionCoroutine);
            }
            questionCoroutine = StartCoroutine(QuestionTime());

            question.gameObject.SetActive(true);
            isCollidingObstacle = true;
            anim.SetInteger("transition", 0);

            Questions info = other.GetComponent<Questions>();
            if (info != null)
            {
                var q = info.questionData;
                questionText.text = q.question;
                correctAnswerIndex = q.correctOptionIndex;

                for (int i = 0; i < optionTexts.Length; i++)
                {
                    optionTexts[i].text = q.options[i];

                    int index = i;
                    optionButtons[i].onClick.RemoveAllListeners();
                    optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
                }
            }
        }

        if (other.gameObject.CompareTag("Boost"))
        {
            StartCoroutine(BoostSpeed(2.5f, 5));
        }

        if (other.gameObject.CompareTag("Clock"))
        {
            extraTimeFromClocks += 5f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            isCollidingObstacle = false;
            question.gameObject.SetActive(false);
        }
    }

    //boost velocidade quando player pega tenis 
    private IEnumerator BoostSpeed(float multiplier, float duration)
    {
        Speed *= multiplier;
        yield return new WaitForSeconds(duration);
        Speed /= multiplier;
    }

    //tempo da pergunta com e sem boost
    private IEnumerator QuestionTime()
    {
        while (questionTimer > 0f)
        {
            questionTimer -= Time.deltaTime;
            questionTimerText.text = $"{Mathf.CeilToInt(questionTimer)}s";
            yield return null;
        }

        if (questionActive)
        {
            question.gameObject.SetActive(false);
            isCollidingObstacle = false;
            questionActive = false;
        }
        questionTimerText.text = "";
    }

    //perguntas
    void OnOptionSelected(int selectedIndex)
    {
        if (selectedIndex == correctAnswerIndex)
        {
            Debug.Log("Resposta correta!");
            optionButtons[selectedIndex].image.color = Color.green;
            StartCoroutine(CloseQuestion(1f));
            obstacleCollider.GetComponent<Collider>().enabled = false;
        }
        else
        {
            Debug.Log("Resposta incorreta!");
            optionButtons[selectedIndex].image.color = Color.red;
            StartCoroutine(CloseQuestion(1f));
            ErrorsText.text = $"{ErrorsText.text}X";
            contErrors += 1;  //QUANDO CHEGAR A 3 VOLTAR DETENÇÃO
        }
    }

    IEnumerator CloseQuestion(float delay)
    {
        yield return new WaitForSeconds(delay);

        //reseta a cor dos botões
        foreach (var btn in optionButtons)
        {
            btn.image.color = Color.white;
        }

        question.gameObject.SetActive(false);
        isCollidingObstacle = false;
        questionActive = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    //comentar: ctrl k ctrl c  descomentar: ctrl k ctrl u
}
