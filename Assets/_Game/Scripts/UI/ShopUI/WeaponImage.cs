using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponImage : MonoBehaviour, IDragHandler
{
    private WeaponModelView weaponModels;

    public void SetUp(WeaponModelView weaponModels)
    {
        this.weaponModels = weaponModels;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(weaponModels == null || eventData.delta.sqrMagnitude < 0.1f) return;

        weaponModels.RotateModel(eventData.delta);
    }
}
