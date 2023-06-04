using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SO/Weapon Data", fileName ="WeaponData_")]
public class WeaponData : ScriptableObject
{
    public WeaponModelsList weaponModelList;

    [Space, Header("Weapon Info")]
    public EWeaponType weaponType;
    public GameObject weaponModel;
    public int damage;
    public float attackRange;

    [Space, Header("Info for Shop")]
    public int price;
    public Sprite weaponIcon;

    private void OnValidate()
    {
        weaponModel = weaponModelList.GetModelsByType(weaponType);
    }
}


public enum EWeaponType
{
    Hammer,
    Arrow,
    Axe,
    AxeDouble,
    Knife,
    Canndy
}
