using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMeleeState : State
{
    private EndlessModeBoss boss;
    private Player player;

    public BossMeleeState(Character character, Animator anim, int animString) : base(character, anim, animString)
    {
        boss = character as EndlessModeBoss;
        player = EndlessGameMode.Instance.MainPlayer;

        int random = Random.Range(0, 2);
        boss.Anim.SetInteger(StringCollection.BossAttackIndex, random);
    }

    public override void Enter()
    {
        base.Enter();
        boss.LookAtTarget(player.transform.position);
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
