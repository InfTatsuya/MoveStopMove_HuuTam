using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerAttackState : PlayerState
{
    public PlayerAttackState(Character character, Animator anim, int animString) : base(character, anim, animString)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Character target;

        if (player.CheckHaveTargetAndInRange())
        {
            target = player.Target;
            player.LookAtTarget(target.transform.position);
        }
        else
        {
            player.ClearPreviousTarget();

            target = player.GetFirstTarget();
            if (target != null)
            {
                Enemy enemy = target.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.SetEnemyAsTarget(true);
                    player.Target = enemy;
                }
                player.LookAtTarget(target.transform.position);
            }
            else
            {
                player.CharacterStateMachine.ChangeState(player.IdleState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Tick()
    {
        base.Tick();

        if(player.MoveDirection.sqrMagnitude > 0.1f)
        {
            player.CharacterStateMachine.ChangeState(player.MoveState);
            return;
        }

        if (hasAnimTrigger)
        {
            hasAnimTrigger = false;

            player.CharacterStateMachine.ChangeState(player.IdleState);
            return;
        }
    }
}
