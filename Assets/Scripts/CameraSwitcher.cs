using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [System.Serializable]
    public class NamedCamera
    {
        public string name;
        public Camera camera;
    }

    public List <NamedCamera> namedCameras = new List <NamedCamera>();

    private Dictionary <string, Camera> cameraDict = new Dictionary<string, Camera>();

    void Start()
    {
        foreach (var entry in namedCameras)
        {
            if (entry.camera != null && !string.IsNullOrEmpty(entry.name))
            {
                cameraDict[entry.name] = entry.camera;
            }
        }

        // Activate The First Camera By Default
        if (namedCameras.Count > 0)
        {
            SwitchToCamera (namedCameras[0].name);
        }
    }

    public void SwitchToCamera (string cameraName)
    {
        foreach (var cam in cameraDict)
        {
            cam.Value.enabled = cam.Key == cameraName;
        }
    }
}
