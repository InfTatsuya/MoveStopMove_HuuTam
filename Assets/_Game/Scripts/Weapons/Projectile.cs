using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 0.2f;
    [SerializeField] int damage = 20;
    [SerializeField] EWeaponType weaponType;
    [SerializeField] GameObject weaponModel;

    [SerializeField] GameObject playerTrailVFX;
    [SerializeField] GameObject enemyTrailVFX;

    [SerializeField] List<GameObject> modelsList = new List<GameObject>();

    private bool needSpin = true;

    private ProjectilePool pool;
    public ProjectilePool Pool
    {
        get => pool;
        set => pool = value;
    }

    private Vector3 destination;
    private float progress;
    private IDamageable dealer;

    public void SetupProjectile(Vector3 destination, IDamageable damageDealer, int damage, EWeaponType weaponType)
    {
        this.damage = damage;

        this.destination = destination;
        this.destination.y = transform.position.y;

        progress = 0;
        dealer = damageDealer;

        if(dealer as Player != null)
        {
            playerTrailVFX.gameObject.SetActive(true);
            enemyTrailVFX.gameObject.SetActive(false);
        }
        else
        {
            playerTrailVFX.gameObject.SetActive(false);
            enemyTrailVFX.gameObject.SetActive(true);
        }

        this.weaponType = weaponType;
        if(weaponType == EWeaponType.Arrow)
        {
            needSpin = false;
        }

        SetupVisualModel();

        AudioManager.Instance.PlayThrowWeaponSound(transform.position);
    }

    private void SetupVisualModel()
    {
        foreach(var model in modelsList)
        {
            model.SetActive(false);
        }

        modelsList[(int)weaponType].SetActive(true);
    }

    private void Update()
    {
        progress += Time.deltaTime * speed;
        transform.position = Vector3.LerpUnclamped(transform.position, destination, progress);

        if (needSpin)
        {
           weaponModel.transform.Rotate(0f, 5f, 0f);
        }

        if(progress > 1.3f)
        {
            ReleaseSelf();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IDamageable>(out var target))
        {
            if (target == dealer) return;
           
            Character damageDealer = dealer as Character;
            target.TakeDamage(damage, damageDealer);
            AudioManager.Instance.PlayHitSound(other.transform.position);

            ReleaseSelf();
        }
    }

    public void ReleaseSelf()
    {
        pool.ReturnToPool(this);
    }
}
