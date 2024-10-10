using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [HideInInspector]
    public HitDetector Detector;
    [HideInInspector]
    public Rigidbody RbComp;
    
    private void Start()
    {
        RbComp = GetComponent<Rigidbody>();
    }

    public void OnRaycastHit(Vector3 direction, Rigidbody rb, float impactForce)
    {
        Detector.OnHit(direction, rb, impactForce);
    }
}
