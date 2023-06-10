using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public static event EventHandler<OnPickupItemArgs> onPickupItem;
    public class OnPickupItemArgs : EventArgs
    {
        public List<StatsBoostEffect> statsBoostEffects;
        public List<AbilityBooster> abilityBoosters;
    }


    [SerializeField] List<StatsBoostEffect> statsBoostEffects = new List<StatsBoostEffect>();
    [SerializeField] List<AbilityBooster> abilityBoosters = new List<AbilityBooster>();

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Player>(out var player))
        {
            onPickupItem?.Invoke(this, new OnPickupItemArgs { abilityBoosters = abilityBoosters, statsBoostEffects = statsBoostEffects });

            Destroy(this.gameObject);
        }
    }
}
