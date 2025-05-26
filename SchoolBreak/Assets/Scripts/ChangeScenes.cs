using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeScenes : MonoBehaviour
{
    public AudioSource music;
    public Canvas howTo;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Start")
        {
            music.Play();
            howTo.gameObject.SetActive(false);
        }
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
    public void SceneStart()
    {
        SceneManager.LoadScene("Start");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void HowTo()
    {
        howTo.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void HowToOut()
    {
        howTo.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void OnReturnStart()
    {
        SceneManager.LoadScene(0);
    }
}
