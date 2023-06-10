using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public string playerName;
    public int money;
    public int starAmt;

    public List<EWeaponType> weaponList;
    public List<int> skinIdList;

    public bool isMusicOn;
    public bool isVibrationOn;
}
