using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmedState : PlayerState
{
    private static readonly int WeaponDrawn = Animator.StringToHash("WeaponDrawn");

    public ArmedState(PlayerControls controls, PlayerWeaponController weaponController) : base(controls, weaponController)
    {
    }

    public override void OnStateEnter()
    {
        Controls.UpperLayerTargetWeight = 1;
        Controls.CharAnimator.SetBool(WeaponDrawn, true);
    }

    public override void OnStateExit()
    {
        
    }

    void HandleWeaponInputs()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Controls.SetState(new AimingState(Controls, WeaponController));
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Controls.Reload();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Controls.SetState(new IdleState(Controls, WeaponController));
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            WeaponController.EquipWeapon(WeaponController.MainHandWeapon, true);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            WeaponController.EquipWeapon(WeaponController.BackWeapon, true);
        }
    }

    public override void Tick()
    {
        HandleWeaponInputs();
    }
}
