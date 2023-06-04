using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Skin Data", fileName ="SkinData_")]
public class SkinData : ScriptableObject
{
    public string itemName;
    public int itemId;
    public ESkinType skinType;
    public float moveSpeed;
    public float attackRange;
    public float attackSpeed;

    public GameObject headModel;
    public GameObject shieldModel;
    public GameObject wingModel;
    public GameObject leftHandWeaponModel;
    public Texture2D paintTexture;

    public Sprite skinIcon;
    public int price;

    public void OnEquip(Character character)
    {
        if(character != null)
        {
            character.ModifyStatsBySkin(moveSpeed, attackRange, attackSpeed);
        }
    }

    private void OnValidate()
    {
        switch (skinType)
        {
            case ESkinType.Pant:
                headModel = null;
                shieldModel = null;
                wingModel = null;
                leftHandWeaponModel = null;
                break;

            case ESkinType.Head:
                shieldModel = null;
                wingModel = null;
                paintTexture = null;
                leftHandWeaponModel = null;
                break;

            case ESkinType.Shield:
                headModel = null;
                wingModel = null;
                paintTexture = null;
                leftHandWeaponModel = null;
                break;

            case ESkinType.FullSet:
                break;

            default:
                break;

        }
    }
}



public enum ESkinType
{
    Pant,
    Head,
    Shield,
    FullSet,
    None
}
