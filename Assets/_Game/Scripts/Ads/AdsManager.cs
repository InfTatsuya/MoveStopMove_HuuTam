using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance { get; private set; }

    [SerializeField] InterstitialAds interstitialAds;
    [SerializeField] RewardedAds rewardedAds;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void LoadInterstitialAds()
    {
        interstitialAds.LoadAd();
    }

    public void LoadRewardAds()
    {
        rewardedAds.LoadAd();
    }
}
