using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject turretManager;
    public GameObject planet;
    public GameObject enemyManager;
    public int difficulty = 0;
    public bool inGame = false;
    void Start()
    {
        Time.timeScale = 0.0f;
        // show main menu
        mainMenu.SetActive(true);
        LaunchGame();
    }

    void GameOver()
    {
        // end game
    }

    public void Easy(){difficulty=0;LaunchGame();}
    public void Medium(){difficulty=1;LaunchGame();}
    public void Hard(){difficulty=2;LaunchGame();}
    public void Quit(){Application.Quit();}
    public void GiveUp(){SceneManager.LoadScene( SceneManager.GetActiveScene().name );}
    public void Resume(){pauseMenu.SetActive(false);Time.timeScale=1.0f;}

    void LaunchGame()
    {
        Time.timeScale=1.0f;
        enemyManager.SetActive(true);
        turretManager.SetActive(true);
        //enemyManager.SendMessage("SetDifficulty",difficulty);
        //turretManager.SendMessage("SetDifficulty",difficulty);
        mainMenu.SetActive(false);
        inGame = true;
    }
    void Update()
    {
        if(Input.GetButtonUp("Menu") && inGame)
        {
            if (Time.timeScale==0.0f)
            {
                pauseMenu.SetActive(false);
                Time.timeScale=1.0f;
            }
            else
            {
                pauseMenu.SetActive(true);
                Time.timeScale=0.0f;
            }
        }
    }
}
