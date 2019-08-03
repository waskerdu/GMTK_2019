using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Difficulty Config")]

public class DifficultyConfig : ScriptableObject
{
    [SerializeField] public float timeBetweenWaves;
    [Header("Enemies Per Group")]
    [SerializeField] public int minEnemiesPerGroup;
    [SerializeField] public int maxEnemiesPerGroup;
    [SerializeField] public float rampSpeedEnemiesPerGroup;
    [Header("Groups Per Wave")]
    [SerializeField] public int minGroupsPerWave;
    [SerializeField] public int maxGroupsPerWave;
    [SerializeField] public float rampSpeedGroupsPerWave;
    [Header("Swarm Chance")]
    [SerializeField] [Range(0, 1)] public float minSwarmChance;
    [SerializeField] [Range(0, 1)] public float maxSwarmChance;
    [SerializeField] public float rampSpeedSwarmChance;
    [Header("Stragglers")]
    [SerializeField] public float stragglerSpawnRate;
    [SerializeField] public float minStragglerSpawnTime;
    [SerializeField] public float rampSpeedStragglerSpawnRate;


}
