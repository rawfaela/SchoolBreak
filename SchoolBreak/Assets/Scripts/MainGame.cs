using UnityEngine;
using UnityEngine.SceneManagement;
public class MainGame : MonoBehaviour
{
    public AudioSource music;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        music.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeScene()
    {
        music.Stop();
        SceneManager.LoadScene(1);
    }
}
