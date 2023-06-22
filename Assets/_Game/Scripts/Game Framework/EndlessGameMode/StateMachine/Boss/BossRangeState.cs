using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRangeState : State
{
    private EndlessModeBoss boss;
    private Player player;

    float timer = 1f;

    public BossRangeState(Character character, Animator anim, int animString) : base(character, anim, animString)
    {
        boss = character as EndlessModeBoss;
        player = EndlessGameMode.Instance.MainPlayer;
    }

    public override void Enter()
    {
        base.Enter();
        timer = 1f;

        boss.BossRangeAttack(player.transform.position);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Tick()
    {
        base.Tick();

        timer -= Time.deltaTime;

        if(timer < 0f)
        {
            timer = 1f;

            if (!boss.IsCastingRangeAttack)
            {
                boss.CharacterStateMachine.ChangeState(boss.MoveState);
            }
        }
    }
}
