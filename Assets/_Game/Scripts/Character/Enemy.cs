using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Character
{
    public static event EventHandler<OnAnyEnemyDeathArgs> onAnyEnemyDeath;
    public class OnAnyEnemyDeathArgs : EventArgs
    {
        public Enemy enemy;
        public Character damageDealer;
    }


    [Space, Header("Enemy Info")]
    [SerializeField] float idleTime = 3f;
    public float IdleTime => idleTime;
    [SerializeField] float patrolRadius = 7f;
    public float PatrolRadius => patrolRadius;
    [SerializeField] GameObject targetVisual;

    private NavMeshAgent agent;

    public bool IsPause { get; set; }


    #region States

    public EnemyIdleState IdleState { get; private set; }
    public EnemyMoveState MoveState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    public EnemyDeathState DeathState { get; private set; }
    #endregion

    protected override void OnInit()
    {
        base.OnInit();

        Debug.Log("OnInit enemy chay");
        IsPause = false;
        agent = GetComponent<NavMeshAgent>();

        IndicatorManager.Instance.CreateNewIndicator(this);

        IdleState = new EnemyIdleState(this, Anim, StringCollection.IdleAnim);
        MoveState = new EnemyMoveState(this, Anim, StringCollection.MoveAnim);
        AttackState = new EnemyAttackState(this, Anim, StringCollection.AttackAnim);
        DeathState = new EnemyDeathState(this, Anim, StringCollection.DeathAnim);

        SetEnemyAsTarget(false);
    }

    public override void OnNewGame()
    {
        base.OnNewGame();

        stateMachine.Initialize(IdleState);
        
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (IsPause) return;

        base.Update();
    }

    protected override void OnDead(Character damageDealer)
    {
        base.OnDead(damageDealer);

        ResetNavMesh();
        SetEnemyAsTarget(false);
        stateMachine.ChangeState(DeathState);
        onAnyEnemyDeath?.Invoke(this, new OnAnyEnemyDeathArgs { enemy = this, damageDealer = damageDealer });
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

        IndicatorManager.Instance.RemoveIndicator(this);
    }
}
