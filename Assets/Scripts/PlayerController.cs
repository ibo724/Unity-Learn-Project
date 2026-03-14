using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Hareket")]
    public float moveSpeed = 8f;

    [Header("Zıplama")]
    public float jumpForce = 18f;
    public int maxJumps = 2;

    [Header("Dash")]
    public float dashSpeed = 22f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1f;

    [Header("Saldırı")]
    public Transform attackPoint;
    public float attackRange = 1.2f;
    public float attackDamage = 1f;
    public float attackCooldown = 0.35f;
    public LayerMask enemyLayer;

    [Header("Zemin Kontrolü")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private float horizontalInput;
    private int jumpsLeft;
    private bool isDashing;
    private float dashCooldownTimer;
    private float attackTimer;
    private bool facingRight = true;
    private float originalGravity;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;
    }

    void Update()
    {
        dashCooldownTimer -= Time.deltaTime;
        attackTimer -= Time.deltaTime;

        if (isDashing) return;

        bool isGrounded = Physics2D.OverlapCircle(
            groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded) jumpsLeft = maxJumps;

        if (GameManager.Instance != null)
            maxJumps = GameManager.Instance.doubleJumpUnlocked ? 2 : 1;

        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput > 0 && !facingRight) Flip();
        else if (horizontalInput < 0 && facingRight) Flip();

        if (Input.GetButtonDown("Jump") && jumpsLeft > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpsLeft--;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0f
            && (GameManager.Instance == null || GameManager.Instance.dashUnlocked))
            StartCoroutine(Dash());

        if (Input.GetMouseButtonDown(0) && attackTimer <= 0f)
            Attack();
    }

    void FixedUpdate()
    {
        if (!isDashing)
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    IEnumerator Dash()
    {
        isDashing = true;
        dashCooldownTimer = dashCooldown;
        rb.gravityScale = 0f;
        float dir = facingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(dir * dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;
    }

    void Attack()
    {
        attackTimer = attackCooldown;
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position, attackRange, enemyLayer);
        foreach (var hit in hits)
        {
            hit.GetComponent<EnemyHealth>()?.TakeDamage(attackDamage);
            hit.GetComponent<BossHealth>()?.TakeDamage(attackDamage);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 s = transform.localScale;
        s.x *= -1f;
        transform.localScale = s;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck)
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        if (attackPoint)
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}