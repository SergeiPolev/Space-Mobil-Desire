using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadGameOver()
    {
        SceneManager.LoadScene("Game Over"); 
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Main Scene");
    }

    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);
        FindObjectOfType<GameSession>().ResetGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
