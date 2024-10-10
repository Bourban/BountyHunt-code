using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviourHandler
{
    public enum BehaviourState
    {
        Sandboxing,
        SandboxScripted,
        Following,
        Fleeing,
        
    }

    //Needed?
    private Transform Target;
    private float TransformRadius;
    
    private BehaviourState CurrentState;
    private NPCBehaviour CurrentBehaviour;

    private readonly NPC OwningChar;

    public NPCBehaviourHandler(NPC owner, BehaviourState defaultState = BehaviourState.Sandboxing)
    {
        OwningChar = owner;
        //Default to sandbox behaviour.
        StartSandboxing();
    }

    // Update is called once per frame
    public void UpdateBehaviour()
    {
        CurrentBehaviour.UpdateBehaviour();
    }

    public bool FollowTarget(Transform target, float radius = 5.0f)
    {
        if (!CanSwitchToState(BehaviourState.Following))
        {
            return false;
        }
        
        CurrentBehaviour = new FollowBehaviour(OwningChar, target, radius);
        CurrentState = BehaviourState.Following;
        
        return true;
    }

    public bool StartScriptedIdle(IdleMarker marker)
    {
        if (!CanSwitchToState(BehaviourState.SandboxScripted))
        {
            return false;
        }

        CurrentBehaviour = new ScriptedIdleBehaviour(OwningChar, marker);
        CurrentState = BehaviourState.Sandboxing;
        return true;
    }
    
    public bool StartSandboxing()
    {
        if (!CanSwitchToState(BehaviourState.Sandboxing))
        {
            return false;
        }

        CurrentBehaviour = new SandboxBehaviour(OwningChar);
        CurrentState = BehaviourState.Sandboxing;
        return true;
    }
    
    public bool StartSandboxing(Transform target, float radius)
    {
        if (!CanSwitchToState(BehaviourState.Sandboxing))
        {
            return false;
        }

        CurrentBehaviour = new SandboxBehaviour(OwningChar, target, radius);
        CurrentState = BehaviourState.Sandboxing;
        return true;
    }
    
    
    private bool CanSwitchToState(BehaviourState newState)
    {
        // TODO 
        return true;
    }
    
    
    public bool SetState(BehaviourState newState)
    {
        switch (newState)
        {
            case BehaviourState.Sandboxing:
                CurrentState = newState;
                break;
            case BehaviourState.SandboxScripted:
                CurrentState = newState;
                break;
            default:
                return false;
        }
        
        return false;
    }
}
