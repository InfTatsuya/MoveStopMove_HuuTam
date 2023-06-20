using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class EndlessGameMode : MonoBehaviour
{
    public static EndlessGameMode Instance { get; private set; }

    public delegate EndlessModeEnemy OnSpawnEnemy();
    public static event OnSpawnEnemy onEnemySpawn;

    public event EventHandler onPlayerClearWave;

    [SerializeField] List<BoosterEffect> boosterEffectsList;
    [SerializeField] EndlessData endlessData;
    [SerializeField] Player player;
    public Player MainPlayer => player;

    private int currentWave;
    private int amtEnemiesToSpawn;

    private float timer = 2f;

    [SerializeField] private List<EndlessModeEnemy> enemiesOnScreen = new List<EndlessModeEnemy>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        EndlessModeEnemy.onEndlessEnemyDeath += EndlessModeEnemy_onEndlessEnemyDeath;

        currentWave = 0;
        amtEnemiesToSpawn = endlessData.enenmiesPerWave[currentWave];

        player.EndlessMode_Equip(DataTransfer.Instance.PlayerEquipWeapon, DataTransfer.Instance.PlayerSkinDataList);
    }

    private void EndlessModeEnemy_onEndlessEnemyDeath(object sender, EndlessModeEnemy.OnAnyEndlessEnemyDeathArgs e)
    {
        enemiesOnScreen.Remove(e.enemy);
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if(timer < 0)
        {
            if (amtEnemiesToSpawn > 0)
            {
                enemiesOnScreen.Add(onEnemySpawn?.Invoke());
                amtEnemiesToSpawn--;

                timer = endlessData.spawnCooldown;
            }
            else if(enemiesOnScreen.Count > 0)
            {
                return;
            }
            else if(amtEnemiesToSpawn == 0)
            {
                timer = endlessData.waveCooldown;
                amtEnemiesToSpawn--;
            }
            else
            {
                NextWave();
            }
        }
    }

    public List<BoosterEffect> GetRandomEffect()
    {
        int random1, random2, random3;

        random1 = Random.Range(0, boosterEffectsList.Count);

        random2 = Random.Range(0, boosterEffectsList.Count);
        while(random2 == random1)
        {
            random2 = Random.Range(0, boosterEffectsList.Count);
        }

        random3 = Random.Range(0, boosterEffectsList.Count);
        while (random3 == random1 || random3 == random2)
        {
            random3 = Random.Range(0, boosterEffectsList.Count);
        }

        return new List<BoosterEffect>() { boosterEffectsList[random1], boosterEffectsList[random2], boosterEffectsList[random3]};
       
    }

    private void NextWave()
    {
        onPlayerClearWave?.Invoke(this, EventArgs.Empty);
        PauseGame();

        currentWave++;
        amtEnemiesToSpawn = endlessData.enenmiesPerWave[currentWave];
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;

        foreach(var enemy in enemiesOnScreen)
        {
            enemy.IsPause = true;
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;

        foreach (var enemy in enemiesOnScreen)
        {
            enemy.IsPause = false;
        }
    }
}
