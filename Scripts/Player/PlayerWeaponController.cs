using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class PlayerWeaponController : MonoBehaviour
{
    //TEMP - Serialised
    [SerializeField]
    public Weapon MainHandWeapon = null;
    [SerializeField]
    private Weapon OffhandWeapon = null;
    [SerializeField]
    public Weapon BackWeapon = null;
    
    [SerializeField] 
    public Transform LeftHandIKTarget;
    
    //Bones for Weapon attachments
    private Transform HolsterRightTransform;
    private Transform HolsterLeftTransform;
    private Transform HolsterBack;
    private Transform HandRight;
    private Transform HandLeft;

    public Dictionary<AmmoType, int> AmmoCounts = new Dictionary<AmmoType, int>();

    private Animator CharAnimator;
    private Rig IkRig;
    private WeaponIK CharIk;
    
    private PlayerEquipType EquipType;
    private Weapon CurrentWeapon;

    private static readonly int EquippedWeaponType = Animator.StringToHash("EquippedWeaponType");

    private void Start()
    {
        SetupComponents();
        
        EquipType = PlayerEquipType.Unarmed;
        CurrentWeapon = MainHandWeapon;

        var ammoTypes = (AmmoType[])Enum.GetValues(typeof(AmmoType));

        foreach(var ammoType in ammoTypes) 
        {
            AmmoCounts.TryAdd(ammoType, 0);
        }

        EquipType = GetCorrectEquipType();
        CharIk.SetAimTransform(GetEquippedWeaponMuzzleTransform());
    }

    private void Update()
    {
        if (CurrentWeapon.OffHandPosition != null)
        {
            LeftHandIKTarget.position = CurrentWeapon.OffHandPosition.position;
            LeftHandIKTarget.rotation = CurrentWeapon.OffHandPosition.rotation;
        }
    }

    private PlayerEquipType GetCorrectEquipType()
    {
        if(CurrentWeapon != null) 
        {
            if (CurrentWeapon.GetWeaponType() == WeaponType.OneHand) 
            {
                /*if(OffhandWeapon != null) 
                {
                    return PlayerEquipType.DualWield;
                }*/
                //TODO: Weapon choice and dual wielding
                return PlayerEquipType.OneHanded;
            }
            else if(CurrentWeapon.GetWeaponType() == WeaponType.TwoHand) 
            {
                return PlayerEquipType.TwoHanded;
            } 
        }
        return PlayerEquipType.Unarmed;
    }

    public void EquipWeapon(Weapon newWeapon, bool shouldEquip = false)
    {
        ToggleEquipWeapon(false);
        CurrentWeapon = newWeapon;
        
        ToggleEquipWeapon(shouldEquip);

        EquipType = GetCorrectEquipType();
        CharAnimator.SetInteger(EquippedWeaponType, (int)EquipType);
        CharIk.SetAimTransform(GetEquippedWeaponMuzzleTransform());
    }
    
    public void Shoot(Vector3 target) 
    {
        if (CurrentWeapon.IsReloading)
        {
            CurrentWeapon.CancelReload();
            //Play "click" sound or whatever.
            return;
        }
        
        if (EquipType == PlayerEquipType.Unarmed || CurrentWeapon == null) 
        {
            return;
        }
        
        //TODO: Alternate, not just only shoot the offhand gun
        if (EquipType == PlayerEquipType.DualWield)
        {
            OffhandWeapon.Shoot(target);
        }
        
        CurrentWeapon.Shoot(target);
    }

    public void Reload() 
    {
        if(CurrentWeapon != null) 
        {
            CurrentWeapon.StartReload();
        }

        if(EquipType == PlayerEquipType.DualWield)
        {
            if (OffhandWeapon != null)
            {
                OffhandWeapon.StartReload();
            }
        }
    }

    public int GetAmmoCountForType(AmmoType type) 
    {
        return AmmoCounts[type];
    }

    public PlayerEquipType GetWeaponEquipType() 
    {
        return EquipType;
    }

  
    private Transform GetHolsterForWeapon()
    {
        switch (CurrentWeapon.GetWeaponType()) //eww eww eww
        {
            case WeaponType.OneHand:
                return HolsterRightTransform;
            case WeaponType.TwoHand:
                return HolsterBack;
        }
        return null;
    }
    
    public void ToggleEquipWeapon(bool equipped)
    {
        CurrentWeapon.transform.SetParent(equipped ? HandRight : GetHolsterForWeapon(), false);
    }
    
    private void SetupComponents()
    {
        CharAnimator = GetComponentInChildren<Animator>();
        IkRig = GetComponentInChildren<Rig>();
        CharIk = GetComponent<WeaponIK>();
        
        //Weapon transforms -- Really not crazy about using GameObject.Find for this, but it's marginally better than clogging up the editor with serialised fields.
        //TODO - Overhaul this setup -- Maybe search through all child objects to at least limit the scope
        HolsterRightTransform = GameObject.Find("HolsterRight").transform;
        HolsterLeftTransform = GameObject.Find("HolsterLeft").transform;
        HolsterBack = GameObject.Find("BackAttachTransform").transform;
        HandRight = GameObject.Find("GunTransformHandR").transform;
    }

    public Transform GetEquippedWeaponMuzzleTransform()
    {
        return CurrentWeapon.GetMuzzleTransform();
    }
}

//Handles character logic for different weapon types.
public enum PlayerEquipType 
{

    OneHanded,
    TwoHanded,
    DualWield,
    Unarmed,
    Item
}

public enum WeaponType 
{
    OneHand,
    TwoHand,
    Thrown,
    Item
}

public enum AmmoType 
{
    ThirtyEight,
    FortyFour, 
    Buckshot,
    Slugs,
}
