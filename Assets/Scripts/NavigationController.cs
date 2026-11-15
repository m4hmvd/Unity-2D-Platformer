using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationController : MonoBehaviour
{
    // Intro screen: Start button
    public void onClickStart()
    {
        ResetGame();
        SceneManager.LoadScene(1);   // Level1
    }

    // Game Over screen: Retry button
    public void onClickRetry()
    {
        ResetGame();
        SceneManager.LoadScene(1);   // Level1
    }

    // Quit button (Intro + Game Over)
    public void onClickQuit()
    {
        Debug.Log("Quit");           // Application.Quit does nothing inside the editor
        Application.Quit();
    }

    // static values survive scene changes, so clear them for a new game
    void ResetGame()
    {
        PlayerStats.score = 0;
        PlayerStats.hasHeart = false;
    }
}
