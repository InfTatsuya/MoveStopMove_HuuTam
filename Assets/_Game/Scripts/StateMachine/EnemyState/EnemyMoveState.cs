using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveState : EnemyState
{
    public EnemyMoveState(Character character, Animator anim, int animString) : base(character, anim, animString)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Vector3 targetPos = new Vector3(Random.Range(-25f, 25f), 0f, Random.Range(-25f, 25f));
        enemy.SetDestination(targetPos);
        Debug.Log(targetPos);

        //NavMeshHit hit;
        //bool isFound = false;

        //while (!isFound)
        //{
        //    Vector3 targetPos = new Vector3(Random.Range(-25f, 25f), 0f, Random.Range(-25f, 25f));

        //    if (NavMesh.SamplePosition(targetPos, out hit, 0.1f, NavMesh.AllAreas))
        //    {
        //        isFound = true;
        //        Vector3 finalPosition = hit.position;
        //        enemy.SetDestination(finalPosition);
        //    }
        //}
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Tick()
    {
        base.Tick();

        if (enemy.IsAtDestination())
        {
            if (enemy.HasTargetInRange)
            {
                enemy.CharacterStateMachine.ChangeState(enemy.AttackState);
            }
            else
            {
                enemy.CharacterStateMachine.ChangeState(enemy.IdleState);
            }
        }    
    }
}
