using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float staminaMax = 100f;
    public float sprintCost = 10f;
    public float staminaRegenRate = 20f;

    private float currentStamina;
    private bool isSprinting = false;
    private CharacterController controller;
    private Vector3 moveDirection;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        currentStamina = staminaMax;
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 move = transform.right * horizontalInput + transform.forward * verticalInput;
        moveDirection = move.normalized;

        if (Input.GetKey(KeyCode.LeftShift) && currentStamina >= sprintCost)
        {
            isSprinting = true;
            currentStamina -= sprintCost * Time.deltaTime;
        }
        else
        {
            isSprinting = false;
        }

        float speed = isSprinting ? sprintSpeed : moveSpeed;
        controller.Move(moveDirection * speed * Time.deltaTime);

        if (!isSprinting && currentStamina < staminaMax)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, staminaMax);
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 150, 20), "Stamina: " + Mathf.Round(currentStamina));
    }
}
