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
    [SerializeField] Transform tailAttachPoint;

    [SerializeField] GameObject body;
    [SerializeField] GameObject pants;

    [SerializeField] SkinData bodySkinData;
    [SerializeField] SkinData pantSkinData;
    [SerializeField] SkinData headSkinData;
    [SerializeField] SkinData shieldSkinData;
    [SerializeField] SkinData skinDataFullSet;

    private GameObject headSkin;
    private GameObject wingSkin;
    private GameObject shieldSkin;
    private GameObject leftHandWeapon;
    private GameObject tailSkin;

    private bool isEquipSet;

    public List<SkinData> GetCurrentEquipSkinData()
    {
        if (isEquipSet)
        {
            return new List<SkinData>() { skinDataFullSet };
        }
        else
        {
            return new List<SkinData>() { bodySkinData, pantSkinData, headSkinData, shieldSkinData };
        }
    }

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
            foreach(Transform child in wingSkin.transform)
            {
                child.gameObject.layer = 8;
            }
        }
        if(shieldSkin != null)
        {
            shieldSkin.layer = 8;
        }
        if(leftHandWeapon != null)
        {
            leftHandWeapon.layer = 8;
        }
        if(tailSkin != null)
        {
            tailSkin.layer = 8;
        }
    }

    private void EquipSkin(SkinData skinData)
    {
        if(pants == null || body == null)
        {
            Destroy(this);
        }

        if (isEquipSet)
        {
            RemoveSkinSet();
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

    private void RemoveSkinSet()
    {
        if(headSkin != null)
        {
            Destroy(headSkin.gameObject);
            headSkin = null;
        }
        
        if(wingSkin != null)
        {
            Destroy(wingSkin.gameObject);
            wingSkin = null;
        }
        
        if(leftHandWeapon != null)
        {
            Destroy(leftHandWeapon.gameObject);
            leftHandWeapon = null;
        }
        
        if(tailSkin != null)
        {
            Destroy(tailSkin.gameObject);
            tailSkin = null;
        }

        if(body == null)
        {
            Destroy(this);
        }
        else
        {
            body.GetComponent<Renderer>().material.SetTexture("_MainTex", null);
            bodySkinData = null;
        }
        
        if(pants == null)
        {
            Destroy(this);
        }
        else
        {
            pants.GetComponent<Renderer>().material.SetTexture("_MainTex", null);
            pantSkinData = null;
        }
        

        isEquipSet = false;
    }

    private void EquipSetSkin(SkinData skinData)
    {
        RemoveSkinSet();

        skinDataFullSet = skinData;
        isEquipSet = true;

        if(skinData.paintTexture != null)
        {
            pants.GetComponent<Renderer>().material.SetTexture("_MainTex", skinData.paintTexture);
        }

        if(skinData.bodyTexture != null)
        {
            body.GetComponent<Renderer>().material.SetTexture("_MainTex", skinData.bodyTexture);
        }

        if(skinData.headModel != null)
        {
            headSkin = Instantiate(skinData.headModel, headAttachPoint);
        }

        if(skinData.wingModel != null)
        {
            wingSkin = Instantiate(skinData.wingModel, wingAttachPoint);
        }

        if(skinData.leftHandWeaponModel != null)
        {
            leftHandWeapon = Instantiate(skinData.leftHandWeaponModel, leftHandWeaponAttachPoint);
        }

        if(skinData.tailModel != null)
        {
            tailSkin = Instantiate(skinData.tailModel, tailAttachPoint);
        }
    }
}
