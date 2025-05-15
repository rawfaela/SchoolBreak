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
    public void SceneStart()
    {
        musica.Stop();
        SceneManager.LoadScene("Start");
    }
    public void SceneGameOver()
    {
        musica.Stop();
        SceneManager.LoadScene("GameOver");
    }
    public void SceneWin()
    {
        musica.Stop();
        SceneManager.LoadScene("Win");
    }
}
