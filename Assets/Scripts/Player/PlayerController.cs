using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(NetworkObject))]
public class PlayerController : NetworkBehaviour
{
    public PlayerState playerState = PlayerState.CanInput;

    [Header("Value's")]
    public float speedGround = 12f;
    public float speedAir = 12f;
    public float jumpHeight = 3f;
    [Space]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.1f;
    [SerializeField] private LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;
    private CharacterController controller;

    public override void OnNetworkSpawn()
    {
        InitializePlayer();
    }
    

    private void Update()
    {
        if(!IsOwner)return;
        if (playerState == PlayerState.CanInput)
        {
            HandleMovement();
            HandleJump();
        }

        GroundCheckFunction();
    }

    private void InitializePlayer()
    {
        if(!IsOwner){
            GameObject camObj = GetComponentInChildren<Camera>().gameObject;
            Destroy(camObj);
            return;
        }
        controller = GetComponent<CharacterController>();
        //this.tag = "ActivePlayer";
    }

    private void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        if (isGrounded)
        {
            controller.SimpleMove(move * speedGround);
        }
    }

    private void HandleJump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            float jumpVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
            velocity.y = jumpVelocity;

            Vector3 forwardForce = transform.forward * speedAir * 2f;
            controller.SimpleMove(forwardForce);
        }
        else if (!isGrounded)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speedAir * Time.deltaTime);
        }
    }

    private void GroundCheckFunction()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    private void OnDrawGizmos()
    {
        if(groundCheck == false)return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
    }
}

public enum PlayerState
{
    CanInput,
    CanNotInput,
    None
}
