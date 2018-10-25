using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private Enemy parent;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {

    }

    public void Update()
    {
        // If cooldown is up and it's not attacking, attack.
        if (parent.TimeSinceLastAttack >= parent.AttackCooldown && !parent.IsAttacking) {
            parent.StartCoroutine(Attack());

            parent.TimeSinceLastAttack = 0;
        }

        // If there is still a target
        if (parent.Target != null) {
            // Check if it needs to start moving or it can just stay in attack state
            float distance = Vector2.Distance(parent.Target.transform.position, parent.transform.position);
            Debug.Log(distance);
            if (distance >= parent.AttackRange + parent.ExtraRange && !parent.IsAttacking) { // Only change if it's not currently attacking
                parent.ChangeState(new FollowState());
            }
        } else {
            parent.ChangeState(new IdleState());
        }
    }

    public IEnumerator Attack () 
    {
        parent.IsAttacking = true;
        parent.SetAttackLayer(Character.AttackLayer.MeleeLayer);
        parent.Animator.SetTrigger("attack");
        parent.IsMeleeing = true;

        yield return new WaitForSeconds(parent.Animator.GetCurrentAnimatorStateInfo(2).length); // Animation time

        parent.IsAttacking = false;
        parent.IsMeleeing = false;

    }
}
