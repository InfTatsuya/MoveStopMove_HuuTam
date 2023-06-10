using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoosterPopupPanel : MonoBehaviour
{
    [SerializeField] Player player;

    [SerializeField] List<BoosterIconUI> iconUIList;

    [SerializeField] Button acceptButton;
    [SerializeField] Button rerollButton;

    private BoosterIconUI currentActiveUI;

    private List<StatsBoostEffect> statsBooster = new List<StatsBoostEffect>();
    private List<AbilityBooster> abilityBoosters = new List<AbilityBooster>();

    private void Start()
    {
        PickupItem.onPickupItem += PickupItem_onPickupItem;

        acceptButton.onClick.AddListener(ApplyBoosterEffect);
        rerollButton.onClick.AddListener(RollAbility);

        this.gameObject.SetActive(false);
    }

    private void PickupItem_onPickupItem(object sender, PickupItem.OnPickupItemArgs e)
    {
        this.gameObject.SetActive(true);

        statsBooster.Clear();
        statsBooster = e.statsBoostEffects;
        abilityBoosters.Clear();
        abilityBoosters = e.abilityBoosters;

        RollAbility();
    }

    private void RollAbility()
    {
        int random1 = Random.Range(0, statsBooster.Count);
        int random2 = Random.Range(0, abilityBoosters.Count);

        iconUIList[0].SetUpBoosterIcon(statsBooster[random1], false, this);
        iconUIList[1].SetUpBoosterIcon(abilityBoosters[random2], false, this);
    }

    public void SetCurrentActiveUI(BoosterIconUI newBoosterIconUI)
    {
        if (currentActiveUI != null)
        {
            currentActiveUI.SetIsActive(false);
        }

        currentActiveUI = newBoosterIconUI;
        currentActiveUI.SetIsActive(true);
    }

    private void ApplyBoosterEffect()
    {
        if(currentActiveUI != null)
        {
            currentActiveUI.BoosterEffect.ApplyEffect(player);
        }

        this.gameObject.SetActive(false);
    }
}
