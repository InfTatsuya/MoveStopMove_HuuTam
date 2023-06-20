using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessEnemyAttackState : State
{
    private EndlessModeEnemy enemy;

    public EndlessEnemyAttackState(Character character, Animator anim, int animString) : base(character, anim, animString)
    {
        enemy = character as EndlessModeEnemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Tick()
    {
        base.Tick();

        if (hasAnimTrigger)
        {
            hasAnimTrigger = false;

            enemy.CharacterStateMachine.ChangeState(enemy.MoveState);
            return;
        }
    }
}
