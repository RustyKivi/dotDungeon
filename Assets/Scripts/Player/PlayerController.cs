using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float airControlMultiplier = 0.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    [Space]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    [Space]
    public Transform raycastOrigin;
    public float raycastDistance = 5f;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private bool isGrounded;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)){Attack();}
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        inputDirection = transform.TransformDirection(inputDirection);
        
        if (isGrounded && moveDirection.y < 0)
        {
            moveDirection.y = -2f;
        }
        
        if (isGrounded)
        {
            moveDirection = inputDirection * walkingSpeed;
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }
        else
        {
            moveDirection = new Vector3(inputDirection.x * walkingSpeed * airControlMultiplier, moveDirection.y, inputDirection.z * walkingSpeed * airControlMultiplier);
        }
        
        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }

    public void Attack()
    {
        Ray ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            
            Debug.Log(hit.collider.name);
            NonePlayer nonPlayerScript = hit.collider.GetComponent<NonePlayer>();
            if (nonPlayerScript != null)
            {
                nonPlayerScript.Damage(10);
            }
        }
    }
}
