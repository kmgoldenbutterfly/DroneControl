using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class EffectVolume : MonoBehaviour
{
    public enum EffectType
    {
        None,
        Collision,
        Repair,
        Victory,
    }

    public EffectType effectType = EffectType.None;

    public void OnTriggerEnter(Collider col)
    {
        DroneController drone = col.GetComponent<DroneController>();
        if (drone == null)
            return; // Not our player

        drone.SetActiveEffect(effectType);
        if (effectType == EffectType.Victory)
            drone.OnVictory();
    }

    void OnTriggerExit(Collider col)
    {
        var drone = col.GetComponent<DroneController>();
        if (drone == null)
            return; // Not our player

        drone.SetActiveEffect(EffectType.None);
    }
}
