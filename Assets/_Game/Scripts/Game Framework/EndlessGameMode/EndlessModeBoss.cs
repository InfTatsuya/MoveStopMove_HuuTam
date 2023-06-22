using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Random = UnityEngine.Random;

public class EndlessModeBoss : Character
{
    public static event EventHandler onBossDead;

    [Space, Header("Boss Info")]
    [SerializeField] GameObject targetVisual;
    [SerializeField] int meleeDamage = 20;
    public int MeleeDamage => meleeDamage;
    [SerializeField] float meleeRange = 1f;
    [SerializeField] Collider meleeAttackTrigger;
    [SerializeField] Projectile_Boss bossRangeAttackPrefab;
    [SerializeField] float rangeAttackRadius = 3f;
    [SerializeField] int rangeAttackDamage = 30;
    [SerializeField] int rangeAttackAmount = 5;

    [Space, Header("Teleport Setup")]
    [SerializeField] CapsuleCollider bossCollider;
    [SerializeField] List<GameObject> bossModels;

    private NavMeshAgent agent;
    private WaitForSeconds rangeAttackDelay = new WaitForSeconds(0.5f);

    public bool IsCastingRangeAttack { get; private set; }

    public bool IsPause { get; set; }

    #region States
    public BossMoveState MoveState { get; private set; }
    public BossMeleeState MeleeState { get; private set; }
    public BossRangeState RangeState { get; private set; }
    public BossTeleportState TeleportState { get; private set; }
    public BossDeathState DeathState { get; private set; }
    #endregion

    protected override void OnInit()
    {
        base.OnInit();

        IsPause = false;
        agent = GetComponent<NavMeshAgent>();

        MoveState = new BossMoveState(this, Anim, StringCollection.MoveAnim);
        MeleeState = new BossMeleeState(this, Anim, StringCollection.MeleeAnim);
        RangeState = new BossRangeState(this, Anim, StringCollection.RangeAnim);
        TeleportState = new BossTeleportState(this, Anim, StringCollection.TeleportAnim);
        DeathState = new BossDeathState(this, Anim, StringCollection.DeathAnim);

        SetEnemyAsTarget(false);
    }

    public override void OnNewGame()
    {
        base.OnNewGame();

        stateMachine.Initialize(MoveState);
        meleeAttackTrigger.gameObject.SetActive(false);
    }


    protected override void Start()
    {
        base.Start();

        Invoke(nameof(OnNewGame), 1f); //for testing. TODO: remove before build
    }

    protected override void Update()
    {
        if (IsPause)
        {
            agent.isStopped = true;
            return;
        }
        else if(agent.enabled == true)
        {
            agent.isStopped = false;
        }

        base.Update();
    }

    protected override void OnDead(Character damageDealer)
    {
        base.OnDead(damageDealer);

        ResetNavMesh();
        SetEnemyAsTarget(false);
        stateMachine.ChangeState(DeathState);
        onBossDead?.Invoke(this, EventArgs.Empty);
    }

    public void SetDestination(Vector3 dest)
    {
        agent.SetDestination(dest);
    }

    public void ResetNavMesh()
    {
        agent.ResetPath();
    }

    public bool IsAtDestination() => !agent.pathPending && !agent.hasPath;

    public void SetEnemyAsTarget(bool isTarget)
    {
        if (stateMachine.CurrentState == DeathState) return;
        targetVisual.SetActive(isTarget);
    }

    public override void ReleaseSelf()
    {
        base.ReleaseSelf();

        // IndicatorManager.Instance.RemoveIndicator(this);
    }

    public bool IsInMeleeRange(Character target)
    {
        return Vector3.Distance(transform.position, target.transform.position) <= meleeRange;
    }

    public void MeleeAttack()
    {
        meleeAttackTrigger.gameObject.SetActive(true);

        StartCoroutine(ResetAttackTrigger());
    }

    private IEnumerator ResetAttackTrigger()
    {
        yield return new WaitForSeconds(0.3f);
        meleeAttackTrigger.gameObject.SetActive(false);
    }

    public void BossRangeAttack(Vector3 playerPos)
    {
        if (IsCastingRangeAttack) return;

        StartCoroutine(BossRangeAttackRoutine(playerPos));  
    }

    private IEnumerator BossRangeAttackRoutine(Vector3 playerPos)
    {
        IsCastingRangeAttack = true;

        Projectile_Boss proj1 = Instantiate(bossRangeAttackPrefab, playerPos, Quaternion.identity);
        proj1.SetupProjectile(rangeAttackRadius, rangeAttackDamage, this);

        yield return rangeAttackDelay;

        for(int i = 1; i < rangeAttackAmount; i++)
        {
            Vector3 pos = playerPos + Random.insideUnitSphere * 5f;
            pos.y = 0f;

            Projectile_Boss proj = Instantiate(bossRangeAttackPrefab, pos, Quaternion.identity);
            proj.SetupProjectile(rangeAttackRadius, rangeAttackDamage, this);

            yield return rangeAttackDelay;
        }

        IsCastingRangeAttack = false;
    }

    public void Teleport(Vector3 pos)
    {
        StartCoroutine(TeleportRoutine(pos));
        
    }

    private IEnumerator TeleportRoutine(Vector3 pos)
    {
        bossCollider.enabled = false;
        agent.enabled = false;
        SetBossVisible(false);

        transform.position = pos;
        Debug.Log("Teleport to:" + pos);

        yield return rangeAttackDelay;

        bossCollider.enabled = true;
        agent.enabled = true;
        agent.ResetPath();
        SetBossVisible(true);
    }

    private void SetBossVisible(bool isVisible)
    {
        foreach(var model in bossModels)
        {
            model.SetActive(isVisible);
        }
    }
}
