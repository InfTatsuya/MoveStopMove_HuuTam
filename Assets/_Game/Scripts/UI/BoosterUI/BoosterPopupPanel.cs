using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoosterPopupPanel : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] bool isEndlessMode = false;

    [SerializeField] List<BoosterIconUI> iconUIList;

    [SerializeField] Button acceptButton;
    [SerializeField] Button rerollButton;

    private BoosterIconUI currentActiveUI;

    private List<StatsBoostEffect> statsBooster = new List<StatsBoostEffect>();
    private List<AbilityBooster> abilityBoosters = new List<AbilityBooster>();

    private List<BoosterEffect> endlessEffectList = new List<BoosterEffect>();

    private void Start()
    {
        Debug.Log("Powerup Start");
        PickupItem.onPickupItem += PickupItem_onPickupItem;
        if(isEndlessMode)
        {
            EndlessGameMode.Instance.onPlayerClearWave += EndlessGameMode_onPlayerClearWave;
        }

        acceptButton.onClick.AddListener(ApplyBoosterEffect);
        rerollButton.onClick.AddListener(RollAbility);

        this.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        PickupItem.onPickupItem -= PickupItem_onPickupItem;

        if (isEndlessMode)
        {
            EndlessGameMode.Instance.onPlayerClearWave -= EndlessGameMode_onPlayerClearWave;
        }
    }

    private void EndlessGameMode_onPlayerClearWave(object sender, System.EventArgs e)
    {
        this.gameObject.SetActive(true);
        endlessEffectList.Clear();
        
        RollAbility();
    }

    private void PickupItem_onPickupItem(object sender, PickupItem.OnPickupItemArgs e)
    {
        this.gameObject.SetActive(true);
        GameManager.Instance.PauseGame();

        statsBooster.Clear();
        statsBooster = e.statsBoostEffects;
        abilityBoosters.Clear();
        abilityBoosters = e.abilityBoosters;

        RollAbility();
    }

    private void RollAbility()
    {
        if (!isEndlessMode)
        {
            int random1 = Random.Range(0, statsBooster.Count);
            int random2 = Random.Range(0, abilityBoosters.Count);

            iconUIList[0].SetUpBoosterIcon(statsBooster[random1], false, this);
            iconUIList[1].SetUpBoosterIcon(abilityBoosters[random2], true, this);
        }
        else
        {
            endlessEffectList = EndlessGameMode.Instance.GetRandomEffect();
            iconUIList[0].SetUpBoosterIcon(endlessEffectList[0], false, this);
            iconUIList[1].SetUpBoosterIcon(endlessEffectList[1], false, this);
            iconUIList[2].SetUpBoosterIcon(endlessEffectList[2], false, this);
        }
        
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

        if (!isEndlessMode)
        {
            GameManager.Instance.ResumeGame();
        }
        else
        {
            EndlessGameMode.Instance.ResumeGame();
        }
    }
}
