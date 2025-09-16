using UnityEngine;

public class CameraPanBackward : MonoBehaviour
{
    public float panSpeed = 2f;         
    private bool isPanning = false;

    void Start()
    {

    }

    void Update()
    {
        if (isPanning)
        {
            transform.position -= transform.forward * panSpeed * Time.deltaTime;
        }
    }

    public void StartPan()
    {
        isPanning = true;
    }

    public void StopPan()
    {
        isPanning = false;
    }
}
