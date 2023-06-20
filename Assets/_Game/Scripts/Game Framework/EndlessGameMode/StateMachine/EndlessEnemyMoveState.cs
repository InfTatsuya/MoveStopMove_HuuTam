using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessEnemyMoveState : State
{
    private EndlessModeEnemy enemy;
    private Player player;

    private float scanTimer;

    public EndlessEnemyMoveState(Character character, Animator anim, int animString) : base(character, anim, animString)
    {
        enemy = character as EndlessModeEnemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = EndlessGameMode.Instance.MainPlayer;
        enemy.SetDestination(player.transform.position);
        scanTimer = 1f;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.ResetNavMesh();
    }

    public override void Tick()
    {
        base.Tick();

        scanTimer -= Time.deltaTime;

        if(scanTimer < 0)
        {
            if (enemy.IsInMeleeRange(player))
            {
                enemy.CharacterStateMachine.ChangeState(enemy.AttackState);
            }
            else
            {
                enemy.CharacterStateMachine.ChangeState(enemy.MoveState);
            }
        }
    }
}
