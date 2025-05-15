using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeScenes : MonoBehaviour
{
    public AudioSource musica;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musica.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeScene()
    {
        musica.Stop();
        SceneManager.LoadScene(1);
    }
}
