using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour {

    [SerializeField] float delayInSeconds = 2f;

    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGame()
    {
        
        SceneManager.LoadScene("Game");
        FindObjectOfType<GameSession>().ResetGame();
    }

    public void LoadContinueGame()
    {
        StartCoroutine(ContinueDelay());
    }

    public void LoadGameOver()
    {
        StartCoroutine(GameOverDelay());     
    }

    IEnumerator ContinueDelay()
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene("Game");
        
    }

    IEnumerator GameOverDelay()
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene("Game Over");
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
