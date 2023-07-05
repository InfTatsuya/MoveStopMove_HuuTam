using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Enddless Mode/Data", fileName = "EndlessData_")]
public class EndlessData : ScriptableObject
{
    [Tooltip("1 meaning boss wave")]
    public List<int> enenmiesPerWave;
    public float waveCooldown = 5f;
    public float spawnCooldown = 1f;

    public List<GameObject> bossPrefabs;
}
