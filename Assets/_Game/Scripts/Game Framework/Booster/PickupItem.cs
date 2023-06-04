using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] bool isStatsBooster = true;

    [SerializeField] List<StatsBoostEffect> statsBoostEffects = new List<StatsBoostEffect>();
    [SerializeField] List<AbilityBooster> abilityBoosters = new List<AbilityBooster>();

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Player>(out var player))
        {
            if (isStatsBooster)
            {
                foreach (var effect in statsBoostEffects)
                {
                    effect.ApplyEffect(player);
                }
            }
            else
            {
                foreach (var effect in abilityBoosters)
                {
                    effect.ApplyEffect(player);
                }
            }

            Destroy(this.gameObject);
        }
    }

    private void OnValidate()
    {
        if (isStatsBooster)
        {
            abilityBoosters.Clear();
        }
        else
        {
            statsBoostEffects.Clear();
        }
    }
}
