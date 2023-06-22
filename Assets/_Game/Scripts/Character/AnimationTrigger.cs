using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AnimationTrigger : MonoBehaviour
{
    [SerializeField] Character myCharacter;

    public void SpawnProjectile()
    {
        Character target = myCharacter.GetFirstTarget();

        if (target != null)
        {
            myCharacter.SpawnProjectile(target.transform.position);
        }
    }

    public void AttackFinishTrigger()
    {
        myCharacter.CharacterStateMachine.CurrentState.SetAnimTrigger(true);
    }

    public void MeleeAttack()
    {
        EndlessModeEnemy enemy = myCharacter as EndlessModeEnemy;
        if(enemy != null)
        {
            enemy.MeleeAttack();
        }

        EndlessModeBoss boss = myCharacter as EndlessModeBoss;
        if(boss != null)
        {
            boss.MeleeAttack();
        }
    }

    public void TriggerTeleport()
    {
        EndlessModeBoss boss = myCharacter as EndlessModeBoss;
        if (boss != null)
        {
            Vector3 pos = EndlessGameMode.Instance.MainPlayer.transform.position + new Vector3(1, 0, -1);
            boss.Teleport(pos);
        }
    }


}
