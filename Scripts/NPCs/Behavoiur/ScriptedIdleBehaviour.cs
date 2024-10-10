using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedIdleBehaviour : NPCBehaviour
{
    // This'll do the job, but it's pretty ugly having to have a serialised IdleMarker in the parent.
    // This is really just placeholder.

    readonly IdleMarker Marker;

    bool IsPlayingIdle;
    private static readonly int EndIdle = Animator.StringToHash("EndIdle");

    public ScriptedIdleBehaviour(NPC parent, IdleMarker marker = null) : base(parent)
    {
        Marker = marker;

        if(Marker != null) 
        {
            StartUsingMarker();
        }
        else 
        {
            //maybe destroy this.
        }
    }

    void StartUsingMarker() 
    {
        ParentNPC.CharAgent.destination = Marker.transform.position;
        Marker.IsAvailable = false;
    }

    protected override void HandleGoals()
    {
        if(IsPlayingIdle == false) 
        {
            if (ParentNPC.HasReachedTarget() == false)
            {
                return;
            }

            ParentNPC.CharAnimator.SetTrigger(Marker.ClipTrigger);
            ParentNPC.transform.LookAt(Marker.LookTarget);
            ParentNPC.transform.position = Marker.transform.position;
            IsPlayingIdle = true;
        }
    }

    ~ScriptedIdleBehaviour() 
    {
        Marker.IsAvailable = true;
        ParentNPC.CharAnimator.SetTrigger(EndIdle);
    }

}
