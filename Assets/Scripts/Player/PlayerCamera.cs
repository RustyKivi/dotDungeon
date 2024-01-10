using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensitivity = 2f;

    private float rotationX = 0f;
    private bool hiddenCursor = true;
    public bool forceLock = false;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -90f, 90f);

            transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
            transform.parent.Rotate(Vector3.up * mouseX);
        }

        HandleCursorLock();
    }

    void HandleCursorLock()
    {
        if(forceLock == true)return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            hiddenCursor = !hiddenCursor;
            if(hiddenCursor == true)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }else{
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
