using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public float invincibilityDuration = 1f;
    public Slider healthSlider;

    private int currentHealth;
    private float invTimer;
    private bool isInvincible;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    void Update()
    {
        if (transform.position.y < -20f)
            Die();

        if (isInvincible)
        {
            invTimer -= Time.deltaTime;
            if (invTimer <= 0f) isInvincible = false;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        isInvincible = true;
        invTimer = invincibilityDuration;

        if (healthSlider != null) healthSlider.value = currentHealth;

        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        currentHealth = maxHealth;
        if (healthSlider != null) healthSlider.value = currentHealth;

        if (GameManager.Instance != null)
            transform.position = GameManager.Instance.respawnPoint;
        else
            transform.position = Vector3.zero;
    }
}