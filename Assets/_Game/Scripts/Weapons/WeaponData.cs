using System.Collections;
using System.Collections.Generic;
using System.Text;
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

    public string GetDescription()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append($"+{damage} DAMAGE");
        stringBuilder.AppendLine();
        stringBuilder.Append($"+{attackRange} ATTACK RANGE");

        return stringBuilder.ToString();
    }
}


public enum EWeaponType
{
    Hammer,
    Arrow,
    Axe,
    AxeDouble,
    Knife,
    Canndy,
    Boomerang
}
