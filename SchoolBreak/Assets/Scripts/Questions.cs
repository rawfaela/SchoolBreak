using UnityEngine;

[System.Serializable]
public class QuestionData
{
    [TextArea]
    public string question;
    public string[] options; 
    public int correctOptionIndex; 
}
public class Questions : MonoBehaviour
{
    public QuestionData questionData;
}
