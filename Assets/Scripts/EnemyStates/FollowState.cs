using UnityEngine;
using System.Collections;

public class FollowState : IState
{
    private Enemy parent;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
    }

    public void Update()
    {
        // Following
        if (!parent.IsDead && parent.Target != null) {
            parent.Direction = (parent.Target.transform.position - parent.transform.position).normalized;
            // transform.position = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime * speed);

            float distance = Vector2.Distance(parent.Target.transform.position, parent.transform.position);
            if (distance <= parent.AttackRange) {
                parent.ChangeState(new AttackState());
            }
        }
        // Unfollow
        if (!parent.InRange){
            parent.ChangeState(new IdleState());
        }
    }
}
