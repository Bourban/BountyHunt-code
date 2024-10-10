using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HitDetector : MonoBehaviour
{
    private RagdollHandler RdHandler;
    private NPC NPCScript;

    private bool IsNPC = false;
    
    // Start is called before the first frame update
    void Start()
    {
        RdHandler = GetComponent<RagdollHandler>();
        NPCScript = GetComponent<NPC>();

        if (NPCScript != null)
        {
            IsNPC = true;
        }
        
        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidBodies)
        {
            HitBox box = rb.gameObject.AddComponent<HitBox>();
            box.Detector = this;
        }
    }

    public void OnHit(Vector3 direction, Rigidbody rb, float impactForce)
    {
        RdHandler.SetRagdollActive(true);
        rb.AddForce(direction * impactForce, ForceMode.VelocityChange);

        if (IsNPC)
        {
            NPCScript.Die();
        }
    }
   
}
