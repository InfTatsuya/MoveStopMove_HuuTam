using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : State
{
    protected Player player;

    public PlayerState(Character character, Animator anim, int animString) : base(character, anim, animString)
    {
        player = character as Player;
    }
}
