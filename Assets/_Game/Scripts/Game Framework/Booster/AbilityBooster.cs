using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Booster/Ability Booster", fileName = "Ability_")]
public class AbilityBooster : BoosterEffect
{
    public EAbilityType abilityType;
    public int amt = 0;
    public GameObject abilityPrefab;

    public override void ApplyEffect(Character character)
    {
        Player player = character as Player;
        if(player != null)
        {
            player.AddAbility(this);
        }
    }

    public override void RemoveEffect(Character character)
    {
        
    }

    private void OnValidate()
    {
        if(abilityType == EAbilityType.Invincible)
        {
            amt = 0;
        }
    }
}

public enum EAbilityType
{
    MultipleThrow,
    Shield,
    WeaponOrbit,
    Invincible,
    None
}
