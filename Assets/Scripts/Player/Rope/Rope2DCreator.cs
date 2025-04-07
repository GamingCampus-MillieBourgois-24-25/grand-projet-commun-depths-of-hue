using System;
using UnityEngine;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class Rope2DCreator : MonoBehaviour
{
    [SerializeField, Range(2, 50)] private int segmentsCount = 2;

    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [SerializeField] private HingeJoint2D hingePrefab;
    [SerializeField] private LineRenderer line;
    [SerializeField] private Rigidbody2D rbPlayer;

    [HideInInspector] public Transform[] segments;
    [HideInInspector] public HingeJoint2D[] joints;

    [SerializeField] private float motorSpeedChangeInterval = 2f;
    private float timeSinceLastChange = 0f;
    private bool wasMotorSpeedNegative = false;

    private Vector2 GetSegmentPosition(int segmentIndex)
    {
        Vector2 posA = pointA.position;
        Vector2 posB = pointB.position;
    
        if (segmentIndex == 0)
        {
            return posA;
        }
    
        if (segmentIndex == segmentsCount - 1)
        {
            return posB;
        }

        float fraction = 1f / (float)(segmentsCount - 1);
        return Vector2.Lerp(posA, posB, fraction * segmentIndex);
    }

    [Button]
    private void GenerateRope()
    {
        Vector3 transformPosition = this.transform.position;
        transformPosition.z = -1f;
        this.transform.position = transformPosition;
        
        segments = new Transform[segmentsCount];
        joints = new HingeJoint2D[segmentsCount];

        for (int i = 0; i < segmentsCount; i++)
        {
            var currJoint = Instantiate(hingePrefab, GetSegmentPosition(i), Quaternion.identity, this.transform);
            segments[i] = currJoint.transform;
            
            Vector3 currJointPosition = segments[i].position;
            currJointPosition.z = -1f;
            segments[i].position = currJointPosition;
            
            joints[i] = currJoint.GetComponent<HingeJoint2D>();
            
            CheckSpeed joint = currJoint.GetComponent<CheckSpeed>();
            joint.motorSpeed = wasMotorSpeedNegative;
            wasMotorSpeedNegative = !wasMotorSpeedNegative;

            if (i > 0)
            {
                int prevIndex = i - 1;
                currJoint.connectedBody = segments[prevIndex].GetComponent<Rigidbody2D>();
            }
            if (i == segmentsCount - 1) currJoint.connectedBody = rbPlayer;
            
            if (i == segmentsCount - 1 || i == 0) continue;
            
            currJoint.useMotor = true;
            var motor = currJoint.motor;
            motor.maxMotorTorque = 1000f;
            motor.motorSpeed = 0.1f;
            currJoint.motor = motor;

        }
    }
    
    [Button]
    private void DeleteRope()
    {
        foreach (var t in segments)
        {
            Destroy(t.gameObject);
        }
        segments = Array.Empty<Transform>();
        joints = Array.Empty<HingeJoint2D>();
        line.positionCount = 0;
    }
    
    private void Update()
    {
        timeSinceLastChange += Time.deltaTime;
    
        if (timeSinceLastChange >= motorSpeedChangeInterval)
        {
            timeSinceLastChange = 0f;

            if (segmentsCount <= 2 || segments.Length <= 0) return;

            for (int i = 1; i < segmentsCount - 1; i++)
            {
                if (!joints[i]) continue;
            
                var motor = joints[i].motor;
                CheckSpeed joint = joints[i].GetComponent<CheckSpeed>();

                motor.motorSpeed = joint.motorSpeed ? Random.Range(-3.5f, -0.1f) : Random.Range(0.1f, 3.5f);
                joint.motorSpeed = !joint.motorSpeed;

                motor.maxMotorTorque = Random.Range(5000f, 10000f);
                joints[i].motor = motor;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (pointA == null || pointB == null) return;
        Gizmos.color = Color.green;
        for (int i = 0; i < segmentsCount; i++)
        {
            Vector2 posAtIndex = GetSegmentPosition(i);
            Gizmos.DrawSphere(posAtIndex, 0.1f);
        }
    }
}
