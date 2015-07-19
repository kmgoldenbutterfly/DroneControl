using UnityEngine;
using System.Collections;

public class VictoryEffect : MonoBehaviour
{
    public const int flashFrames = 4;

    public AudioSource cameraSnap;
    public Light lightFlash;
    private int frameCount = 0;
    private bool victoryActive = false;

    void Start()
    {
        cameraSnap = GetComponentsInChildren<AudioSource>(true)[0];
        lightFlash = GetComponentsInChildren<Light>(true)[0];
    }

    public void OnVictory()
    {
        frameCount = 0;
        victoryActive = true;
        cameraSnap.gameObject.SetActive(true);
        lightFlash.gameObject.SetActive(true);
    }

    void Update()
    {
        if (victoryActive)
        {
            frameCount++;
            if (frameCount > flashFrames)
            {
                lightFlash.gameObject.SetActive(false);
            }
        }
    }
}
