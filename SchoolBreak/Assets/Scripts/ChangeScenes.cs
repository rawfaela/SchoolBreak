using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeScenes : MonoBehaviour
{
    public AudioSource musica;

    void Start()
    {
        musica.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SceneGame()
    {
        musica.Stop();
        SceneManager.LoadScene("Game");
    }
    public void SceneGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
    public void SceneWin()
    {
        SceneManager.LoadScene("Win");
    }
    public void OnReturnStart()
    {
        SceneManager.LoadScene(0);
    }
}
