using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackTrigger : MonoBehaviour
{
    [SerializeField] EndlessModeEnemy enemy;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Player>(out var player))
        {
            player.TakeDamage(enemy.MeleeDamage, enemy);
        }
    }
}
