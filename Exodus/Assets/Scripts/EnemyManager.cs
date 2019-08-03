﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] List<DifficultyConfig> difficultyConfigs;
    [SerializeField] float spawnDistance = 10f;
    [SerializeField] float minEnemyGroupingDistance = 0.2f;
    [SerializeField] float maxEnemyGroupingDistance = 1.2f;

    [SerializeField] GameObject enemy;
    [SerializeField] int currentWave;

    [Header("Sounds")]
    [SerializeField] AudioSource smallEnemyDamageSound;
    [SerializeField] AudioSource bigEnemyDamageSound;
    [SerializeField] AudioSource smallEnemyDestroySound;
    [SerializeField] AudioSource bigEnemyDestroySound;
    [SerializeField] AudioSource smallEnemyAttackSound;
    [SerializeField] AudioSource bigEnemyAttackSound;
    [SerializeField] AudioSource swarmSound;
    [SerializeField] AudioSource warningSound;



    DifficultyConfig difficultyConfig;
    List<GameObject> enemyPool = new List<GameObject>();
    Vector3 planetPos = new Vector3(0,0,0);
    float waveTimer;
    float stragglerTimer;


    private void Awake()
    {
        SetDifficulty(0);
        waveTimer = difficultyConfig.timeBetweenWaves;
        stragglerTimer = waveTimer;
    }

    // Update is called once per frame
    void Update()
    {
        WaveSpawning();
        StragglerSpawning();
    }

    public void SetDifficulty(int difficulty)
    {
        difficultyConfig = difficultyConfigs[difficulty];
    }

    public void GameWon()
    {

    }

    public void GameOver()
    {
        var enemies = GetComponentsInChildren<Enemy>();
        foreach (var enemy in enemies)
        {
            enemy.SendMessage("GameOver");
        }
    }

    void SpawnGroup()
    {
        var spawnPos = GetNewSpawnPos();

        var enemiesThisGroup = Mathf.Min(difficultyConfig.minEnemiesPerGroup + (difficultyConfig.rampSpeedEnemiesPerGroup * currentWave), difficultyConfig.maxEnemiesPerGroup);


        for (int i = 0; i < enemiesThisGroup; i++)
        {
            var positionOffset = GetNewSpawnPos().normalized * Random.Range(minEnemyGroupingDistance, maxEnemyGroupingDistance);
            var newEnemy = FindFirstInactiveEnemy();
            if (!newEnemy)
            {
                newEnemy = Instantiate(enemy, spawnPos + positionOffset, Quaternion.identity, transform);
                enemyPool.Add(newEnemy);
            }
            else
            {
                newEnemy.SetActive(true);
                newEnemy.transform.position = spawnPos + positionOffset;
            }
            

        }


    }

    void SpawnSingle()
    {
        var newEnemy = FindFirstInactiveEnemy();
        if (!newEnemy)
        {
            newEnemy = Instantiate(enemy, GetNewSpawnPos(), Quaternion.identity, transform);
            enemyPool.Add(newEnemy);
        }
        else
        {
            newEnemy.SetActive(true);
            newEnemy.transform.position = GetNewSpawnPos();
        }
        
    }

    Vector3 GetNewSpawnPos()
    {
        var TempObject = new GameObject();
        var SpawnDir = TempObject.transform;
        SpawnDir.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 364f));

        var SpawnPos = new Vector3();
        SpawnPos = SpawnDir.up * spawnDistance;
        Destroy(TempObject);
        return SpawnPos;
    }

    void WaveSpawning()
    {
        var groupsThisWave = Mathf.Min(difficultyConfig.minGroupsPerWave + Mathf.Round(difficultyConfig.rampSpeedGroupsPerWave * currentWave), difficultyConfig.maxGroupsPerWave);
        waveTimer -= Time.deltaTime;
        if (waveTimer < 0)
        {
            waveTimer = difficultyConfig.timeBetweenWaves;
            for (int i = 0; i < groupsThisWave; i++)
            {
                SpawnGroup();
            }
            currentWave++;
        }
    }

    void StragglerSpawning()
    {
        stragglerTimer -= Time.deltaTime;
        if (stragglerTimer < 0)
        {
            stragglerTimer = Mathf.Max(difficultyConfig.stragglerSpawnRate - (currentWave * difficultyConfig.rampSpeedStragglerSpawnRate), difficultyConfig.minStragglerSpawnTime);
            SpawnSingle();
        }
    }

    GameObject FindFirstInactiveEnemy()
    {
        if (enemyPool.Count == 0)
        {
            return null;
        }
        foreach (GameObject enemy in enemyPool)
        {
            if (!enemy.activeInHierarchy) return enemy;
        }
        return null;
    }

    public void SmallDeathSound()
    {
        if (!smallEnemyDestroySound.isPlaying)
        {
            smallEnemyDestroySound.Play();

        }
    }

    public void BigDeathSound()
    {
        if (!bigEnemyDestroySound.isPlaying)
        {
            bigEnemyDestroySound.Play();

        }
    }

    public void SmallDamageSound()
    {
        if (!smallEnemyDamageSound.isPlaying)
        {
            smallEnemyDamageSound.Play();

        }
    }

    public void BigDamageSound()
    {
        if (!bigEnemyDamageSound.isPlaying)
        {
            bigEnemyDamageSound.Play();

        }
    }

    public void SmallAttackSound()
    {
        if (!smallEnemyAttackSound.isPlaying)
        {
            smallEnemyAttackSound.Play();

        }
    }

    public void BigAttackSound()
    {
        if (!bigEnemyAttackSound.isPlaying)
        {
            bigEnemyAttackSound.Play();

        }
    }
}