using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(Character character, Animator anim, int animString) : base(character, anim, animString)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Character target = enemy.GetFirstTarget();
        if (target != null)
        {
            enemy.LookAtTarget(target.transform.position);
        }
        else
        {
            enemy.CharacterStateMachine.ChangeState(enemy.IdleState);
        }
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

            enemy.CharacterStateMachine.ChangeState(enemy.IdleState);
            return;
        }
    }
}
