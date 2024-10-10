using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowDirectBehaviour : NPCBehaviour
{
    public FollowDirectBehaviour(NPC parent, Transform target, float radius = 1.0f) : base(parent, target, radius)
    {
        if (target == null)
        {
            Debug.LogError("Invalid Target given to FollowDirectBehaviour.");
        }
    }

    protected override void HandleGoals()
    {
        NavMesh.SamplePosition(Target.transform.position + new Vector3(0, 0, 2), out NavMeshHit hit, 3.6f, NavMesh.AllAreas);

        if (hit.position.magnitude > 0)
        {
            ParentNPC.CharAgent.destination = hit.position;
        }
    }
}
