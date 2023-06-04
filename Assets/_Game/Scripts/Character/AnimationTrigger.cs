using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AnimationTrigger : MonoBehaviour
{
    [SerializeField] Character myCharacter;

    public void SpawnProjectile()
    {
        Character target = myCharacter.GetFirstTarget();

        if (target != null)
        {
            myCharacter.SpawnProjectile(target.transform.position);
        }
    }

    public void AttackFinishTrigger()
    {
        myCharacter.CharacterStateMachine.CurrentState.SetAnimTrigger(true);
    }
}
