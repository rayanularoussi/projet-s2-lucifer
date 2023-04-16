using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenuManager : MonoBehaviour
{
    public PlayerStats playerStats;
    public GameObject gameOverUI;

    public void Start()
    {
        gameOverUI.SetActive(false);
    }

    public void Update()
    {
        gameOver();
    }
    
    public void PlayGame()
    {
        StartCoroutine(StartGameWithDelay(1f)); // start the coroutine with a delay of 2 seconds
    }

    IEnumerator StartGameWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // wait for the specified delay
        SceneManager.LoadScene(2); // load the game scene
    }

    public void Menu()
    {
        SceneManager.LoadScene(1);  // start the coroutine with a delay of 2 seconds
    }

    public void QuitGame()
    {
        StartCoroutine(QuitGameWithDelay(1f)); // start the coroutine with a delay of 2 seconds
    }

    IEnumerator QuitGameWithDelay(float delay)
    {
        Debug.Log("Quitting in " + delay + " seconds...");
        yield return new WaitForSeconds(delay); // wait for the specified delay
        Debug.Log("Quitting");
        Application.Quit(); // quit the application
    }

    public void gameOver()
    {
        if(playerStats.currentHealth <= 0)
        {
            gameOverUI.SetActive(true);
            Cursor.visible = true;
        }
    }
}