using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTransfer : MonoBehaviour
{
    public static DataTransfer Instance { get; private set; }

    private List<SkinData> playerSkinDataList = new List<SkinData>();
    public List<SkinData> PlayerSkinDataList
    {
        get
        {
            return playerSkinDataList;
        }
    }


    private EWeaponType playerEquipWeapon;
    public EWeaponType PlayerEquipWeapon => playerEquipWeapon;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SetPlayerData()
    {
        playerEquipWeapon = GameManager.Instance.MainPlayer.WeaponType;
        playerSkinDataList = GameManager.Instance.MainPlayer.GetComponent<CharacterSkin>().GetCurrentEquipSkinData();
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
