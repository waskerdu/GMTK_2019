using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] List<DifficultyConfig> difficultyConfigs;
    [SerializeField] int currentWave;
    [SerializeField] GameObject enemy;
    [SerializeField] float spawnDistance = 10f;
    [SerializeField] float minEnemyGroupingDistance = 0.2f;
    [SerializeField] float maxEnemyGroupingDistance = 1.2f;

    [Header("Warning")]
    [SerializeField] GameObject warning;
    [SerializeField] float warningTime = 0.5f;


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
    bool gameOver = false;


    private void Awake()
    {
        SetDifficulty(0);
        waveTimer = difficultyConfig.beginningSpawnDelay;
        stragglerTimer = waveTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            return;
        }
        WaveSpawning();
        StragglerSpawning();
    }

    public void SetDifficulty(int difficulty)
    {
        difficultyConfig = difficultyConfigs[difficulty];
    }

    public void GameWon()
    {
        gameOver = true;

        foreach (var enemy in enemyPool)
        {
            if (!enemy.activeInHierarchy)
            {
                continue;
            }
            enemy.SendMessage("GameWon");
        }
    }

    public void GameOver()
    {
        gameOver = true;
        var enemies = GetComponentsInChildren<Enemy>();
        foreach (var enemy in enemies)
        {
            enemy.SendMessage("GameOver");
        }
    }

    void SpawnGroup()
    {
        var spawnPos = GetNewSpawnPos();

        Warning(spawnPos);

        var enemiesThisGroup = Mathf.Min(difficultyConfig.minEnemiesPerGroup + (difficultyConfig.rampSpeedEnemiesPerGroup * currentWave), difficultyConfig.maxEnemiesPerGroup);

        if (UnityEngine.Random.Range(0f,1f) <= Mathf.Min(difficultyConfig.minSwarmChance + (difficultyConfig.rampSpeedSwarmChance * currentWave), difficultyConfig.maxSwarmChance) )
        {
            var newKing = SpawnSingle();
            newKing.transform.position = spawnPos;
            newKing.GetComponent<Enemy>().BecomeKing();
            enemiesThisGroup -= 1;
        }

        for (int i = 0; i < enemiesThisGroup; i++)
        {
            var positionOffset = GetNewSpawnPos().normalized * UnityEngine.Random.Range(minEnemyGroupingDistance, maxEnemyGroupingDistance);
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

    private void Warning(Vector3 spawnPos)
    {
        WarningSound();
        var warningPos = spawnPos / 2f;
        var warningObject = Instantiate(warning, warningPos, Quaternion.LookRotation(Vector3.forward, spawnPos));
        Destroy(warningObject, warningTime);
    }

    

    GameObject SpawnSingle()
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
        return newEnemy;
    }

    Vector3 GetNewSpawnPos()
    {
        var TempObject = new GameObject();
        var SpawnDir = TempObject.transform;
        SpawnDir.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0f, 364f));

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

    public void SwarmSound()
    {
        if (!swarmSound.isPlaying)
        {
            swarmSound.Play();

        }
    }

    public void WarningSound()
    {
        if (!warningSound.isPlaying)
        {
            warningSound.Play();

        }
    }
}
