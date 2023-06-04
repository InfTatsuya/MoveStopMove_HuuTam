using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageObject : MonoBehaviour
{
    private int damage;
    private Character owner;

    public void SetUp(int damage, Character owner)
    {
        this.damage = damage;
        this.owner = owner;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IDamageable>(out var target))
        {
            target.TakeDamage(damage, owner);
        }
    }
}
