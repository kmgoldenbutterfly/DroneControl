using UnityEngine;
using System.Collections;

public class TargetTracker : MonoBehaviour
{
    public Transform bestTarget = null;
    public EffectVolume[] allTargets;
    public float bestDistSq;
    public bool isCrashed = false;

    void Start()
    {
        allTargets = GameObject.FindObjectsOfType<EffectVolume>();
    }

    void Update()
    {
        if (isCrashed)
            return;

        bestTarget = null;
        bestDistSq = -1;

        float eachDistSq;
        foreach (EffectVolume eachTarget in allTargets)
        {
            if (eachTarget.effectType != EffectVolume.EffectType.Victory)
                continue;

            eachDistSq = (transform.position - eachTarget.transform.position).sqrMagnitude;
            if (bestTarget == null || eachDistSq < bestDistSq)
            {
                bestTarget = eachTarget.transform;
                bestDistSq = eachDistSq;
            }
        }

        if (bestTarget != null)
            transform.LookAt(bestTarget.transform);
    }
}
