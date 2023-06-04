using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Weapon List", fileName = "WeaponList_")]
public class WeaponList : ScriptableObject
{
    public List<WeaponDataPair> weaponList = new List<WeaponDataPair>();

    public GameObject GetWeaponPrefab(EWeaponType weaponType)
    {
        foreach(var weapon in weaponList)
        {
            if(weapon.data.weaponType == weaponType)
            {
                return weapon.prefab;
            }
        }

        return null;
    }
}

[System.Serializable]
public class WeaponDataPair
{
    public WeaponData data;
    public GameObject prefab;
}
