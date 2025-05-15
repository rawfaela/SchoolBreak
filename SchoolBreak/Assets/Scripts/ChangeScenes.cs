using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeScenes : MonoBehaviour
{
    public AudioSource music;

    void Start()
    {
        music.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SceneGame()
    {
        music.Stop();
        SceneManager.LoadScene("Game");
    }
    public void SceneGameOver()
    {
        SceneManager.LoadScene("GameOver");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void SceneWin()
    {
        SceneManager.LoadScene("Win");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
