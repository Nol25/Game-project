using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControl : MonoBehaviour
{
    [SerializeField] AudioSource buttonPress;
    [SerializeField] GameObject FadeOut;
    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public void StartGame()
    {
       // buttonPress.Play();
      //  FadeOut.SetActive(true);
        PlayerMaintain.levelNumber = 2;
        StartCoroutine(PlayTheGame());
    }

    public void QuitGame()
        {
            Application.Quit();
    }

    IEnumerator PlayTheGame()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(2);
    }
}
