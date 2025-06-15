//? enemy
//se player tiver colidindo com o obstaculo da pergunta inimigo n se move
if (playerScript.isCollidingObstacle)
{
    agent.isStopped = true;
    anim.SetInteger("transition", 0);
    return;
}


//? change scenes
//bloqueia/desbloqueia cursor do mouse, ativa/desativa musica


//? item
transform.Rotate(rotationSpeed * Time.deltaTime); 
//gira item no eixo y


//? questions
[System.Serializable] //permite editar essa classe no Inspector da Unity
public class QuestionData
{
    public string question; //texto da pergunta
    public string[] options; //alternativas
    public int correctOptionIndex; //índice da resposta correta
}
//isso é pra referenciar as coisas no inspector (arrasta da hierarquia)
public Canvas questionCanvas;
public TMP_Text questionText;
public TMP_Text[] optionTexts;
public Button[] optionButtons;
public TMP_Text questionTimerText;
public TMP_Text errorsText;

//inteiros não inicializados recebem o padrão 0 (e 0 seria uma resposta que pode ou nao estar certa) - todos iam ter a resposta 0 como certa (nem todas são)
private int correctAnswerIndex = -1;

//se fosse OnOptionSelected(i) ia pegar sempre so o ultimo valor do i
for (int i = 0; i < optionTexts.Length; i++)
{
    optionTexts[i].text = questionData.options[i];
    int index = i;
    optionButtons[i].onClick.RemoveAllListeners();
    optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
}

//enquanto tempo nao acabar vai diminuindo, mas quando acaba (e se tiver ativa) conta como erro
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


//? player
void Rotate()
{
    Vector3 lookDirection = cameraTransform.forward; //pega a direção para onde a câmera está olhando
    lookDirection.y = 0f; //remove inclinação vertical da direção da câmera, impede que o player incline para cima ou para baixo

    if (lookDirection.sqrMagnitude > 0.1f) //sqrMagnitude - quadrado do tamanho do vetor (mais rapido) - se direção for quase 0 não rotaciona
    {
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection); //alvo p/ onde player gira
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 8f * Time.deltaTime); //slerp - interpolação - rotação suave entre rotação atual e final, 8f timedeltatime - velocidade da rotação, 
    };
}

//? camera
Vector3 rayOrigin = target.position + Vector3.up * 1.5f;
Vector3 rayDirection = (desiredPosition - rayOrigin).normalized;
float rayDistance = Vector3.Distance(rayOrigin, desiredPosition);

RaycastHit hit;
if (Physics.Raycast(rayOrigin, rayDirection, out hit, rayDistance))
{
    desiredPosition = hit.point - rayDirection * 0.5f;
}
//pra nao passar pela parede