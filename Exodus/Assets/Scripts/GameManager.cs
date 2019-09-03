﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] menus;
    //public GameObject mainMenu;
    //public GameObject pauseMenu;
    public GameObject turretManager;
    public GameObject planet;
    public GameObject planetExplosion;
    public GameObject enemyManager;
    public GameObject wormhole;
    public AudioClip planetExplosionSound;
    public AudioClip[] music;
    public AudioSource audioSource;
    public GameObject face;
    public Sprite[] faceSprites;
    public GameObject tutorialText;
    public int currentSong = 0;
    [Range(0,1)] public float planetExplosionSoundVolume = 1;
    public int difficulty = 0;
    public bool inGame = false;
    public bool gameOver = false;
    public bool skipMenu = false;
    public float gameTime = 0.0f;
    public float gameDuration = 300.0f;
    public float playerHealth = 1.0f;
    public float healthThreshold = 0.3f;
    public bool inTutorial = false;
    void Start()
    {
        //Time.timeScale = 0.0f;
        //mainMenu.SetActive(true);
        inTutorial = (PlayerPrefs.GetInt("IsReplaying", 0) == 0);
        if (inTutorial)
        {
            tutorialText.transform.parent.gameObject.SetActive(false);
        }
        PlayerPrefs.SetInt("IsReplaying", 1);

        SelectMenu(0);
        if(skipMenu)LaunchGame();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = music[currentSong];
        audioSource.Play();
    }

    void SetPlayerHealth(float health)
    {
        if (health < 0.1f && !(playerHealth < 0.1f))
        {
            face.GetComponent<SpriteRenderer>().sprite=faceSprites[3]; // panick
            audioSource.Stop();
            currentSong = 4;
            audioSource.clip = music[currentSong];
            audioSource.loop = true;
            audioSource.Play();
        }
        else if (health < 0.25f && !(playerHealth < 0.25f))// alarm face
        {
            face.GetComponent<SpriteRenderer>().sprite=faceSprites[2];
        }
        else if (health < 0.5f && !(playerHealth < 0.5f)) // meh face
        {
            face.GetComponent<SpriteRenderer>().sprite=faceSprites[1];
        }
        playerHealth = health;
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
    public void Tutorial()
    {
        inTutorial = !inTutorial;
        if (inTutorial)
        {
            tutorialText.GetComponent<TextMeshProUGUI>().text = "Replay Tutorial!";
        }
        else tutorialText.GetComponent<TextMeshProUGUI>().text = "Replay Tutorial?";
    }

    public void TutorialOver()
    {
        inTutorial = false;
        enemyManager.SetActive(true);
        enemyManager.SendMessage("SetDifficulty",difficulty);
    }

    public void GenerateBiomes()
    {
        int numForests = 7;
        int numDeserts = 5;
        int numOceans = 5;
        int numMountains = 3;
        int i;
        List<int> tempBiomes = new List<int>();
        List<int> biomes = new List<int>();
        if (inTutorial)
        {
            for (i = 0; i < 20; i++)
            {
                biomes.Add( i % 4);
            }
            turretManager.SendMessage("SetBiomeData", biomes.ToArray());
            return;
        }
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
        turretManager.SendMessage("SetBiomeData",biomes.ToArray());
    }

    void LaunchGame()
    {
        Time.timeScale=1.0f;
        planet.SetActive(true);
        wormhole.SetActive(false);

        if(difficulty == 0){gameDuration *= 0.6f;}
        
        turretManager.SetActive(true);
        GenerateBiomes();
        if (!inTutorial)
        {
            enemyManager.SetActive(true);
            enemyManager.SendMessage("SetDifficulty",difficulty);
        }
        turretManager.SendMessage("SetDifficulty",difficulty);
        HideMenus();
        gameTime = gameDuration;
        //mainMenu.SetActive(false);
        currentSong=2;
        audioSource.clip = music[currentSong];
        audioSource.Play();
        audioSource.loop = false;
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
        turretManager.SendMessage("GameOver");
        enemyManager.SendMessage("GameOver");
        planet.SetActive(false);
        planetExplosion.SetActive(true);
        AudioSource.PlayClipAtPoint(planetExplosionSound, Vector3.zero, planetExplosionSoundVolume );
        audioSource.Stop();
        currentSong = 6;
        audioSource.clip = music[currentSong];
        audioSource.PlayDelayed(6.0f);
        audioSource.loop = false;
        inGame = false;
        gameOver = true;
        SelectMenu(5);
    }

    void SpawnWormhole()
    {
        if(wormhole.activeInHierarchy)return;
        wormhole.SetActive(true);
        audioSource.Stop();
        currentSong = 5;
        audioSource.clip = music[currentSong];
        audioSource.Play();
        audioSource.loop = false;
        Debug.Log(wormhole.activeInHierarchy);
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
                audioSource.Stop();
                audioSource.clip = music[currentSong];
                audioSource.Play();
            }
            else
            {
                //pauseMenu.SetActive(true);
                SelectMenu(1);
                Time.timeScale=0.0f;
                audioSource.Stop();
                audioSource.clip = music[7];
                audioSource.Play();
            }
        }
        if(inGame)
        {
            if (inTutorial) { }
            else if (gameTime > 0 && !wormhole.activeInHierarchy){gameTime-=Time.deltaTime;}
            else{SpawnWormhole();}
            //Debug.Log((Vector3.zero-wormhole.transform.position).magnitude);
            if( (Vector3.zero-wormhole.transform.position).magnitude < 50  )
            {
                Debug.Log("Got here");
                if (playerHealth > healthThreshold){GoodWin();}
                else{BadWin();}
            }
        }

        if (currentSong == 0 && audioSource.isPlaying == false)
        {
            currentSong++;
            audioSource.clip = music[currentSong];
            audioSource.Play();
            audioSource.loop = true;
        }
        if (currentSong == 2 && audioSource.isPlaying == false)
        {
            currentSong++;
            audioSource.clip = music[currentSong];
            audioSource.Play();
            audioSource.loop = true;
        }
    }
}
