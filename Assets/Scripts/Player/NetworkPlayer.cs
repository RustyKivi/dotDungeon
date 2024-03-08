using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkPlayer : NetworkBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int extraJumps = 1;
    
    private Rigidbody2D rb;
    private bool isGrounded;
    private int remainingJumps;

    public override void OnNetworkSpawn()
    {
        if(IsOwner == false){
            return;
        }else{
            CameraController.instance.target = this.gameObject.transform;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        remainingJumps = extraJumps;
        if(IsOwner == false)return;
        CameraController.instance.target = this.gameObject.transform;
        GameManager.instance.myObject = this.gameObject;
    }

    private void Update()
    {
        if(IsOwner == false)return;
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.2f, LayerMask.GetMask("Ground"));

        float horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        if (isGrounded)
        {
            remainingJumps = extraJumps;
        }

        if (Input.GetButtonDown("Jump") && (isGrounded || remainingJumps > 0))
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        remainingJumps--;
    }
}
