using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EndlessEnemyDeathState : State
{
    private EndlessModeEnemy enemy;
    private float timer = 2f;

    public EndlessEnemyDeathState(Character character, Animator anim, int animString) : base(character, anim, animString)
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

        timer -= Time.deltaTime;

        if (timer < 0f)
        {
            enemy.ReleaseSelf();
        }
    }
}
