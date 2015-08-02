using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovingObject : MonoBehaviour
{
    public enum MoveOption { ConstantVelocity, MoveToLocation, Arrived }

    public Rigidbody myRigidBody;

    public MoveOption moveOption;
    public Vector3 constantVelocity;
    public Vector3 moveToLocation;
    public float moveToLocationVelocity;
    public float threshhold = 0.5f;
    public bool setMovement = true;

    public void Start()
    {
        // Initialize movement
        myRigidBody = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (setMovement)
            SetMovement();

        if (moveOption == MoveOption.MoveToLocation)
        {
            Vector3 distToTarget = moveToLocation - transform.position;
            if (distToTarget.sqrMagnitude < threshhold * threshhold)
            {
                // Stop
                myRigidBody.velocity = Vector3.zero;
                moveOption = MoveOption.Arrived;
            }
        }
    }

    private void SetMovement()
    {
        if (moveOption == MoveOption.ConstantVelocity)
        {
            myRigidBody.velocity = constantVelocity;
            myRigidBody.useGravity = false;
        }
        else if (moveOption == MoveOption.MoveToLocation)
        {
            Vector3 delta = moveToLocation - transform.position;
            constantVelocity = Vector3.ClampMagnitude(delta, Math.Min(delta.magnitude, moveToLocationVelocity));
            myRigidBody.velocity = constantVelocity;
            myRigidBody.useGravity = false;
        }
        setMovement = false;
    }

    public static void StopAllMovers()
    {
        MovingObject[] allMovers = GameObject.FindObjectsOfType<MovingObject>();
        foreach (var eachMover in allMovers)
        {
            eachMover.myRigidBody.velocity = Vector3.zero;
            eachMover.myRigidBody.angularVelocity = Vector3.zero;
            eachMover.myRigidBody.useGravity = false;
        }
    }
}
