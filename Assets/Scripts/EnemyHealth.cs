using System.Collections;
using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    Animator anim;
    EnemyAudioManager audioManager;
    private Rigidbody2D rb;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int hp;
    [SerializeField] public bool IsStunned { get; private set; }
    public int HP
    {
        get { return hp; }
        private set
        {
            hp = Mathf.Clamp(value, 0, maxHealth);
            if (hp <= 0) Die();
        }
    }
    public event Action<GameObject> OnEnemyDied;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hp = maxHealth;
        audioManager = GetComponentInChildren<EnemyAudioManager>();
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// This applies damage to the enemy and initiates a knockback effect.
    /// It reduces the enemy's health by the specified amount and applies a knockback force based on the attack position.
    /// It also starts a coroutine to make the enemy invulnerable for a short period.
    /// </summary>
    public void Damage(int damageAmount, Vector3 attackPos, int knockbackAmount = 0)
    {
        if (IsStunned) return;

        HP -= damageAmount;
        audioManager.PlayEnemyDamageSound();
        anim.SetBool("Damage", true);
        ApplyKnockback(attackPos, knockbackAmount);

        StartCoroutine(InvulnerabilityCoroutine(.5f));
    }

    /// <summary>
    /// This applies a knockback effect to the enemy based on the attack position and knockback amount.
    /// It calculates the direction away from the attack position and applies a force to the enemy's rigidbody.
    /// </summary>
    private void ApplyKnockback(Vector3 attackPos, int knockbackAmount)
    {
        if (rb != null)
        {
            Vector3 knockbackVector = (transform.position - attackPos).normalized;
            rb.velocity += (Vector2)knockbackVector * knockbackAmount;
        }
    }

    /// <summary>
    /// This coroutine makes the enemy invulnerable for a specified duration.
    /// It sets IsStunned to true, waits for the invulnerability time to elapse, then sets IsStunned to false.
    /// </summary>
    private IEnumerator InvulnerabilityCoroutine(float invulnerabilityTime)
    {
        IsStunned = true;
        yield return new WaitForSeconds(invulnerabilityTime);
        IsStunned = false;
        anim.SetBool("Damage", false);
    }

    private void Die()
    {
        OnEnemyDied?.Invoke(gameObject);
        Destroy(gameObject);
    }
}
