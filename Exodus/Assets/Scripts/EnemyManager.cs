using System.Collections;
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
    DifficultyConfig difficultyConfig;
    Vector3 planetPos = new Vector3(0,0,0);



    private void Awake()
    {
        SetDifficulty(0);

        SpawnSingle();
        SpawnGroup();

    }

    // Update is called once per frame
    void Update()
    {
        
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

        var enemiesThisGroup = difficultyConfig.minEnemiesPerGroup + (difficultyConfig.rampSpeedEnemyNumber * currentWave);


        for (int i = 0; i < enemiesThisGroup; i++)
        {
            var positionOffset = GetNewSpawnPos().normalized * Random.Range(minEnemyGroupingDistance, maxEnemyGroupingDistance);
            var newEnemy = Instantiate(enemy, spawnPos + positionOffset, Quaternion.identity, transform);

        }

        currentWave++;
    }

    void SpawnSingle()
    {
        Instantiate(enemy, GetNewSpawnPos(), Quaternion.identity, transform);
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


}
