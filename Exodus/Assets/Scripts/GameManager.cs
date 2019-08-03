using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] menus;
    //public GameObject mainMenu;
    //public GameObject pauseMenu;
    public GameObject turretManager;
    public GameObject planet;
    public GameObject enemyManager;
    public int difficulty = 0;
    public bool inGame = false;
    public bool skipMenu = false;
    void Start()
    {
        //Time.timeScale = 0.0f;
        //mainMenu.SetActive(true);
        SelectMenu(0);
        if(skipMenu)LaunchGame();
    }

    void StartIntro()
    {
        Time.timeScale = 1.0f;
        SelectMenu(2);
    }

    void SlideshowFinished()
    {
        if(inGame == false)
        {
            LaunchGame();
        }
        else{GiveUp();}
    }

    void GameOver()
    {
        // end game
    }

    void HideMenus()
    {
        foreach (var menu in menus)
        {
            menu.SetActive(false);
        }
    }
    void SelectMenu(int ind){
        HideMenus();
        menus[ind].SetActive(true);
    }

    public void Easy(){difficulty=0;StartIntro();}
    public void Medium(){difficulty=1;StartIntro();}
    public void Hard(){difficulty=2;StartIntro();}
    public void Quit(){Application.Quit();}
    public void GiveUp(){SceneManager.LoadScene( SceneManager.GetActiveScene().name );}
    //public void Resume(){pauseMenu.SetActive(false);Time.timeScale=1.0f;}
    public void Resume(){HideMenus();Time.timeScale=1.0f;}

    void LaunchGame()
    {
        Time.timeScale=1.0f;
        enemyManager.SetActive(true);
        turretManager.SetActive(true);
        enemyManager.SendMessage("SetDifficulty",difficulty);
        turretManager.SendMessage("SetDifficulty",difficulty);
        HideMenus();
        //mainMenu.SetActive(false);
        planet.SetActive(true);
        inGame = true;
    }
    void Update()
    {
        if(Input.GetButtonUp("Menu") && inGame)
        {
            if (Time.timeScale==0.0f)
            {
                //pauseMenu.SetActive(false);
                HideMenus();
                Time.timeScale=1.0f;
            }
            else
            {
                //pauseMenu.SetActive(true);
                SelectMenu(1);
                Time.timeScale=0.0f;
            }
        }
    }
}
