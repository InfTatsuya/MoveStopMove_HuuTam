using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Booster/StatsBooster", fileName = "Booster_")]
public class StatsBoostEffect : BoosterEffect
{
    [Space, Header("Stats Info")]
    public EStatsType statsType;
    public float amtToBoost;

    public override void ApplyEffect(Character character)
    {
        character.AppleEffect(statsType, amtToBoost, duration);
    }

    public override void RemoveEffect(Character character)
    {
        
    }
}

public enum EStatsType
{
    AttackRange,
    MoveSpeed,
    Damage,
    none
}
