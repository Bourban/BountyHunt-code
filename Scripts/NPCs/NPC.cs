using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class NPC : MonoBehaviour
{

    [SerializeField]
    private float RunSpeed;
    [SerializeField]
    private float WalkSpeed;
    
    public Animator CharAnimator;
    private Rigidbody CharRigidbody;

    public NavMeshAgent CharAgent;

    private Vector2 SmoothDeltaPosition = Vector2.zero;
    private Vector2 Velocity = Vector2.zero;

    public CharacterIdentity Identity;
    public bool ShouldUseIdentity = false;

    NPCBehaviourHandler BehaviourHandler;

    private bool IsDead = false;
    
    // Temp
    [SerializeField]
    IdleMarker OverrideMarker;
    [SerializeField] 
    private bool ShouldFollow;
    [SerializeField] 
    private Transform FollowTarget;

    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private static readonly int MoveY = Animator.StringToHash("MoveY");

    // Start is called before the first frame update
    private void Start()
    {
        DefaultSetupComponents();

        CharAgent.updatePosition = false;
        CharAgent.speed = WalkSpeed;

        SandboxManager.Instance.RegisterNPC(this);
        BehaviourHandler = new NPCBehaviourHandler(this);

        if(OverrideMarker != null)
        {
            BehaviourHandler.StartScriptedIdle(OverrideMarker);
        }

        if (ShouldFollow)
        {
            BehaviourHandler.FollowTarget(FollowTarget, 4.0f);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (IsDead == false)
        {
            BehaviourHandler.UpdateBehaviour();
            HandleAnimatorVars();
        }
    }

    private void HandleAnimatorVars() 
    {
        Vector3 worldDeltaPosition = CharAgent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        SmoothDeltaPosition = Vector2.Lerp(SmoothDeltaPosition, deltaPosition, smooth);

        // Don't divide by 0!
        if (Time.deltaTime > 1e-5f)
            Velocity = SmoothDeltaPosition / Time.deltaTime;

        bool shouldMove = Velocity.magnitude > 0.5f && CharAgent.remainingDistance > CharAgent.radius;

        // Update animation parameters
        CharAnimator.SetBool(IsMoving, shouldMove);
        CharAnimator.SetFloat(MoveX, Velocity.normalized.x * 0.2f);
        CharAnimator.SetFloat(MoveY, Velocity.normalized.y * 0.2f);

        //TODO
        //GetComponent<LookAt>().lookAtTargetPosition = CharAgent.steeringTarget + transform.forward;
    }

    public bool HasReachedTarget() 
    {
        // Check if we've reached the destination
        if (!CharAgent.pathPending)
        {
            if (CharAgent.remainingDistance <= CharAgent.stoppingDistance)
            {
                if (!CharAgent.hasPath || CharAgent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void OnAnimatorMove()
    {
        // Update position to agent position
        transform.position = CharAgent.nextPosition;
    }

    private void DefaultSetupComponents()
    {
        if (CharAgent == null)
        {
            CharAgent = GetComponent<NavMeshAgent>();
        }
        if (CharAnimator == null)
        {
            CharAnimator = GetComponent<Animator>();
        }
        if (CharRigidbody == null)
        {
            CharRigidbody = GetComponent<Rigidbody>();
        }
    }

    public void Die()
    {
        CharAgent.enabled = false;
        IsDead = true;
    } 
}


[Flags]
public enum CharacterIdentity
{
    //Bitmask -- Make sure these are multpiles of 2.
    None = 0,
    Wanderer = 1,
    Lawman = 2,
    Barkeep = 4,
    SaloonPatron = 8,
}

public static class CharacterIdentityExtensions
{
    static CharacterIdentity AddSlot(this CharacterIdentity self, CharacterIdentity other)
    {
        return self | other;
    }

    static CharacterIdentity RemoveSlot(this CharacterIdentity self, CharacterIdentity other)
    {
        return self & ~other;
    }

    public static bool HasFlag(this CharacterIdentity self, CharacterIdentity flag)
    {
        return (self & flag) == flag;
    }

}
