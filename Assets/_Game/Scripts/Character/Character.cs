using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Character : MonoBehaviour, IDamageable
{
    public static event EventHandler<OnAnyCharacterSpawnProjectileArgs> onAnyCharacterSpawnProjectile;
    public class OnAnyCharacterSpawnProjectileArgs : EventArgs
    {
        public Vector3 destination;
        public int damage;
        public int projectileAmt;
        public EWeaponType weaponType;
    }


    [Space, Header("Character Setup")]
    [SerializeField] LayerMask characterLayer;
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected Animator anim;
    [SerializeField] protected Transform spawnProjectilePoint;
    [SerializeField] TextMeshPro nameText;
    public Transform SpawnProjectilePoint => spawnProjectilePoint;
    [SerializeField] protected Projectile weaponPrefab;
    [SerializeField] protected Transform attachWeaponPoint;
    [SerializeField] protected float attackRange = 5f;
    [SerializeField] protected int damage;
    [SerializeField] protected int projectileAmount = 1;
    public int ProjectileAmount => projectileAmount;
    [SerializeField] protected WeaponList weaponList;
    [SerializeField] Transform attachIndicatorPoint;
    public Transform AttachIndicatorPoint => attachIndicatorPoint;
    protected CharacterSkin characterSkin;

    [Space, Header("Character Info")]
    [SerializeField] protected int health = 100;
    [SerializeField] protected int maxHeath = 100;
    [SerializeField] protected int shield = 0;
    [SerializeField] protected bool isInvicible;
    [SerializeField] ParticleSystem hitVFX;
    [SerializeField] private int level = 0;
    public int Level => level;
    [SerializeField] private float scaleFactor = 0.05f;

    [SerializeField] EWeaponType weaponType = EWeaponType.AxeDouble;
    public EWeaponType WeaponType => weaponType;
    private Weapon equipedWeapon;

    protected bool isDead;
    public bool IsDead => isDead;

    protected CharacterPool pool;
    public CharacterPool Pool
    {
        get => pool;
        set => pool = value;
    }

    public Animator Anim => anim;

    protected StateMachine stateMachine;
    public StateMachine CharacterStateMachine => stateMachine;


    protected Collider[] targetsInRange = new Collider[10];

    public bool HasTargetInRange
    {
        get
        {
            return Physics.OverlapSphereNonAlloc(transform.position, attackRange, targetsInRange, characterLayer) > 1;
        }
    }
    public Character GetFirstTarget()
    {
        int i = 0;

        while (HasTargetInRange && i < targetsInRange.Length)
        {
            if (targetsInRange[i] == null) return null;

            //Character target = targetsInRange[i].gameObject.GetComponent<Character>();
            if(CachedObjects.TryGetCharacterByCollider(targetsInRange[i], out Character target))
            {
                if (target.gameObject.activeInHierarchy && target != this && !target.IsDead)
                {
                    return target;
                }
            }
            else
            {
                return null;
            }
            i++;

            //if(target == null) return null;
        }

        return null;
    }

    protected virtual void Start()
    {
        OnInit();
    }

    public virtual void OnNewGame()
    {
        OnInit();
    }

    protected virtual void OnInit()
    {
        if(stateMachine == null)
        {
            stateMachine = new StateMachine();
        }

        if (weaponList == null) return; //for debug only. TODO: remove before build

        maxHeath = health;

        characterSkin = GetComponent<CharacterSkin>();
        isDead = false;
        SetCharacterName();
    }

    protected virtual void SetCharacterName(string name = "Character_00")
    {
        nameText.text = name;
    }

    protected virtual void ChangeWeapon(EWeaponType newWeaponType)
    {
        weaponType = newWeaponType;
        if (equipedWeapon != null)
        {
            Destroy(equipedWeapon.gameObject);
        }
        equipedWeapon = Instantiate(weaponList.GetWeaponPrefab(weaponType).GetComponent<Weapon>(), attachWeaponPoint);
        equipedWeapon.OnEquip(this);
    }

    protected virtual void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.CurrentGameState != GameManager.GameState.Playing) return;

        if(stateMachine.CurrentState != null)
        {
            stateMachine.CurrentState.Tick();
        }
    }

    public void SpawnProjectile(Vector3 destination)
    {
        onAnyCharacterSpawnProjectile?.Invoke(this, new OnAnyCharacterSpawnProjectileArgs 
                                                                { 
                                                                        destination = destination,
                                                                        damage = damage,
                                                                        projectileAmt = projectileAmount,
                                                                        weaponType = weaponType                                                          
                                                                });
    }


    public void TakeDamage(int damageToTake, Character damageDealer)
    {
        if (isInvicible) return;
        if(isDead) return;

        if(shield > 0)
        {
            shield -= damageToTake;

            if(shield <= 0)
            {
                OnShieldDestroy();
            }
        }
        else
        {
            health -= damageToTake;
            hitVFX.Play();

            if (health <= 0)
            {
                health = 0;
                OnDead(damageDealer);
            }
        } 
    }

    protected virtual void OnShieldDestroy()
    {

    }

    protected virtual void OnDead(Character damageDealer)
    {
        isDead = true;
        AudioManager.Instance.PlayDeadSound(transform.position);
    }

    public void LookAtTarget(Vector3 target)
    {
        transform.LookAt(target);
        Vector3 angleEuler = transform.rotation.eulerAngles;
        angleEuler.x = 0f;
        transform.rotation = Quaternion.Euler(angleEuler);
    }

    public virtual void ReleaseSelf()
    {
        pool.ReturnToPool(this);
    }

    public void AppleEffect(EStatsType statType, float newValue, float duration)
    {
        if(duration > 0)
        {
            StartCoroutine(ModifyStatsByBoosterWithDuration(statType, newValue, duration));
        }
        else
        {
            ModifyStatsByBooster(statType, newValue);
        }
    }

    protected void ModifyStatsByBooster(EStatsType statType, float newValue)
    {
        switch (statType)
        {
            case EStatsType.AttackRange:
                attackRange = newValue;
                break;

            case EStatsType.MoveSpeed:
                moveSpeed = newValue;
                break;

            default:
                break;
        }
    }


    protected IEnumerator ModifyStatsByBoosterWithDuration(EStatsType statType ,float newValue, float duration)
    {
        float defaultValue = 0;

        switch (statType)
        {
            case EStatsType.AttackRange:
                defaultValue = attackRange;
                attackRange = newValue;
                break;

            case EStatsType.MoveSpeed:
                defaultValue = moveSpeed;
                moveSpeed = newValue;
                break;

            default:
                break;
        }

        yield return new WaitForSeconds(duration);

        switch (statType)
        {
            case EStatsType.AttackRange:
                attackRange = defaultValue;
                break;

            case EStatsType.MoveSpeed:
                moveSpeed = defaultValue;
                break;

            default:
                break;
        }
    }

    public void ModifyStatsByWeapon(float attackRange, int damage)
    {
        this.attackRange = attackRange;

        this.damage = damage;
    }

    public void ModifyStatsBySkin(float moveSpeed, float attackRange, float attackSpeed)
    {
        if(this.attackRange < attackRange)
        {
            this.attackRange = attackRange;
        }
        
        if(moveSpeed > 0)
        {
            this.moveSpeed = moveSpeed;
        }

        if(attackSpeed > 0)
        {
            //TODO: increase attack speed
        }
    }

    public void IncreaseLevel()
    {
        level++;
        transform.localScale = Vector3.one * (1 + level * scaleFactor);
    }

    protected void SetLevel(int newLevel)
    {
        level = newLevel;
        transform.localScale = Vector3.one * (1 + level * scaleFactor);
    }

    public float GetHealthPercent() => (float)health / maxHeath;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
