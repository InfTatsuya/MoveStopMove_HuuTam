using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringCollection 
{
    public static int IdleAnim = Animator.StringToHash("Idle");
    public static int MoveAnim = Animator.StringToHash("Move");
    public static int AttackAnim = Animator.StringToHash("Attack");
    public static int DeathAnim = Animator.StringToHash("Death");
    public static int DanceAnim = Animator.StringToHash("Dance");

    public static int MeleeAnim = Animator.StringToHash("Melee");
    public static int RangeAnim = Animator.StringToHash("Range");
    public static int TeleportAnim = Animator.StringToHash("Teleport");
    public static int BossAttackIndex = Animator.StringToHash("AttackIndex");
}
