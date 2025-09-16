using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    [Tooltip ("Adjust Value To Control Rotation Speed")]
    public float rotationSpeed = 1.0f;

    [Tooltip ("Initial Skybox Rotation (0 - 360 Degrees)")]
    public float initialRotation = 0.0f;

    private float currentRotation;

    void Start()
    {
        Material skyboxMaterial = RenderSettings.skybox;

        if (skyboxMaterial != null && skyboxMaterial.HasProperty ("_Rotation"))
        {
            currentRotation = initialRotation % 360f;
            skyboxMaterial.SetFloat ("_Rotation", currentRotation);
        }
        else
        {
            Debug.LogWarning ("Skybox Material Missing Or Doesn't Have '_Rotation'.");
        }
    }

    void Update()
    {
        Material skyboxMaterial = RenderSettings.skybox;

        if (skyboxMaterial != null && skyboxMaterial.HasProperty ("_Rotation"))
        {
            currentRotation += rotationSpeed * Time.deltaTime;

            currentRotation = currentRotation % 360f;
            if (currentRotation > 360f)
            {
                currentRotation -= 360f;
            }
            
            skyboxMaterial.SetFloat ("_Rotation", currentRotation);
        }
    }
}
