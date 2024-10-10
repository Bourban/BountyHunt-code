using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BountyHunt/Weapons/Weapon Data SO")]
public class WeaponDataSO : ScriptableObject
{
    [SerializeField]
    public string WeaponName; 

    [SerializeField]
    public WeaponType WeaponEquipType;
    [SerializeField]
    public AmmoType AmmoUsed;

    //When fired, how many projectiles are released. Some shells like slugs will fire multiple.
    [SerializeField]
    public int ProjectilesPerShot;
    //Amount, in degrees, which projectiles can deviate from the target.
    [SerializeField]
    public float ProjectileSpread;
    //Knockback Force
    [SerializeField] 
    public float ImpactForce;

    //Minimum delay between shots.
    [SerializeField]
    public float FireRate;

    //How long it takes to load each bullet.
    [SerializeField]
    public float BulletReloadTime;
    //Max capacity of the magazine/cylinder/etc.
    [SerializeField]
    public int Capacity;

    //TODO: VFX and Sound Clip(s)
}
