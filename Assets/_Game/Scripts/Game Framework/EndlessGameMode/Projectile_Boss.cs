using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Boss : MonoBehaviour
{
    [SerializeField] GameObject warningVisual;
    [SerializeField] GameObject vfx;
    [SerializeField] LayerMask characterMask;

    private float radius = 2f;
    private int damage = 10;

    private Character dealer;

    [SerializeField] private bool isActive = false;

    private float timer = 3f;

    private void Start()
    {
        vfx.SetActive(false);
    }

    public void SetupProjectile(float radius, int damage, Character damageDealer)
    {
        this.radius = radius;
        this.damage = damage;
        dealer = damageDealer;

        isActive = true;

        timer = 3f;
    }

    private void Update()
    {
        if (!isActive) return;

        timer -= Time.deltaTime;

        if(timer < 0f)
        {
            warningVisual.SetActive(false);
            vfx.SetActive(true);

            Invoke(nameof(ApplyDamage), 0.5f);

            Destroy(this.gameObject, 1.5f);

            isActive = false;
        }
    }

    private void ApplyDamage()
    {
        var colliders = Physics.OverlapSphere(transform.position, radius, characterMask);
        foreach(Collider collider in colliders)
        {
            if(collider.TryGetComponent<Character>(out var target))
            {
                if(target != dealer)
                {
                    target.TakeDamage(damage, dealer);
                }
            }
        }
    }
}
