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
        if (!parent.IsDead && parent.Target != null) {
            parent.Direction = (parent.Target.transform.position - parent.transform.position).normalized;
            // transform.position = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
        // Unfollow
        } else {
            parent.ChangeState(new IdleState());
        }
    }
}
