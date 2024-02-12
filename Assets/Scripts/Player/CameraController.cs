using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Transform camRoot;
    public float distance = 5.0f;
    public float height = 2.0f;
    public float sensitivityX = 5.0f;
    public float sensitivityY = 2.0f;
    public LayerMask obstacleMask;

    private Vector3 offset;
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    private void Start()
    {
        offset = new Vector3(0, height, distance);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        player.Rotate(Vector3.up * Input.GetAxis("Mouse X") * sensitivityX);

        rotationY -= Input.GetAxis("Mouse Y") * sensitivityY;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f);
        camRoot.localRotation = Quaternion.Euler(rotationY, 0, 0);

        Quaternion camRotation = Quaternion.Euler(rotationY, player.eulerAngles.y, 0);
        Vector3 desiredPosition = player.position - (camRotation * offset);

        RaycastHit hit;
        if (Physics.Linecast(player.position, desiredPosition, out hit, obstacleMask))
        {
            transform.position = hit.point;
        }
        else
        {
            transform.position = desiredPosition;
        }

        transform.LookAt(camRoot);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(player.position - (camRoot.rotation * offset), 0.1f);
        Gizmos.DrawLine(player.position, player.position - (camRoot.rotation * offset));
    }
}
