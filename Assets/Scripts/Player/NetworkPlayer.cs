using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkPlayer : NetworkBehaviour
{
    [Header("My Value's")]
    public int Health = 100;
    [Header("Character Value's")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int extraJumps = 1;
    [Header("Inventory")]
    public PlayerItem item1;
    public PlayerItem item2;
    
    private Rigidbody2D rb;
    private bool isGrounded;
    private int currentItem = 0;
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

        if(Input.GetKeyDown(KeyCode.Alpha0)){EquiptItemServerRPC(0);}
        if(Input.GetKeyDown(KeyCode.Alpha1)){EquiptItemServerRPC(1);}
        if(Input.GetKeyDown(KeyCode.Alpha2)){EquiptItemServerRPC(2);}
        if(Input.GetKeyDown(KeyCode.Mouse0)){UseItemServerRPC();}

        float horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        if (horizontalInput != 0f)
        {
            Vector3 currentScale = transform.localScale;
            currentScale.x = Mathf.Sign(horizontalInput) * Mathf.Abs(currentScale.x);
            transform.localScale = currentScale;
        }

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

    [ServerRpc(RequireOwnership = false)]
    public void EquiptItemServerRPC(int item)
    {
        EquiptItemClientRPC(item);
    }

    [ClientRpc]
    private void EquiptItemClientRPC(int item)
    {
        if(item == 0)
        {
            item1.equiptFunction(false);
            item2.equiptFunction(false);
            currentItem = 0;
        }
        else if(item == 1)
        {
            item1.equiptFunction(true);
            item2.equiptFunction(false);
            currentItem = 1;
        }else if(item == 2)
        {
            item1.equiptFunction(false);
            item2.equiptFunction(true);
            currentItem = 2;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void UseItemServerRPC()
    {
        UseItemClientRPC();
    }
    [ClientRpc]
    private void UseItemClientRPC()
    {
        if(currentItem == 0)return;

        if(currentItem == 1)
        {
            item1.Trigger();
        }else if (currentItem == 2)
        {
            item2.Trigger();
        }
    }

    public void Damage(int dmg)
    {
        if(Health >= 0)
        {
            Health -= dmg;
            if(Health <= 0)
            {
                Health = 0;
            }
        }
    }
    public void Heal(int h)
    {
        if(Health >= 0)
        {
            Debug.Log("Current NetworkPlayer is already dead");
            return;
        }else{
            Health += h;
            if(Health >= 100){Health = 100;}
        }
    }
}
