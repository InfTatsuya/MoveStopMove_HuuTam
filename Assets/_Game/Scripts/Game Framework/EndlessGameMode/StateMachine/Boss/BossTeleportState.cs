using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTeleportState : State
{
    private EndlessModeBoss boss;

    public BossTeleportState(Character character, Animator anim, int animString) : base(character, anim, animString)
    {
        boss = character as EndlessModeBoss;
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

            boss.CharacterStateMachine.ChangeState(boss.MoveState);
            return;
        }
    }

}
