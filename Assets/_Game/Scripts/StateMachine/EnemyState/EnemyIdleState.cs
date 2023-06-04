using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private float idleTime;

    public EnemyIdleState(Character character, Animator anim, int animString) : base(character, anim, animString)
    {
    }

    public override void Enter()
    {
        base.Enter();
        idleTime = enemy.IdleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Tick()
    {
        base.Tick();

        idleTime -= Time.deltaTime;

        if(idleTime < 0f)
        {
            enemy.CharacterStateMachine.ChangeState(enemy.MoveState);
        }
    }
}
