using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected readonly PlayerControls Controls;
    protected readonly PlayerWeaponController WeaponController;

    public abstract void Tick();

    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }

    public PlayerState(PlayerControls controls, PlayerWeaponController weaponController)
    {
        Controls = controls;
        WeaponController = weaponController;
    }
}
