using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    public delegate Enemy OnSpawnEnemy();
    public static event OnSpawnEnemy onEnemySpawn;

    [SerializeField] Player player;
    public Player MainPlayer => player;

    [SerializeField] int initialAmountEnemy = 5;
    [SerializeField] float spawnCooldown = 5f;
    [SerializeField] int maxAmountPerSpawn = 8;
    [SerializeField] int minAmountPerSpawn = 3;

    [SerializeField] private List<Enemy> enemiesOnScreen = new List<Enemy>();
    private float spawnTimer;
    private bool isPlaying;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (!isPlaying) return;

        spawnTimer -= Time.deltaTime;

        if(spawnTimer < 0f)
        {
            spawnTimer = float.MaxValue;
            int randomAmt = UnityEngine.Random.Range(minAmountPerSpawn, maxAmountPerSpawn);
            StartCoroutine(SpawnEnemy(randomAmt));
        }
    }

    private IEnumerator SpawnEnemy(int amount)
    {
        yield return new WaitForSeconds(2f);

        for(int i = 0; i < amount; i++)
        {
            enemiesOnScreen.Add(onEnemySpawn?.Invoke());
            yield return new WaitForSeconds(1f);
        }

        spawnTimer = spawnCooldown;
    }

    public void StartNewGame()
    {
        StartCoroutine(SpawnEnemy(1));
        spawnTimer = spawnCooldown;
        isPlaying = true;

        player.OnNewGame();
    }

    public void GameOver()
    {
        isPlaying = false;
        foreach(var enemy in enemiesOnScreen)
        {
            enemy.IsPause = true;
        }
    }

    public void ResumeGame()
    {
        isPlaying = true;
        foreach (var enemy in enemiesOnScreen)
        {
            enemy.IsPause = false;
        }
    }

    public void ReturnAllEnemy()
    {
        foreach(var enemy in enemiesOnScreen)
        {
            enemy.ReleaseSelf();
        }
    }
}
