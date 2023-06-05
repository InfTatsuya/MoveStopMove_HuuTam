using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RevivePanel : MonoBehaviour
{
    [SerializeField] Image cooldownImage;
    [SerializeField] TextMeshProUGUI cooldownText;
    [SerializeField] Button acceptButton;

    private float timer;
    private bool cancelTimeout;

    private void OnEnable()
    {
        timer = 5f;
        cooldownText.text = timer.ToString();
        cooldownImage.fillAmount = 1f;
        acceptButton.onClick.AddListener(AcceptRevive);

        cancelTimeout = false;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        cooldownImage.fillAmount = timer / 5f;
        cooldownText.text = Mathf.RoundToInt(timer).ToString();

        if(timer < 0f && !cancelTimeout)
        {
            OnTimeOut();
        }
    }

    private void OnDisable()
    {
        acceptButton.onClick.RemoveListener(AcceptRevive);
    }

    private void AcceptRevive()
    {
        cancelTimeout = true;
        this.gameObject.SetActive(false);
        UIManager.Instance.SwitchToIngameUI();

        AdsManager.Instance.LoadRewardAds();
    }

    private void OnTimeOut()
    {
        this.gameObject.SetActive(false);
        UIManager.Instance.SwitchToLosePanel();
    }
}
