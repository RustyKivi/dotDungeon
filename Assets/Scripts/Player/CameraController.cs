using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Transform target;
    public float smoothTime = 0.3f;
    public float minXPosition = 0f;
    private bool follow = false;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void camInit(bool _init)
    {
        follow = _init;
        if (!follow)
        {
            transform.position = new Vector3(0, 0, -10);
        }
    }

    void LateUpdate()
    {
        if (!follow || target == null || target.position.x < minXPosition)
            return;

        Vector3 targetPosition = new Vector3(target.position.x, 0, -10);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
