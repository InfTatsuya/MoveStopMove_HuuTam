using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EndlessModeBoss : Character
{
    public static event EventHandler onBossDead;


    [Space, Header("Boss Info")]
    [SerializeField] int meleeDamage = 20;
    public int MeleeDamage => meleeDamage;
    [SerializeField] float meleeRange = 1f;
    [SerializeField] Collider meleeAttackTrigger;
    [SerializeField] GameObject targetVisual;

    private NavMeshAgent agent;

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

        //stateMachine.Initialize(MoveState);
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
        else
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
}
