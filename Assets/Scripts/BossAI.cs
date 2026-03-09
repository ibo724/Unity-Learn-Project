using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BossAI : MonoBehaviour
{
    [Header("Hareket")]
    public float chaseSpeed = 3f;

    [Header("Şarj")]
    public float chargeSpeed = 14f;
    public float chargeDuration = 0.4f;
    public float chargeCooldown = 4f;

    [Header("Saldırı")]
    public float attackRange = 1.5f;
    public int meleeDamage = 2;
    public int chargeDamage = 3;
    public float attackCooldown = 1.2f;

    private Rigidbody2D rb;
    private Transform player;
    private BossHealth bossHealth;
    private float attackTimer;
    private float chargeTimer;
    private bool isCharging;
    private bool isPhase2;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bossHealth = GetComponent<BossHealth>();
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;
        chargeTimer = chargeCooldown;
    }

    void Update()
    {
        if (player == null || isCharging) return;

        // Faz 2 kontrolü
        if (!isPhase2 && bossHealth.currentHealth <= bossHealth.maxHealth * 0.5f)
        {
            isPhase2 = true;
            chaseSpeed *= 1.5f;
            chargeCooldown *= 0.55f;
            Debug.Log("Boss Faz 2!");
        }

        attackTimer -= Time.deltaTime;
        chargeTimer -= Time.deltaTime;

        // Şarj saldırısı
        if (chargeTimer <= 0f)
        {
            StartCoroutine(Charge());
            return;
        }

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= attackRange)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            if (attackTimer <= 0f)
            {
                attackTimer = attackCooldown;
                player.GetComponent<PlayerHealth>()?.TakeDamage(meleeDamage);
            }
        }
        else
        {
            float dir = player.position.x > transform.position.x ? 1f : -1f;
            rb.linearVelocity = new Vector2(dir * chaseSpeed, rb.linearVelocity.y);
        }

        if (rb.linearVelocity.x > 0.1f) transform.localScale = new Vector3(1, 1, 1);
        else if (rb.linearVelocity.x < -0.1f) transform.localScale = new Vector3(-1, 1, 1);
    }

    IEnumerator Charge()
    {
        isCharging = true;
        chargeTimer = chargeCooldown;
        float dir = player.position.x > transform.position.x ? 1f : -1f;
        rb.linearVelocity = new Vector2(dir * chargeSpeed, rb.linearVelocity.y);

        yield return new WaitForSeconds(chargeDuration);

        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        isCharging = false;
    }

    // Şarj sırasında çarpınca hasar
    void OnCollisionEnter2D(Collision2D col)
    {
        if (isCharging && col.gameObject.CompareTag("Player"))
            col.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(chargeDamage);
    }
}