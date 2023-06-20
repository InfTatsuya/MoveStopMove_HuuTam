using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanelUI : MonoBehaviour
{
    [SerializeField] Button buyButton;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] Button equipButton;
    [SerializeField] Button closeButton;
    [SerializeField] ShopItemUI shopItemUIPrefab;
    [SerializeField] Transform parentUI;
    [SerializeField] TextMeshProUGUI descriptionText;

    [Space, Header("For Weapon Shop")]
    [SerializeField] WeaponModelView weaponModels;
    [SerializeField] WeaponImage weaponImage;

    [Space, Header("For Skin Shop")]
    [SerializeField] bool isSkinShop;
    [SerializeField] Transform headPanel;
    [SerializeField] Transform pantPanel;
    [SerializeField] Transform shieldPanel;
    [SerializeField] Transform fullSetPanel;
    [SerializeField] CharacterSkin modelSkinPreview;

    [SerializeField] List<ShopItemUI> itemsUIList = new List<ShopItemUI>();

    private ShopItemUI currentActiveItemUI;

    private void Start()
    {
        buyButton.onClick.AddListener(OnBuyItem);
        equipButton.onClick.AddListener(OnEquipItem);
        closeButton.onClick.AddListener(DeactiveSelf);

        equipButton.gameObject.SetActive(false);

        if (!isSkinShop)
        {
            foreach (WeaponData data in ShopSystem.Instance.GetSellWeaponList())
            {
                ShopItemUI itemUI = Instantiate(shopItemUIPrefab, parentUI);
                itemUI.SetupWeaponItemUI(this, data, ShopSystem.Instance.CheckHasPurchasedWeapon(data));
                itemsUIList.Add(itemUI);
            }
        }
        else
        {
            foreach(SkinData data in ShopSystem.Instance.GetSellSkinList())
            {
                ShopItemUI itemUI = Instantiate(shopItemUIPrefab, GetParentBySkinType(data.skinType));
                itemUI.SetupSkinItemUI(this, data, ShopSystem.Instance.CheckHasPurchasedSkin(data));
                itemsUIList.Add(itemUI);
            }
        }
    }

    private Transform GetParentBySkinType(ESkinType skinType)
    {
        switch(skinType)
        {
            case ESkinType.Pant:
                return pantPanel;

            case ESkinType.Head:
                return headPanel;

            case ESkinType.Shield:
                return shieldPanel;

            case ESkinType.FullSet:
                return fullSetPanel;

            default:
                return parentUI;
        }
    }

    public void SetActiveItem(ShopItemUI itemUI)
    {
        if(currentActiveItemUI != null)
        {
            currentActiveItemUI.SetActive(false);
        }

        currentActiveItemUI = itemUI;
        currentActiveItemUI.SetActive(true);

        UpdateVisualModel();

        if (itemUI.IsPurchased)
        {
            equipButton.gameObject.SetActive(true);
        }
        else
        {
            equipButton.gameObject.SetActive(false);
            priceText.text = itemUI.Price.ToString();
        }

        foreach(var item in itemsUIList)
        {
            item.UpdateVisual();
        }
    }

    private void UpdateVisualModel()
    {
        if (!currentActiveItemUI.IsSkin)
        {
            WeaponData data = currentActiveItemUI.GetWeaponData();
            descriptionText.text = data.GetDescription();
            weaponModels.SetActiveModel((int)data.weaponType);
            weaponImage.SetUp(weaponModels);
        }
        else
        {
            SkinData data = currentActiveItemUI.GetSkinData();
            descriptionText.text = data.GetDescription();
            modelSkinPreview.ChangeSkin(data, null);
        }
    }

    private void OnBuyItem()
    {
        if (currentActiveItemUI == null) return;

        if (!currentActiveItemUI.IsSkin)
        {
            if (ShopSystem.Instance.TryPurchaseItem(currentActiveItemUI.GetWeaponData().price))
            {
                ShopSystem.Instance.AddWeaponToPlayer(currentActiveItemUI.GetWeaponData());

                currentActiveItemUI.IsPurchased = true;
                SetActiveItem(currentActiveItemUI);
            }
        }
        else
        {
            if (ShopSystem.Instance.TryPurchaseItem(currentActiveItemUI.GetSkinData().price))
            {
                ShopSystem.Instance.AddSkinToPlayer(currentActiveItemUI.GetSkinData());

                currentActiveItemUI.IsPurchased = true;
                SetActiveItem(currentActiveItemUI);

                //itemsUIList.Remove(currentActiveItemUI);
                //Destroy(currentActiveItemUI.gameObject);
                //currentActiveItemUI = null;
            }
        }
    }

    private void OnEquipItem()
    {
        if (currentActiveItemUI == null) return;

        if (!currentActiveItemUI.IsSkin)
        {
            ShopSystem.Instance.EquipWeapon(currentActiveItemUI.GetWeaponData());
        }
        else
        {
            ShopSystem.Instance.EquipSkin(currentActiveItemUI.GetSkinData());
        }
    }

    private void DeactiveSelf()
    {
        this.gameObject.SetActive(false);
        UIManager.Instance.DeactivateModelView();
    }
}
