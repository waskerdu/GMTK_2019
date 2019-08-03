using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Difficulty Config")]

public class DifficultyConfig : ScriptableObject
{
    [SerializeField] public int minEnemiesPerGroup;
    [SerializeField] public float timeBetweenWaves;
    [SerializeField] public float swarmChance;
    [SerializeField] public int rampSpeedEnemyNumber;
    [SerializeField] public float rampSpeedSwarmChance;
    [SerializeField] public float maxSwarmChance;

}
