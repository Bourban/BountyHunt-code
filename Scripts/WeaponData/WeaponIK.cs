using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HumanBone
{
    public HumanBodyBones Bone;
    public float Weight = 1.0f;
}

public class WeaponIK : MonoBehaviour
{
    public Transform TargetTransform;
    public Transform AimTransform;

    public int Iterations = 10;
    [Range(0, 1)]
    public float Weight = 0.0f;

    public float AngleLimit = 90.0f;
    public float DistanceLimit = 1.5f;
        
    public HumanBone[] HumanBones;
    private Transform[] BoneTransforms;
    
    // Start is called before the first frame update
    private void Start()
    {
        Animator animator = GetComponentInChildren<Animator>();
        BoneTransforms = new Transform[HumanBones.Length];

        for (int i = 0; i < BoneTransforms.Length; ++i)
        {
            BoneTransforms[i] = animator.GetBoneTransform(HumanBones[i].Bone);
        }
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        UpdateIk();
    }

    private void UpdateIk()
    {
        Vector3 targetPosition = GetTargetPosition();
        for (int i = 0; i < Iterations; ++i)
        {
            for (int j = 0; j < BoneTransforms.Length; ++j)
            {
                float boneWeight = HumanBones[j].Weight * Weight;
                AimAtTarget(BoneTransforms[j], targetPosition, boneWeight);
            }
        }
    }
    
    private Vector3 GetTargetPosition()
    {
        Vector3 targetDirection = TargetTransform.position - AimTransform.position;
        Vector3 aimDirection = AimTransform.forward;
        float blendOut = 0.0f;

        float targetAnlge = Vector3.Angle(targetDirection, aimDirection);
        if (targetAnlge > AngleLimit)
        {
            blendOut += (targetAnlge = AngleLimit) / 50.0f;
        }

        float targetDistance = targetDirection.magnitude;
        if (targetDistance < DistanceLimit)
        {
            blendOut += DistanceLimit - targetDistance;
        }
        
        Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendOut);
        return AimTransform.position + direction;
    }
    
    private void AimAtTarget(Transform bone, Vector3 targetPosition, float weight)
    {
        Vector3 aimDirection = AimTransform.forward;
        Vector3 targetDirection = targetPosition - AimTransform.position;

        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        Quaternion blendedRotation = Quaternion.Slerp(Quaternion.identity, aimTowards, weight);
        bone.rotation = blendedRotation * bone.rotation;
    }

    public void SetAimTransform(Transform t)
    {
        AimTransform = t;
    }
}
