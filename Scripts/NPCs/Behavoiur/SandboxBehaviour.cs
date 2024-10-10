using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SandboxBehaviour : NPCBehaviour
{
    enum CurrentBehaviourState
    {
        Deafault,
        Moving,
        Idling,
        MarkerIdling
    }

    CurrentBehaviourState CurrentState;

    private const int TotalIdles = 4;
    int IdleIndex;
    float IdleStartTime;
    float CurrentIdleDuration;

    private bool HasTargetArea = false;

    //Idle Marker vars
    IdleMarker CurrentIdle;
    bool IsPlayingIdleAnim = false;
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Index = Animator.StringToHash("Idle Index");

    public SandboxBehaviour(NPC parent) : base(parent)
    {
    }

    public SandboxBehaviour(NPC parent, Transform target, float radius = 1.0f) : base(parent, target, radius)
    {
    }
    
    protected override void HandleGoals()
    {
        switch (CurrentState)
        {
            case CurrentBehaviourState.Deafault:
                ChooseNewSandboxGoal();
                break;
            case CurrentBehaviourState.Moving:
                if (ParentNPC.HasReachedTarget())
                {
                    CurrentState = CurrentBehaviourState.Deafault;
                }
                break;
            case CurrentBehaviourState.MarkerIdling:
                UseIdleMarkerUpdate();
                break;
            case CurrentBehaviourState.Idling:
                if (Time.time - IdleStartTime >= CurrentIdleDuration)
                {
                    CurrentState = CurrentBehaviourState.Deafault;
                }
                break;
            default:
                break;
        }

        //TODO: Check if still sandboxing, playing idle etc.

    }

    void ChooseNewSandboxGoal()
    {
        int choice = Random.Range(0, 3);

        switch (choice)
        {
            case 0:
                GetNewTarget();
                break;
            case 1:
                PlayGenericIdle();
                break;
            case 2:
                StartUseIdleMarker();
                break;
            default:
                break;
        }
    }

    void PlayGenericIdle()
    {
        IdleIndex = Random.Range(0, TotalIdles);

        ParentNPC.CharAnimator.SetInteger(Index, IdleIndex);
        ParentNPC.CharAnimator.SetTrigger(Idle);

        CurrentState = CurrentBehaviourState.Idling;

        IdleStartTime = Time.time;
        CurrentIdleDuration = 4.0f;
    }

    void StartUseIdleMarker()
    {
        if (ParentNPC.ShouldUseIdentity)
        {
            CurrentIdle = SandboxManager.Instance.GetAvailableIdleMarker(ParentNPC.Identity);
        }
        else
        {
            CurrentIdle = SandboxManager.Instance.GetAvailableIdleMarker();
        }

        if (CurrentIdle == null)
        {
            //Fall back on a generic idle if no available markers.
            PlayGenericIdle();
            return;
        }

        NavMesh.SamplePosition(CurrentIdle.transform.position, out NavMeshHit hit, 3.6f, NavMesh.AllAreas);

        ParentNPC.CharAgent.destination = hit.position;
        CurrentState = CurrentBehaviourState.MarkerIdling;
    }

    void UseIdleMarkerUpdate()
    {
        if (!ParentNPC.HasReachedTarget())
        {
            return;
        }

        if (!IsPlayingIdleAnim)
        {
            ParentNPC.CharAnimator.SetTrigger(CurrentIdle.ClipTrigger);
            IsPlayingIdleAnim = true;
            IdleStartTime = Time.time;

            ParentNPC.transform.LookAt(CurrentIdle.LookTarget);
            ParentNPC.transform.position = CurrentIdle.transform.position;
        }
        else
        {
            if (Time.time - IdleStartTime >= CurrentIdle.AnimDuration)
            {
                IsPlayingIdleAnim = false;

                if (CurrentIdle.LinkedMarker == null)
                {
                    CurrentState = CurrentBehaviourState.Deafault;
                    CurrentIdle.IsAvailable = true;
                }
                else
                {
                    CurrentIdle.IsAvailable = true;
                    CurrentIdle = CurrentIdle.LinkedMarker;

                    NavMesh.SamplePosition(CurrentIdle.transform.position, out NavMeshHit hit, 3.6f, NavMesh.AllAreas);
                    ParentNPC.CharAgent.destination = hit.position;
                }
            }
        }
    }

    void GetNewTarget()
    {
        RandomPoint(ParentNPC.transform.position, 10.0f, out Vector3 randomPoint);
        NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 3.6f, NavMesh.AllAreas);

        ParentNPC.CharAgent.destination = hit.position;
        CurrentState = CurrentBehaviourState.Moving;

        //For now just pick a new random spot to go to.
    }
}
