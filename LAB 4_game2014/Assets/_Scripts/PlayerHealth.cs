using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    public float invulnerabilityTime = 1f;
    public float knockbackForce = 7f;

    [Header("UI")]
    public Slider healthSlider;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public event Action<int, int> OnHealthChanged;

    private int currentHealth;
    private bool isInvulnerable = false;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private PlayerRespawn playerRespawn;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerRespawn = GetComponent<PlayerRespawn>();
        UpdateHealthUI();
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage, Vector2 hitSourcePosition)
    {
        if (isInvulnerable) return;

        currentHealth -= damage; //damage for player health

        // Trigger Cinemachine Perlin camera shake
        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.Shake();
        }

        // Apply knockback away from the enemy
        if (rb != null)
        {
            float knockDir = transform.position.x >= hitSourcePosition.x ? 1f : -1f;
            rb.velocity = new Vector2(knockDir * knockbackForce, knockbackForce * 0.6f);
        }

        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;
            if (playerRespawn != null)
            {
                playerRespawn.Respawn();
            }
        }
        else
        {
            StartCoroutine(InvulnerabilityRoutine());
        }

        UpdateHealthUI();
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;

        if (spriteRenderer != null)
        {
            for (int i = 0; i < 5; i++)
            {
                spriteRenderer.enabled = false;
                yield return new WaitForSeconds(invulnerabilityTime / 10f);
                spriteRenderer.enabled = true;
                yield return new WaitForSeconds(invulnerabilityTime / 10f);
            }
        }
        else
        {
            yield return new WaitForSeconds(invulnerabilityTime);
        }

        isInvulnerable = false;
    }
}
