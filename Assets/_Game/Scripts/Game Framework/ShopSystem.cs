using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSystem : MonoBehaviour, ISaveManager
{
    public static ShopSystem Instance { get; private set; }

    public event EventHandler<OnEquipWeaponArgs> onEquipWeapon;
    public class OnEquipWeaponArgs : EventArgs
    {
        public WeaponData weaponData;
    }

    public event EventHandler<OnEquipSkinArgs> onEquipSkin;
    public class OnEquipSkinArgs : EventArgs
    {
        public SkinData skinData;
    }

    [Space, Header("Player Currency")]
    [SerializeField] int money = 10000;
    public int Money => money;

    //Weapon List
    [SerializeField] WeaponList sellWeaponList;
    public List<WeaponData> GetSellWeaponList() 
    { 
        List<WeaponData> sellList = new List<WeaponData>();

        foreach(var weapon in sellWeaponList.weaponList)
        {
            if (!purchasedWeaponList.Contains(weapon.data))
            {
                sellList.Add(weapon.data);
            }
        }

        return sellList; 
    }

    //Skin List
    [SerializeField] List<SkinData> sellSkinDataList;
    public List<SkinData> GetSellSkinList() => sellSkinDataList;
    

    private List<WeaponData> purchasedWeaponList = new List<WeaponData>();
    private List<SkinData> purchasedSkinList = new List<SkinData>();

    //for save system
    private List<EWeaponType> weaponSaveList = new List<EWeaponType>();
    private List<int> skinSaveList = new List<int>();   

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        Enemy.onAnyEnemyDeath += Enemy_onAnyEnemyDeath;
    }

    private void Enemy_onAnyEnemyDeath(object sender, Enemy.OnAnyEnemyDeathArgs e)
    {
        if (e.damageDealer as Player == null) return;

        (e.damageDealer as Player).IncreaseLevel();
        GameManager.Instance.IncreaseKillCount();

        int amtMoney = UnityEngine.Random.Range(0, 50);
        AddMoney(amtMoney);
    }

    private void AddMoney(int amt)
    {
        money += amt;
    }

    public bool TryPurchaseItem(int price)
    {
        if(money >= price)
        {
            money -= price;
            UIManager.Instance.UpdateInfoOnScreen();
            return true;
        }

        return false;
    }

    public void AddWeaponToPlayer(WeaponData weaponData)
    {
        if (purchasedWeaponList.Contains(weaponData)) return;

        purchasedWeaponList.Add(weaponData);
        
    }

    public void EquipWeapon(WeaponData weaponData)
    {
        onEquipWeapon?.Invoke(this, new OnEquipWeaponArgs { weaponData = weaponData });
    }

    public void AddSkinToPlayer(SkinData skinData)
    {
        if(purchasedSkinList.Contains(skinData)) return;

        purchasedSkinList.Add(skinData);
        
    }

    public void EquipSkin(SkinData skinData)
    {
        onEquipSkin?.Invoke(this, new OnEquipSkinArgs { skinData = skinData });
    }

    public void LoadData(GameData data)
    {
        money = data.money;

        weaponSaveList = data.weaponList;
        skinSaveList = data.skinIdList;
    }

    public void SaveData(ref GameData data)
    {
        data.money = money;

        data.weaponList = new List<EWeaponType>();
        foreach(WeaponData weaponData in purchasedWeaponList)
        {
            data.weaponList.Add(weaponData.weaponType);
        }

        data.skinIdList = new List<int>();
        foreach(SkinData skinData in purchasedSkinList)
        {
            data.skinIdList.Add(skinData.itemId);
        }
    }

    public bool CheckHasPurchasedWeapon(WeaponData data)
    {
        foreach(var value in weaponSaveList)
        {
            if(data.weaponType == value) 
                return true;
        }

        return false;
    }

    public bool CheckHasPurchasedSkin(SkinData data)
    {
        foreach (var value in skinSaveList)
        {
            if (data.itemId == value)
                return true;
        }

        return false;
    }
}
