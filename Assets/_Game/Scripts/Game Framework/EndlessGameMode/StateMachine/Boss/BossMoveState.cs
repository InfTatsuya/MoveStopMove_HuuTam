using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoveState : State
{
    private EndlessModeBoss boss;
    private Player player;

    private float timer;

    public BossMoveState(Character character, Animator anim, int animString) : base(character, anim, animString)
    {
        boss = character as EndlessModeBoss;
        //player = EndlessGameMode.Instance.MainPlayer;
    }

    public override void Enter()
    {
        base.Enter();

        Vector3 target = player.transform.position;
        boss.SetDestination(target);

        timer = 1f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Tick()
    {
        base.Tick();

        timer -= Time.deltaTime;

        if( timer < 0f )
        {
            if (boss.IsInMeleeRange(player))
            {
                boss.CharacterStateMachine.ChangeState(boss.MeleeState);
            }
            else if (boss.GetHealthPercent() < 0.2f)
            {
                boss.CharacterStateMachine.ChangeState(boss.TeleportState);
            }
            else if(boss.GetHealthPercent() < 0.6f)
            {
                boss.CharacterStateMachine.ChangeState(boss.RangeState);
            }
            else
            {
                boss.CharacterStateMachine.ChangeState(boss.MoveState);
            }
        }
    }
}
