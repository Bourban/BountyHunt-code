using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    WeaponDataSO WeaponData;

    [SerializeField]
    private Transform MuzzlePosition;
    [SerializeField] 
    public Transform OffHandPosition;
    
    public bool IsReloading = false;

    bool ShouldCancelReload = false;

    int AmmoCount;
    float LastShotTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(WeaponData == null) 
        {
            Debug.LogErrorFormat("No Weapon data on {0}. Set one in the inspector.", gameObject.name);
        }

        AmmoCount = WeaponData.Capacity;
    }

    public void Shoot(Vector3 target)
    {
        if (AmmoCount > 0)
        {
            if (Time.time - LastShotTime >= WeaponData.FireRate)
            {
                Vector3 shotDir = target - MuzzlePosition.position;

                var ray = Physics.Raycast(MuzzlePosition.position, shotDir.normalized, out RaycastHit hit);
                Debug.DrawLine(MuzzlePosition.position, hit.point, Color.red, 5.0f);

                var box = hit.collider.GetComponent<HitBox>();
                if(box)
                {
                    Debug.LogFormat("Ow my {0}", box.gameObject.name);
                    box.OnRaycastHit(shotDir.normalized, box.RbComp, WeaponData.ImpactForce);
                }

                --AmmoCount;
                LastShotTime = Time.time;
            }
        }
    }

    //Allow reloads to be interrupted, bullets will be loaded one by one.
    public void StartReload() 
    {
        //If we're already reloading we don't want to start the coroutine again.
        if (IsReloading == false)
        {
            StartCoroutine(ReloadBullet());
        }

        IsReloading = true;
        ShouldCancelReload = false;
    }

    public void CancelReload() 
    {
        ShouldCancelReload = true;
    }

    IEnumerator ReloadBullet() 
    {
        //Animation to be handled in the character controller.
        yield return new WaitForSeconds(WeaponData.BulletReloadTime);

        if (AmmoCount < WeaponData.Capacity)
        {
            ++AmmoCount;
            Debug.Log(AmmoCount);
        }
        else 
        {
            ShouldCancelReload = true;
        }
       
        if(ShouldCancelReload == false) 
        {
            yield return ReloadBullet();
        }
        else 
        {
            IsReloading = false;
        }
    }

    public Transform GetMuzzleTransform()
    {
        return MuzzlePosition;
    }
    
    public WeaponType GetWeaponType() 
    {
        return WeaponData.WeaponEquipType;
    }
}
