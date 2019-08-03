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
    public GameObject wormhole;
    public int difficulty = 0;
    public bool inGame = false;
    public bool gameOver = false;
    public bool skipMenu = false;
    public float gameTime = 0.0f;
    public float gameDuration = 300.0f;
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
        if(inGame == false && gameOver == false)
        {
            LaunchGame();
        }
        else{GiveUp();}
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

    public void GenerateBiomes()
    {
        int numForests = 7;
        int numDeserts = 5;
        int numOceans = 5;
        int numMountains = 3;
        int i;
        List<int> tempBiomes = new List<int>();
        List<int> biomes = new List<int>();
        for (i = 0; i < numForests; i++)
        {
            tempBiomes.Add(0);
        }
        for (i = 0; i < numDeserts; i++)
        {
            tempBiomes.Add(1);
        }
        for (i = 0; i < numOceans; i++)
        {
            tempBiomes.Add(2);
        }
        for (i = 0; i < numMountains; i++)
        {
            tempBiomes.Add(3);
        }
        while (tempBiomes.Count>0)
        {
            i = Random.Range(0,tempBiomes.Count-1);
            biomes.Add(tempBiomes[i]);
            tempBiomes.RemoveAt(i);
        }
        turretManager.SendMessage("SetBiomeData",biomes);
    }

    void LaunchGame()
    {
        Time.timeScale=1.0f;
        planet.SetActive(true);
        wormhole.SetActive(false);
        enemyManager.SetActive(true);
        turretManager.SetActive(true);
        enemyManager.SendMessage("SetDifficulty",difficulty);
        turretManager.SendMessage("SetDifficulty",difficulty);
        HideMenus();
        gameTime = gameDuration;
        //mainMenu.SetActive(false);
        
        inGame = true;
    }

    void GoodWin()
    {
        turretManager.SendMessage("GameWon");
        enemyManager.SendMessage("GameWon");
        planet.SetActive(false);
        inGame = false;
        gameOver = true;
        SelectMenu(3);
    }

    void BadWin()
    {
        turretManager.SendMessage("GameWon");
        enemyManager.SendMessage("GameWon");
        planet.SetActive(false);
        inGame = false;
        gameOver = true;
        SelectMenu(4);
    }

    void GameOver()
    {
        turretManager.SendMessage("GameWon");
        enemyManager.SendMessage("GameWon");
        planet.SetActive(false);
        inGame = false;
        gameOver = true;
        SelectMenu(5);
    }

    void SpawnWormhole()
    {
        wormhole.SetActive(true);
    }
    void Update()
    {
        if(Input.GetButtonDown("Jump") && inGame == false){SlideshowFinished();}
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
        if(inGame)
        {
            if (gameTime > 0){gameTime-=Time.deltaTime;}
            else{SpawnWormhole();}
            if( (Vector3.zero-wormhole.transform.position).magnitude < 5  ){planet.SendMessage("Win");}
        }
    }
}
