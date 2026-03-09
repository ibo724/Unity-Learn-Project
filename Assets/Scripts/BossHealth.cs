using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public float maxHealth = 20f;
    public float currentHealth; // public — BossAI erişmesi için
    public Slider healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (healthBar) healthBar.value = currentHealth;
        if (currentHealth <= 0f) Die();
    }

    void Die()
    {
        Debug.Log("Boss yenildi!");
        Destroy(gameObject);
        // İleride buraya: victory ekranı veya yeni sahne yükle
    }
}