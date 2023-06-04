using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] WeaponData weaponData;

    //[SerializeField] Mesh mesh;
    //public Mesh GetMesh() { return mesh; }

    //[Space, Header("Weapon Info")]
    //[SerializeField] int damage = 20;
    //public int Damage => damage;

    //[SerializeField] float attackRange = 5f;
    ////public float AttackRate => attackRange; 

    public void OnEquip(Character character)
    {
        character.ModifyStatsByWeapon(weaponData.attackRange, weaponData.damage);
    }

}
