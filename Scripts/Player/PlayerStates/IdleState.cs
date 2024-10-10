using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerState
{
    private static readonly int WeaponDrawn = Animator.StringToHash("WeaponDrawn");

    public IdleState(PlayerControls controls, PlayerWeaponController weaponController) : base(controls, weaponController)
    {
    }

    public override void OnStateEnter() 
    {
        Controls.ToggleEquipWeapon(false);
        
        Controls.UpperLayerTargetWeight = 0;
        Controls.CharAnimator.SetBool(WeaponDrawn, false);
    }

    public override void OnStateExit() 
    {
        Controls.ToggleEquipWeapon(true);
    }

    public override void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Controls.SetState(new ArmedState(Controls, WeaponController));
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            WeaponController.EquipWeapon(WeaponController.MainHandWeapon, false);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            WeaponController.EquipWeapon(WeaponController.BackWeapon, false);
        }
    }
}
