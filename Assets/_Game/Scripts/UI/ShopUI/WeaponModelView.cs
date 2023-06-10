using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModelView : MonoBehaviour
{
    [SerializeField] List<GameObject> weaponModels;

    private int activeIndex = -1;

    private void Start()
    {
        DeactiveAll();
    }

    private void DeactiveAll()
    {
        foreach(var weaponModel in weaponModels)
        {
            weaponModel.SetActive(false);
        }
    }

    private void DeactiveModel(int index)
    {
        weaponModels[index].SetActive(false);
    }

    public void SetActiveModel(int index)
    {
        if(activeIndex >= 0)
        {
            DeactiveModel(activeIndex);
        }

        activeIndex = index;
        weaponModels[index].SetActive(true);
    }

    public void RotateModel(Vector2 eulerAngleXY)
    {
        if(Mathf.Abs(eulerAngleXY.x) > Mathf.Abs(eulerAngleXY.y))
        {
            transform.Rotate(0f, eulerAngleXY.x, 0f);
        }
        else
        {
            transform.Rotate(eulerAngleXY.y, 0f, 0f);
        }
    }
}
