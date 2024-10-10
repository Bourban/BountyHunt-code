using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollHandler : MonoBehaviour
{
    private Rigidbody[] RigidBodies;
    private Animator CharAnim;
    
    // Start is called before the first frame update
    private void Start()
    {
        SetupComponents();
        SetRagdollActive(false);
    }

    public void SetRagdollActive(bool isActive)
    {
        foreach (Rigidbody rb in RigidBodies)
        {
            rb.isKinematic = !isActive;
        }
        CharAnim.enabled = !isActive;
    }

    private void SetupComponents()
    {
        RigidBodies = GetComponentsInChildren<Rigidbody>();
        CharAnim = GetComponent<Animator>();
    }
}
