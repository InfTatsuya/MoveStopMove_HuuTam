using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour, ISaveManager
{
    public enum GameState
    {
        MainMenu,
        Playing,
        Pause,
        GameOver
    }

    public static GameManager Instance {  get; private set; }

    public delegate Enemy OnSpawnEnemy();
    public static event OnSpawnEnemy onEnemySpawn;

    private GameState gameState = GameState.MainMenu;
    public GameState CurrentGameState
    {
        get => gameState;

        private set
        {
            gameState = value;
            switch (value)
            {
                case GameState.MainMenu:
                    isPlaying = false;
                    Time.timeScale = 1f;
                    break;

                case GameState.Playing:
                    isPlaying = true;
                    Time.timeScale = 1f;
                    break;

                case GameState.Pause:
                    isPlaying = false;
                    Time.timeScale = 0f;
                    break;

                case GameState.GameOver:
                    break;

                default:
                    break;
            }
        }
    }

    [SerializeField] Player player;
    public Player MainPlayer => player;

    [SerializeField] int initialAmountEnemy = 5;
    [SerializeField] float spawnCooldown = 5f;
    [SerializeField] int maxAmountPerSpawn = 8;
    [SerializeField] int minAmountPerSpawn = 3;

    [SerializeField] private List<Enemy> enemiesOnScreen = new List<Enemy>();
    private float spawnTimer;
    private bool isPlaying;

    [SerializeField] private int playerKillCount;
    public int PlayerKillCount => playerKillCount;

    private int starCount = 0;
    public int StarCount => starCount;

    private string playerName;
    public string PlayerName => playerName;

    public void SetPlayerName(string newName)
    {
        playerName = newName;
    }

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

    private void Start()
    {
        gameState = GameState.MainMenu;
    }

    private void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }

    private void Update()
    {
        if (gameState != GameState.Playing) return;

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
            Enemy enemy = onEnemySpawn?.Invoke();
            bool isOkie = false;

            while (!isOkie)
            {
                Vector3 pos = new Vector3(Random.Range(-25f, 25f), 0f, Random.Range(-25f, 25f));

                if (Vector3.Distance(pos, GameManager.Instance.MainPlayer.transform.position) > 5f)
                {
                    isOkie = true;

                    if (enemy != null)
                    {
                        enemy.transform.SetParent(null);
                        enemy.transform.position = pos;
                        enemy.OnNewGame();
                    }
                }
            }

            enemiesOnScreen.Add(enemy);
            yield return new WaitForSeconds(1f);
        }

        spawnTimer = spawnCooldown;
    }

    public void StartNewGame()
    {
        CurrentGameState = GameState.Playing;

        StartCoroutine(SpawnEnemy(initialAmountEnemy));
        spawnTimer = spawnCooldown;

        player.OnNewGame();
        playerKillCount = 0;
    }

    public void CalculateStarByKillAmount()
    {
        if(playerKillCount > 0 && playerKillCount < 30)
        {
            starCount += 1;
        }
        else if(playerKillCount >= 30 && playerKillCount < 60)
        {
            starCount += 2;
        }
        else if(playerKillCount >= 60)
        {
            starCount += 3;
        }
    }

    public void IncreaseKillCount()
    {
        playerKillCount++;
    }

    public void PauseGame()
    {
        CurrentGameState = GameState.Pause;
        foreach(var enemy in enemiesOnScreen)
        {
            enemy.IsPause = true;
        }
    }

    public void ResumeGame()
    {
        CurrentGameState = GameState.Playing;
        foreach (var enemy in enemiesOnScreen)
        {
            enemy.IsPause = false;
        }
    }

    public void ReturnAllEnemy()
    {
        CurrentGameState = GameState.MainMenu;
        foreach (var enemy in enemiesOnScreen)
        {
            enemy.ReleaseSelf();
        }
    }

    public void LoadData(GameData data)
    {
        starCount = data.starAmt;
        playerName = data.playerName;
    }

    public void SaveData(ref GameData data)
    {
        data.starAmt = starCount;
        data.playerName = playerName;
    }
}
