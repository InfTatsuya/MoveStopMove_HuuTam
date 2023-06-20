using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Enemy/Data List", fileName = "EnemyDataList_")]
public class EnemyDataList : ScriptableObject
{
    public List<string> enemyNames;
    public int maxLevel;
    public List<SkinData> skinDataList;
    public List<EWeaponType> weaponTypeList;
}
