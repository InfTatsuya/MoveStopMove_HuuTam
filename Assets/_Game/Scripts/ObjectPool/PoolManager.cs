using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] CharacterPool characterPool;
    [SerializeField] ProjectilePool projectilePool;

    private void Start()
    {
        Character.onAnyCharacterSpawnProjectile += Character_onAnyCharacterSpawnProjectile;
        if(GameManager.Instance != null)
        {
            GameManager.onEnemySpawn += GameManager_onEnemySpawn;
        }
        else if(EndlessGameMode.Instance != null)
        {
            EndlessGameMode.onEnemySpawn += EndlessGameMode_onEnemySpawn;
        }
    }

    private Enemy GameManager_onEnemySpawn()
    {
        Debug.Log("Spawn Enemy run");

        Enemy enemy = characterPool.GetPooledObject() as Enemy;

        if(enemy != null)
        {
            Vector3 pos = new Vector3(Random.Range(-25f, 25f), 0f, Random.Range(-25f, 25f));           
            enemy.transform.SetParent(null);
            enemy.transform.position = pos;
            enemy.OnNewGame();
        }

        return enemy;
    }


    private EndlessModeEnemy EndlessGameMode_onEnemySpawn()
    {
        Debug.Log("Spawn Enemy run");

        EndlessModeEnemy enemy = characterPool.GetPooledObject() as EndlessModeEnemy;

        if (enemy != null)
        {
            Vector3 pos = new Vector3(Random.Range(-25f, 25f), 0f, Random.Range(-25f, 25f));
            enemy.transform.SetParent(null);
            enemy.transform.position = pos;
            enemy.OnNewGame();
        }

        return enemy;
    }

    private void Character_onAnyCharacterSpawnProjectile(object sender, Character.OnAnyCharacterSpawnProjectileArgs e)
    {
        SpawnProjectile(sender as Character, e.destination, e.damage, e.projectileAmt, e.weaponType);
    }

    private void SpawnProjectile(Character instigator, Vector3 destination, int damage,int amount, EWeaponType weaponType)
    {
        for (int i = 0; i < amount; i++)
        {
            Projectile project = projectilePool.GetPooledObject();
            project.transform.SetParent(null);
            project.transform.position = instigator.SpawnProjectilePoint.position;
            if(i == 0)
            {
                project.SetupProjectile(destination, instigator, damage, weaponType);
            }
            else
            {
                Vector3 dest = destination + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
                project.SetupProjectile(dest, instigator, damage, weaponType);
            }
        }       
    }

    private void OnDestroy()
    {
        Character.onAnyCharacterSpawnProjectile -= Character_onAnyCharacterSpawnProjectile;
        GameManager.onEnemySpawn -= GameManager_onEnemySpawn;
        EndlessGameMode.onEnemySpawn -= EndlessGameMode_onEnemySpawn;
    }
}
