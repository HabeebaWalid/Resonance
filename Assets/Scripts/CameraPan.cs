using UnityEngine;

public class CameraPan : MonoBehaviour
{
    [Header ("Pan Settings")]
    public Transform startPoint;   
    public Transform endPoint;     
    public float panSpeed = 2f;

    private bool isPanning = false;
    private float t = 0f; 

    void Update()
    {
        if (isPanning && startPoint != null && endPoint != null)
        {
            t += Time.deltaTime * panSpeed;

            transform.position = Vector3.Lerp (startPoint.position, endPoint.position, t);

            if (t >= 1f)
            {
                isPanning = false;
            }
        }
    }

    public void StartPan()
    {
        if (startPoint != null && endPoint != null)
        {
            transform.position = startPoint.position;
            t = 0f;
            isPanning = true;
        }
    }

    public void StopPan()
    {
        isPanning = false;
    }
}
