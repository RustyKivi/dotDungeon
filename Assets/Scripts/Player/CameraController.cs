using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public float minXPosition = 0f;
    private bool follow = false;

    public void camInit(bool _init)
    {
        if(_init == true){
            follow = true;
        }else{
            follow = false;
            transform.position = new Vector3(0,0,-10);
        }
    }

    void LateUpdate()
    {
        if (target.position.x < minXPosition || follow == false)
            return;

        Vector3 desiredPosition = new Vector3(target.position.x,0,-10);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
