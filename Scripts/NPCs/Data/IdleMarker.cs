using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleMarker : MonoBehaviour
{
    [SerializeField]
    public CharacterIdentity IdentityMask;

    [SerializeField]
    public Transform LookTarget;

    [SerializeField]
    public string ClipTrigger;

    [SerializeField]
    public GameObject PropPrefab;

    [SerializeField]
    public Transform PropPlacement;

    [SerializeField, Tooltip("Optional end marker if a carry/work/etc. idle")]
    public IdleMarker LinkedMarker;

    [SerializeField, Tooltip("Should ever be used as a starting point, untick for 'dropoff' markers, etc.")]
    public bool IsEverUsable = true;

   // [HideInInspector]
    public bool IsAvailable = true;

    public float AnimDuration;

    private void Start()
    {
        SandboxManager.Instance.RegisterIdleMarker(this);
    }

    public void SetAvailability(bool value) 
    {
        IsAvailable = value;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);

        Gizmos.DrawWireSphere(LookTarget.position, 0.1f);
    }

}
