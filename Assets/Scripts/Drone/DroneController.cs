using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovingObject))]
[RequireComponent(typeof(Collider))]
public class DroneController : MonoBehaviour
{
    private System.Random rng = new System.Random();
    private Dictionary<EffectVolume.EffectType, List<ParticleSystem>> effectsByType = new Dictionary<EffectVolume.EffectType, List<ParticleSystem>>();

    public MovingObject myMovingObject;
    public TargetTracker trackingCam;
    public Transform visual;
    public PropSpinner[] allProps;
    public VictoryEffect victoryEffect;
    public bool doVictoryWaggle = false;
    public int curHitPoints = 100;
    public int maxHitPoints = 100;
    public float collisionEffectDuration = 3.5f;
    public float dmgMultiplier = 10.0f;
    public float deathFlipMultiplier = 20.0f;

    private DateTime lastCollision = DateTime.MinValue;
    private DateTime lastUpdate = DateTime.MinValue;

    public void Start()
    {
        // Grab references to various sub-components
        myMovingObject = GetComponent<MovingObject>();
        visual = transform.FindChild("VisualAndColliders");
        trackingCam = GetComponentsInChildren<TargetTracker>(true)[0];
        victoryEffect = GetComponentsInChildren<VictoryEffect>(true)[0];
        allProps = GetComponentsInChildren<PropSpinner>();
        var allParticles = GetComponentsInChildren<ParticleSystem>(true);
        List<ParticleSystem> eachList;
        EffectVolume.EffectType eachEffectType;
        foreach (var eachParticle in allParticles)
        {
            if (TryParseEnum(eachParticle.gameObject.name, out eachEffectType))
            {
                if (!effectsByType.TryGetValue(eachEffectType, out eachList))
                    effectsByType[eachEffectType] = eachList = new List<ParticleSystem>();
                eachList.Add(eachParticle);
            }
        }
    }

    public void Update()
    {
        DateTime now = DateTime.UtcNow;

        // Victory Waggle
        if (doVictoryWaggle)
        {
            double t = now.Millisecond / 1000.0;
            double x = 20.0 * Math.Cos(2.0 * Math.PI * t);
            double z = 20.0 * Math.Sin(2.0 * Math.PI * t);
            Vector3 eulerWaggle = new Vector3((float)x, 0, (float)z);
            visual.localRotation = Quaternion.Euler(eulerWaggle);
        }
        else
        {
            visual.localRotation = Quaternion.identity;
        }

        // Disable Collision Effect
        var collisionEndTime = lastCollision + TimeSpan.FromSeconds(collisionEffectDuration);
        if (lastUpdate <= collisionEndTime && collisionEndTime < now)
            SetActiveEffect(EffectVolume.EffectType.None);
        lastUpdate = now;
    }

    void OnCollisionEnter(Collision col)
    {
        int prevHitPoints = curHitPoints;
        if (curHitPoints > 0)
        {
            int dmg = (int)Math.Ceiling(col.relativeVelocity.magnitude * dmgMultiplier);
            curHitPoints -= dmg;
            Debug.Log(gameObject.name + " took " + dmg + " damage from a collision!  HP: " + curHitPoints + "/" + maxHitPoints);
            lastCollision = DateTime.UtcNow;
            SetActiveEffect(EffectVolume.EffectType.Collision);
        }

        if (prevHitPoints > 0 && curHitPoints <= 0)
        {
            Debug.Log("Drone Death");
            // Crash the drone
            myMovingObject.myRigidBody.useGravity = true;
            Vector3 deathSpin = new Vector3((float)rng.NextDouble() * deathFlipMultiplier, (float)rng.NextDouble() * deathFlipMultiplier, (float)rng.NextDouble() * deathFlipMultiplier);
            myMovingObject.myRigidBody.angularVelocity = myMovingObject.myRigidBody.angularVelocity + deathSpin;
            myMovingObject.myRigidBody.angularDrag = 0.35f;

            // Stop the propellers
            foreach (var eachProp in allProps)
                eachProp.spinning = false;

            // Lock up the tracking-cam, so it looks like it's crashing
            trackingCam.isCrashed = true;
            lastCollision = DateTime.UtcNow + TimeSpan.FromHours(24);
            SetActiveEffect(EffectVolume.EffectType.Collision);
        }
    }

    public void SetActiveEffect(EffectVolume.EffectType effectType)
    {
        foreach (var pair in effectsByType)
            foreach (var effect in pair.Value)
                effect.gameObject.SetActive(pair.Key == effectType);
    }

    public void OnVictory()
    {
        Debug.Log("Drone Victory: (" + curHitPoints + "/" + maxHitPoints + ")");
        if (curHitPoints > 0) // Doesn't count if you crash into it
        {
            MovingObject.StopAllMovers();
            doVictoryWaggle = true;
            victoryEffect.OnVictory();
        }
    }

    // Utility Section
    public static bool TryParseEnum<ET>(string str, out ET value)
    {
        try
        {
            value = (ET)Enum.Parse(typeof(ET), str, true);
        }
        catch (Exception)
        {
            value = default(ET);
            return false;
        }

        return true;
    }
}
