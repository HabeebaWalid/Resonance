using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 sideOffset = new Vector3 (3f, 2f, 0f);

    public float smoothTime = 0.3f;
    public float rotationSpeed = 2f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 smoothedTargetPosition;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 rawTargetPosition = target.position;

        Vector3 desiredPosition = rawTargetPosition 
                                + target.right * sideOffset.x 
                                + Vector3.up * sideOffset.y 
                                + target.forward * sideOffset.z;

        transform.position = Vector3.SmoothDamp (transform.position, desiredPosition, ref velocity, smoothTime);

        Vector3 lookTarget = rawTargetPosition + Vector3.up * 1.5f;
        Quaternion desiredRotation = Quaternion.LookRotation (lookTarget - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp (transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
    }
}
