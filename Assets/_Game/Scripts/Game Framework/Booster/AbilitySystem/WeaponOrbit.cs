using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponOrbit : MonoBehaviour
{
    [SerializeField] List<DealDamageObject> damageObjects;

    private float speed = 30f;
    private Transform owner;

    public void SetUpSkill(int damage, int amt, Character owner)
    {
        for(int i = 0; i < amt; i++)
        {
            damageObjects[i].gameObject.SetActive(true);
            damageObjects[i].SetUp(damage, owner);        
        }

        this.owner = owner.transform;
    }

    private void Update()
    {
        transform.Rotate(0f, speed * Time.deltaTime, 0f);
        transform.position = owner.position;
    }
}
