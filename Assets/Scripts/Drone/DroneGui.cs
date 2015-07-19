using UnityEngine;
using System.Collections;

public class DroneGui : MonoBehaviour
{
    public Camera[] allCameras;
    public Rect[] cameraPos;
    public Camera startCamera;

    void Awake()
    {
        // We disable cameras before their Awake or Start functions can be called, so we must manually init ChaseCameras before we disable them.
        var chaseCameras = GameObject.FindObjectsOfType<ChaseCamera>();
        for (int i = 0; i < chaseCameras.Length; i++)
            chaseCameras[i].InitFromDroneGui();

        // Grab all the cameras
        allCameras = GameObject.FindObjectsOfType<Camera>();
        cameraPos = new Rect[allCameras.Length];
        if (startCamera == null)
            startCamera = allCameras[0];

        for (int i = 0; i < allCameras.Length; i++)
        {
            allCameras[i].gameObject.SetActive(allCameras[i] == startCamera);
            cameraPos[i] = new Rect(10, 10 + 30 * i, 200, 25);
        }
    }

    void OnGUI()
    {
        for (int i = 0; i < allCameras.Length; i++)
        {
            if (GUI.Button(cameraPos[i], allCameras[i].gameObject.name))
                for (int j = 0; j < allCameras.Length; j++)
                    allCameras[j].gameObject.SetActive(i == j); // Enable only this camera when you click its button
        }
    }
}
