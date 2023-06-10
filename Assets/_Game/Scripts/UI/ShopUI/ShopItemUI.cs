using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] Image backgroundImage;
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] WeaponData weaponData;
    public WeaponData GetWeaponData() { return weaponData; }
    [SerializeField] SkinData skinData;
    public SkinData GetSkinData() { return skinData; }
    private bool isSkin = false;
    public bool IsSkin { get {  return isSkin; } }
    private int price;
    public int Price { get { return price; } }

    [Space, Header("Setup Visual")]
    [SerializeField] Sprite defaulBackground;
    [SerializeField] Sprite activeBackground;
    
    private ShopPanelUI shopPanelUI;
    private bool isActive = false;
    private bool isPurchased = false;
    public bool IsPurchased { get => isPurchased; set => isPurchased = value; }
    public void SetActive(bool isActive)
    {
        this.isActive = isActive;   
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        shopPanelUI.SetActiveItem(this);
    }

    public void SetupWeaponItemUI(ShopPanelUI shopPanelUI, WeaponData data, bool isPurchased)
    {
        this.shopPanelUI = shopPanelUI;
        this.weaponData = data;
        this.price = data.price;
        this.isPurchased = isPurchased;

        backgroundImage.sprite = defaulBackground;
        itemIcon.sprite = data.weaponIcon;
        priceText.text = data.price.ToString();
    }

    public void SetupSkinItemUI(ShopPanelUI shopPanelUI, SkinData data, bool isPurchased)
    {
        this.shopPanelUI = shopPanelUI;
        this.skinData = data;
        this.isPurchased = isPurchased;
        this.price = data.price;
        isSkin = true;

        backgroundImage.sprite = defaulBackground;
        itemIcon.sprite = data.skinIcon;
        priceText.text = data.price.ToString();

    }

    public void UpdateVisual()
    {
        if (isActive)
        {
            backgroundImage.sprite = activeBackground;
        }
        else
        {
            backgroundImage.sprite = defaulBackground;
        }
    }
}
