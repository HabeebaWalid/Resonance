using UnityEngine;

public class DirectionalLightRotator : MonoBehaviour
{
    [Tooltip ("The Speed At Which The Light Rotates")]
    public float rotationSpeed = 5.0f; 

    [Tooltip ("The Axis Around Which The Light Will Rotate")]
    public Vector3 rotationAxis = Vector3.right; 

    void Update()
    {
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }
}