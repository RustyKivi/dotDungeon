using System.Collections;
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

    [Header("Weapon")]
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public GameObject swordObject;
    public float swordCooldown = 0.5f;
    public float arrowForce = 500f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int currentItem = 0;
    private int remainingJumps;
    private bool canUseSword = true;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        CameraController.instance.target = this.gameObject.transform;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        remainingJumps = extraJumps;
        if (!IsOwner) return;
        CameraController.instance.target = this.gameObject.transform;
        GameManager.instance.myObject = this.gameObject;
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        isGrounded = Physics2D.OverlapCircle(transform.position, 0.2f, LayerMask.GetMask("Ground"));

        float horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Mouse0) && canUseSword)
        {
            StartCoroutine(SwordAttack());
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            FireArrow();
        }

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

    private IEnumerator SwordAttack()
    {
        canUseSword = false;
        yield return new WaitForSeconds(swordCooldown);
        canUseSword = true;
    }

    private void FireArrow()
    {
        if (arrowPrefab && arrowSpawnPoint)
        {
            GameObject arrowInstance = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
            Rigidbody2D arrowRb = arrowInstance.GetComponent<Rigidbody2D>();
            if (arrowRb)
            {
                arrowRb.AddForce(arrowSpawnPoint.right * arrowForce);
            }
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
        // Implement item usage logic for clients
    }

    public void Damage(int dmg)
    {
        if (Health > 0)
        {
            Health -= dmg;
            if (Health <= 0)
            {
                Health = 0;
            }
        }
    }

    public void Heal(int h)
    {
        if (Health <= 0)
        {
            Debug.Log("Current NetworkPlayer is already dead");
            return;
        }

        Health += h;
        if (Health > 100) Health = 100;
    }
}
