using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class QuestionData
{
    public string question;
    public string[] options;
    public int correctOptionIndex;
}

public class Questions : MonoBehaviour
{
    public QuestionData questionData;
    public bool alreadyAnsweredCorrectly = false;

    public Canvas questionCanvas;
    public TMP_Text questionText;
    public TMP_Text[] optionTexts;
    public Button[] optionButtons;
    public TMP_Text questionTimerText;
    public TMP_Text errorsText;
    private Color brown = new Color(188f / 255f, 158f / 255f, 145f / 255f);

    private int correctAnswerIndex = -1;
    private float questionTimer = 0f;
    private bool questionActive = false;
    private float extraTime = 0f;
    private Coroutine questionCoroutine;
    private Player playerRef;

    public GameObject blockingObstacle;

    private void Start()
    {
        questionCanvas.gameObject.SetActive(false);
        blockingObstacle.SetActive(true);
    }

    public void ShowQuestion(Player player)
    {
        if (alreadyAnsweredCorrectly)
        {
            return;
        }

        playerRef = player;
        playerRef.isCollidingObstacle = true;
        playerRef.GetComponent<Animator>().SetInteger("transition", 0);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        questionCanvas.gameObject.SetActive(true);

        questionTimer = 25f + extraTime;
        extraTime = 0f;
        questionActive = true;

        if (questionCoroutine != null)
            StopCoroutine(questionCoroutine);

        questionCoroutine = StartCoroutine(QuestionTimer());

        questionText.text = questionData.question;
        correctAnswerIndex = questionData.correctOptionIndex;

        for (int i = 0; i < optionTexts.Length; i++)
        {
            optionTexts[i].text = questionData.options[i];
            int index = i;
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
        }
    }

    public void AddExtraTime(float time)
    {
        extraTime += time;
    }

    private IEnumerator QuestionTimer()
    {
        while (questionTimer > 0f)
        {
            questionTimer -= Time.deltaTime;
            questionTimerText.text = $"{Mathf.CeilToInt(questionTimer)}s";
            yield return null;
        }

        if (questionActive)
        {
            playerRef.contErrors++;
            errorsText.text += "X";

            CloseQuestion();
            if (playerRef.contErrors == 3)
            {
                playerRef.changeScenes.SceneGameOver();
            }
        }
    }

    private void OnOptionSelected(int index)
    {
        if (index == correctAnswerIndex)
        {
            optionButtons[index].image.color = Color.green;
            alreadyAnsweredCorrectly = true;
            blockingObstacle.SetActive(false);
        }
        else
        {
            optionButtons[index].image.color = Color.red;
            playerRef.contErrors++;
            errorsText.text += "X";

            if (playerRef.contErrors == 3)
            {
                playerRef.changeScenes.SceneGameOver();
            }
        }

        StartCoroutine(CloseQuestionDelayed(1f));
    }

    private IEnumerator CloseQuestionDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        CloseQuestion();
    }

    private void CloseQuestion()
    {
        foreach (var btn in optionButtons)
        {
            btn.image.color = brown;
        }

        questionCanvas.gameObject.SetActive(false);
        questionTimerText.text = "";
        questionActive = false;

        playerRef.isCollidingObstacle = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
