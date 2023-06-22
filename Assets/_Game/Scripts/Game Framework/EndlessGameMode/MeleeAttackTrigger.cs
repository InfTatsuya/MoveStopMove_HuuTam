using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackTrigger : MonoBehaviour
{
    [SerializeField] EndlessModeEnemy enemy;
    [SerializeField] EndlessModeBoss boss;

    private int damage;

    private void Start()
    {
        if(enemy != null)
        {
            damage = enemy.MeleeDamage;
        }

        if(boss != null)
        {
            damage = boss.MeleeDamage;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Player>(out var player))
        {
            player.TakeDamage(damage, enemy);
        }
    }
}
