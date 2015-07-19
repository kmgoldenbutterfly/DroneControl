using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class ChaseCamera : MonoBehaviour
{
    public enum Relative { Fixed, Rotating };

    public Relative relative = Relative.Rotating;
    public Transform lookAtObject;
    public Vector3 lookatOffset;
    public Vector3 cameraOffset;

    public void InitFromDroneGui()
    {
        // If target is not pre-defined, find the drone and track it
        if (lookAtObject == null)
            lookAtObject = FindObjectOfType<DroneController>().GetComponent<DroneController>().transform;
        cameraOffset = transform.position - lookAtObject.position;
    }

    void Update()
    {
        float yaw = lookAtObject.rotation.eulerAngles.y; // The only angle we care about in terms of how the target is rotated
        Vector3 temp = cameraOffset;
        if (relative == Relative.Rotating)
            temp = Quaternion.Euler(0, yaw, 0) * cameraOffset;

        // Keep myself at a fixed relative position from the target
        transform.position = lookAtObject.position + temp;
        // Look at my target
        transform.LookAt(lookAtObject.position + lookatOffset);
    }
}
