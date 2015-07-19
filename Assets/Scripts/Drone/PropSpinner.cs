using System;
using System.Collections;
using UnityEngine;

public class PropSpinner : MonoBehaviour
{
    public bool spinning = true;
    public float RPS = 10.0f;

    void Update()
    {
        if (spinning)
        {
            DateTime now = DateTime.UtcNow;
            Vector3 euler = new Vector3(0, (now.Millisecond * 360.0f * RPS / 1000.0f), 0);
            transform.localRotation = Quaternion.Euler(euler);
        }
    }
}
