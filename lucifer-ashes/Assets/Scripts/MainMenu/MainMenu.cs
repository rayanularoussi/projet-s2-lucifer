using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        StartCoroutine(StartGameWithDelay(3f)); // start the coroutine with a delay of 2 seconds
    }

    IEnumerator StartGameWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // wait for the specified delay
        Cursor.visible = false;
        SceneManager.LoadScene("Game"); // load the game scene
    }

    public void QuitGame()
    {
        StartCoroutine(QuitGameWithDelay(3f)); // start the coroutine with a delay of 2 seconds
    }

    IEnumerator QuitGameWithDelay(float delay)
    {
        Debug.Log("Quitting in " + delay + " seconds...");
        yield return new WaitForSeconds(delay); // wait for the specified delay
        Debug.Log("Quitting");
        Application.Quit(); // quit the application
    }
}
