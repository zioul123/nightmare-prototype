using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
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
        // Change to follow state if in range
        if (parent.Target != null) {
            parent.ChangeState(new FollowState());
        }
    }
}
