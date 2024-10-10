using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class NPCBehaviour 
{
    protected readonly NPC ParentNPC;

    // An Optional target the individual behaviours can use differently, for a Follow behaviour it might be the GameObject
    // they're following, for a Sandbox one, it might be the center of the area they can roam, with a radius of TargetRadius.
    protected Transform Target;
    // Optional float individual behaviours can use as a threshold for how near (or far) they might need to be from Target.
    protected float TargetRadius;

    protected NPCBehaviour(NPC parent) 
    {
        ParentNPC = parent;

        if(ParentNPC == null) 
        {
            Debug.LogError("NPC Behavoiur script created with no parent NPC!");
        }
    }
    
    protected NPCBehaviour(NPC parent, Transform target, float radius = 1.0f) 
    {
        ParentNPC = parent;
        Target = target;
        TargetRadius = radius;

        if(ParentNPC == null) 
        {
            Debug.LogError("NPC Behavoiur script created with no parent NPC!");
        }
    }

    protected virtual bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
    
    public virtual void SetTargetTransform(Transform transform)
    {
        Target = transform;
    }

    public virtual void SetTargetRadius(float radius)
    {
        TargetRadius = radius;
    }

    public virtual void UpdateBehaviour() 
    {
        HandleGoals();
    }

    protected abstract void HandleGoals();

}
