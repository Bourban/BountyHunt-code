using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerControls : MonoBehaviour
{
    ////////////// Serialised Fields
    [SerializeField]
    private Transform CameraTransform;
    [SerializeField]
    public CrosshairHandler Crosshair;
    
    //IK Target
    [SerializeField]
    public Transform IKAimTarget;
    public Vector3 CurrentCursorPosition;

    /////////////// Movement vars
    private readonly float RotationSpeed = 15.0f;
    private readonly float MaxSpeed = 2.5f;
    ///////////////

    ////////////// Anim Vars
    [HideInInspector]
    public float IkRigTargetWeight = 0.0f;
    [HideInInspector]
    public float UpperLayerTargetWeight = 0.0f;

    private float UpperLayerCurrentWeight = 0.0f;

    private PlayerState CurrentState;

    //Other Player components -- making some public so they're accessible by the PlayerStates.
    [HideInInspector]
    public Animator CharAnimator;
    private Rig IkRig;

    private WeaponIK CharWeaponIk;

    private PlayerWeaponController WeaponController;
    private CharacterController Controller;

    private PlayerEquipType WeaponEquipType;
    
    private static readonly int IsMovingTrigger = Animator.StringToHash("IsMoving");
    private static readonly int ReloadTrigger = Animator.StringToHash("Reload");
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");

    // Start is called before the first frame update
    void Start()
    {
        SetupComponents();
        
        SetState(new IdleState(this, WeaponController));
        SetIkWeight(0);
    }

    private void Update()
    {
        UpdateMovement();
        UpdateAnimVars();
        UpdateCursorPosition();
        CurrentState.Tick();
    }

    void UpdateMovement()
    {
        Vector3 direction = HandleMovementInput();
        Vector3 movement = CameraTransform.forward * direction.z + CameraTransform.right * direction.x;

        HandleRotation(movement);

        movement *= MaxSpeed;
        
        CharAnimator.SetBool(IsMovingTrigger, direction != Vector3.zero);
        //CharAnimator.SetFloat(MoveX,);
        CharAnimator.SetFloat(MoveY, Mathf.Lerp(0, 0.5f, Controller.velocity.magnitude / MaxSpeed));
        
        movement += Physics.gravity;

        Controller.Move(movement * Time.deltaTime);
    }

    void HandleRotation(Vector3 direction)
    {
        //TODO: If aiming, look forward & strafe.

        direction = Vector3.RotateTowards(transform.forward, direction, RotationSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(direction);
    }

    Vector3 HandleMovementInput()
    {
        return new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
    }

    public void Reload() 
    {
        WeaponController.Reload();
        CharAnimator.SetTrigger(ReloadTrigger);
    }

    public void Shoot() 
    {
        WeaponController.Shoot(IKAimTarget.position);
    }

    public void SetState(PlayerState state)
    {
        if (CurrentState != null)
        {
            CurrentState.OnStateExit();
        }

        CurrentState = state;

        if (CurrentState != null)
        {
            CurrentState.OnStateEnter();
        }
    }

    private void UpdateCursorPosition()
    {
        IKAimTarget.position = Vector3.Lerp(IKAimTarget.position, CurrentCursorPosition, 15 * Time.deltaTime);
    }
    
    private void UpdateAnimVars()
    {

        //IkRig.weight = Mathf.Lerp(IkRig.weight, IkRigTargetWeight, 25 * Time.deltaTime);
        
        UpperLayerCurrentWeight = Mathf.Lerp(UpperLayerCurrentWeight, UpperLayerTargetWeight, 50 * Time.deltaTime);
        CharAnimator.SetLayerWeight(1, UpperLayerCurrentWeight);
    }

    public void ToggleEquipWeapon(bool val)
    {
        WeaponController.ToggleEquipWeapon(val);
    }

    public void SetIkWeight(float weight)
    {
        CharWeaponIk.Weight = weight;
    }
    
    private void SetupComponents()
    {
        WeaponController = GetComponent<PlayerWeaponController>();
        Controller = GetComponent<CharacterController>();
        CharWeaponIk = GetComponent<WeaponIK>();
        
        CharAnimator = GetComponentInChildren<Animator>();
        IkRig = GetComponentInChildren<Rig>();
    }
}
