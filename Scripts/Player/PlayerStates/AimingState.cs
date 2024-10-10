using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingState : PlayerState
{
    private static readonly int IsAiming = Animator.StringToHash("IsAiming");

    public AimingState(PlayerControls controls, PlayerWeaponController weaponController) : base(controls, weaponController)
    {
    }

    public override void OnStateEnter()
    {
        Controls.CharAnimator.SetBool(IsAiming, true);
        
        Controls.Crosshair.SetActive(true);
        
        Controls.SetIkWeight(1);
        Controls.UpperLayerTargetWeight = 1;
    }

    public override void OnStateExit()
    {
        Controls.CharAnimator.SetBool(IsAiming, false);
        
        Controls.Crosshair.SetActive(false);
        
        Controls.UpperLayerTargetWeight = 0;
        Controls.SetIkWeight(0);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public override void Tick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        GraphicsRaycast.Raycast(ray.origin, ray.direction, out RaycastHit hit, 1000);

        Controls.CurrentCursorPosition = hit.point;

        if (Input.GetMouseButtonDown(0))
        {
            Controls.Shoot();
        }

        if (Input.GetMouseButtonUp(1))
        {
            Controls.SetState(new ArmedState(Controls, WeaponController));
        }
    }
}
