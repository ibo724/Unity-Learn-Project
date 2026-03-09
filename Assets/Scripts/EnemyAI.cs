using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [Header("Patrol")]
    public float patrolSpeed = 2f;
    public float patrolDistance = 3f;

    [Header("Takip")]
    public float chaseSpeed = 4f;
    public float detectionRange = 6f;

    [Header("Saldırı")]
    public float attackRange = 1f;
    public int attackDamage = 1;
    public float attackCooldown = 1f;

    private Rigidbody2D rb;
    private Transform player;
    private Vector2 startPos;
    private int patrolDir = 1;
    private float attackTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;
    }

    void Update()
    {
        if (player == null) return;
        attackTimer -= Time.deltaTime;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= attackRange)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            if (attackTimer <= 0f)
            {
                attackTimer = attackCooldown;
                player.GetComponent<PlayerHealth>()?.TakeDamage(attackDamage);
            }
        }
        else if (dist <= detectionRange)
        {
            float dir = player.position.x > transform.position.x ? 1f : -1f;
            rb.linearVelocity = new Vector2(dir * chaseSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(patrolDir * patrolSpeed, rb.linearVelocity.y);
            if (Mathf.Abs(transform.position.x - startPos.x) >= patrolDistance)
                patrolDir *= -1;
        }

        // Yön döndürme
        if (rb.linearVelocity.x > 0.1f) transform.localScale = new Vector3(1, 1, 1);
        else if (rb.linearVelocity.x < -0.1f) transform.localScale = new Vector3(-1, 1, 1);
    }
}