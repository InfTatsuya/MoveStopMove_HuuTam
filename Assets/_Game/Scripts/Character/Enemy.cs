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
    [SerializeField] EnemyDataList enemyDataList;
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

        if(IndicatorManager.Instance != null)
        {
            IndicatorManager.Instance.CreateNewIndicator(this);
        }

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

        SetupEnemyInfo();
        health = maxHeath;
    }

    private void SetupEnemyInfo()
    {
        int randomIndex = UnityEngine.Random.Range(0, enemyDataList.enemyNames.Count);
        int randomLevel = UnityEngine.Random.Range(0, enemyDataList.maxLevel);
        int randomWeapon = UnityEngine.Random.Range(0, enemyDataList.weaponTypeList.Count);
        int randomSkin = UnityEngine.Random.Range(0, enemyDataList.skinDataList.Count);
        SkinData skinData = enemyDataList.skinDataList[randomSkin];

        SetCharacterName(enemyDataList.enemyNames[randomIndex]);
        SetLevel(randomLevel);
        ChangeWeapon(enemyDataList.weaponTypeList[randomWeapon]);
        characterSkin.ChangeSkin(skinData, this);
    }


    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (true)
        {

        }
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
        onAnyEnemyDeath?.Invoke(this, new OnAnyEnemyDeathArgs { enemy = this, damageDealer = damageDealer });
    }

    Vector3 desPoint;
    public void SetDestination(Vector3 dest)
    {
        desPoint = dest;
        agent.SetDestination(dest);
    }

    public void ResetNavMesh()
    {
        agent.ResetPath();
    }

    //public bool IsAtDestination() => !agent.pathPending && !agent.hasPath;
    public bool IsAtDestination() => Vector3.Distance(transform.position, desPoint) < 0.1f + Mathf.Abs(transform.position.y - desPoint.y);

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
