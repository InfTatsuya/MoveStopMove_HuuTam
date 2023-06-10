using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoosterIconUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Image boosterIcon;
    [SerializeField] Image chooseImage;
    [SerializeField] TextMeshProUGUI descriptionText;

    private BoosterPopupPanel boosterPanel;
    private BoosterEffect boosterEffect;
    public BoosterEffect BoosterEffect => boosterEffect;
    private bool isAbility;
    private bool isActive;

    public void OnPointerDown(PointerEventData eventData)
    {
        boosterPanel.SetCurrentActiveUI(this);
    }

    public void SetUpBoosterIcon(BoosterEffect boosterEffect, bool isAbility, BoosterPopupPanel panel)
    {
        this.boosterEffect = boosterEffect;
        this.isAbility = isAbility;
        boosterPanel = panel;

        boosterIcon.sprite = boosterEffect.icon;
        descriptionText.text = boosterEffect.effectDescription;

        SetIsActive(false);
    }

    private void UpdateVisual()
    {
        chooseImage.gameObject.SetActive(isActive);
    }

    public void SetIsActive(bool isActive)
    {
        this.isActive = isActive;
        UpdateVisual();
    }

    
}
