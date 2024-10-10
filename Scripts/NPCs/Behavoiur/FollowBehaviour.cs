using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowBehaviour : NPCBehaviour
{
    private bool IsInRange = false;
    private bool IsMoving = false;
    
    public FollowBehaviour(NPC parent, Transform target, float radius = 1.0f) : base(parent, target, radius)
    {
    }

    protected override void HandleGoals()
    {
        IsInRange = (Target.transform.position - ParentNPC.transform.position).magnitude < TargetRadius;

        if (IsMoving)
        {
            IsMoving = !ParentNPC.HasReachedTarget();
        }
        
        if (IsInRange || IsMoving)
        {
            return;
        }

        GetNewTargetPosition();
    }
    
    void GetNewTargetPosition()
    {
        // Maybe don't just do a random point? 
        RandomPoint(Target.transform.position, TargetRadius, out Vector3 randomPoint);
        NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 3.6f, NavMesh.AllAreas);

        ParentNPC.CharAgent.destination = hit.position;
        IsMoving = true;
    }
}
