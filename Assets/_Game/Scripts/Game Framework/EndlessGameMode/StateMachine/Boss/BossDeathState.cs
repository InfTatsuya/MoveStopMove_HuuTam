using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeathState : State
{
    float timer = 3f;

    public BossDeathState(Character character, Animator anim, int animString) : base(character, anim, animString)
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

        timer -= Time.deltaTime;

        if (timer < 0f)
        {
            character.ReleaseSelf();
        }
    }
}
