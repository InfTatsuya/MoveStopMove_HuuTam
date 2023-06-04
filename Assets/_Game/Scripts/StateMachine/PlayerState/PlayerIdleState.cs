using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Character character, Animator anim, int animString) : base(character, anim, animString)
    {
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

        if(player.MoveDirection.sqrMagnitude > 0)
        {
            player.CharacterStateMachine.ChangeState(player.MoveState);
            return;
        }
        else if (player.GetFirstTarget() != null)
        {
            player.CharacterStateMachine.ChangeState(player.AttackState);
            return;
        }
        else
        {
            player.ClearPreviousTarget();
        }
    }
}
