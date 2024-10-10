using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingState : NPCBehaviour
{
    private Transform FollowGoal;
    
    public FollowingState(NPC parent) : base(parent)
    {
    }

    public void SetFollowGoal(Transform t)
    {
        FollowGoal = t;
    }
    
    protected override void HandleGoals()
    {
        
    }
}
