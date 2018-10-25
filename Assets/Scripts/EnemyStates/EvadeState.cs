using UnityEngine;
using System.Collections;

public class EvadeState : IState
{
    private Enemy parent;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
        parent.Target = null;
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
    }

    public void Update()
    {
        parent.Direction = (parent.StartPosition - parent.transform.position).normalized;
        if (Vector2.Distance(parent.transform.position, parent.StartPosition) <= 0.1) {
            parent.ChangeState(new IdleState());
        }
    }
}
