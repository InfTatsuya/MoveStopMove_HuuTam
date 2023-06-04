using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : State
{
    protected Enemy enemy;

    public EnemyState(Character character, Animator anim, int animString) : base(character, anim, animString)
    {
        enemy = character as Enemy;
    }
}
