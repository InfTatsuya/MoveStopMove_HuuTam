using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkin : MonoBehaviour
{
    [SerializeField] bool isPreview;

    [SerializeField] Transform headAttachPoint;
    [SerializeField] Transform wingAttachPoint;
    [SerializeField] Transform shieldAttachPoint;
    [SerializeField] Transform leftHandWeaponAttachPoint;

    [SerializeField] GameObject body;
    [SerializeField] GameObject pants;

    [SerializeField] SkinData pantSkinData;
    [SerializeField] SkinData headSkinData;
    [SerializeField] SkinData shieldSkinData;
    [SerializeField] SkinData skinDataFullSet;

    private GameObject headSkin;
    private GameObject wingSkin;
    private GameObject shieldSkin;
    private GameObject leftHandWeapon;

    private bool isEquipSet;

    public void ChangeSkin(SkinData skinData, Character character)
    {
        EquipSkin(skinData);

        if(!isPreview)
        {
            skinData.OnEquip(character);
        }
        else
        {
            SetPreview();
        }
    }

    private void SetPreview()
    {
        if(headSkin != null)
        {
            headSkin.layer = 8;
        } 
        if(wingSkin != null)
        {
            wingSkin.layer = 8;
        }
        if(shieldSkin != null)
        {
            shieldSkin.layer = 8;
        }
        if(leftHandWeapon != null)
        {
            leftHandWeapon.layer = 8;
        }
    }

    private void EquipSkin(SkinData skinData)
    {
        if (isEquipSet)
        {
            Destroy(headSkin.gameObject);
            headSkin = null;

            Destroy(wingSkin.gameObject);
            wingSkin = null;

            Destroy(leftHandWeapon.gameObject);
            leftHandWeapon = null;

            pants.GetComponent<Renderer>().material.SetTexture("_MainTex", null);
            pantSkinData = null;

            isEquipSet = false;
        }

        switch (skinData.skinType)
        {
            case ESkinType.Pant:
                pantSkinData = skinData;
                pants.GetComponent<Renderer>().material.SetTexture("_MainTex", skinData.paintTexture);
                break;

            case ESkinType.Head:
                headSkinData = skinData;
                if(headSkin != null)
                {
                    Destroy(headSkin.gameObject); 
                }

                headSkin = Instantiate(skinData.headModel, headAttachPoint);
                break;

            case ESkinType.Shield:
                shieldSkinData = skinData;
                if (shieldSkin != null)
                {
                    Destroy(shieldSkin.gameObject);
                }

                shieldSkin = Instantiate(skinData.shieldModel, shieldAttachPoint);
                break;

            case ESkinType.FullSet:
                EquipSetSkin(skinData);
                break;

            default:
                break;
        }
    }

    private void EquipSetSkin(SkinData skinData)
    {
        skinDataFullSet = skinData;
        isEquipSet = true;

        if(skinData.paintTexture != null)
        {
            pants.GetComponent<Renderer>().material.SetTexture("_MainTex", skinData.paintTexture);
        }

        if(skinData.headModel != null)
        {
            if (headSkin != null)
            {
                Destroy(headSkin.gameObject);
            }

            headSkin = Instantiate(skinData.headModel, headAttachPoint);
        }

        if(skinData.wingModel != null)
        {
            if (wingSkin != null)
            {
                Destroy(wingSkin.gameObject);
            }

            wingSkin = Instantiate(skinData.wingModel, wingAttachPoint);
        }

        if(skinData.leftHandWeaponModel != null)
        {
            if (leftHandWeapon != null)
            {
                Destroy(leftHandWeapon.gameObject);
            }

            leftHandWeapon = Instantiate(skinData.leftHandWeaponModel, leftHandWeaponAttachPoint);
        }
    }
}
