using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHP : MonoBehaviour
{
    SceneManagement sceneManagement;
    [SerializeField] Animator characterAnim;
    private Rigidbody2D rb;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int hp;
    public int HP
    {
        get { return hp; }
        private set
        {
            hp = Mathf.Clamp(value, 0, maxHealth);
            if (hp <= 0) Die();
        }
    }

    [SerializeField] private bool isInvulnerable = false;

    private void Start()
    {
        sceneManagement = FindObjectOfType<SceneManagement>();  
        rb = GetComponent<Rigidbody2D>();
        hp = maxHealth;
    }

    /// <summary>
    /// This applies damage to the character and initiates a knockback effect.
    /// It reduces the character's health by the specified amount and applies a knockback force based on the attack position.
    /// It also starts a coroutine to make the character invulnerable for a short period.
    /// </summary>
    public void Damage(int damageAmount, Vector3 attackPos, int knockbackAmount = 0)
    {
        if (isInvulnerable) return;

        HP -= damageAmount;
        
        ApplyKnockback(attackPos, knockbackAmount);
        if (!isInvulnerable) StartCoroutine(InvulnerabilityCoroutine(.4f));
        StartCoroutine(DamageAnimator(.2f));
    }

    /// <summary>
    /// This applies a knockback effect to the character based on the attack position and knockback amount.
    /// It calculates the direction away from the attack position and applies a force to the character's rigidbody.
    /// </summary>
    private void ApplyKnockback(Vector3 attackPos, int knockbackAmount)
    {
        if (rb != null)
        {
            Vector3 knockbackVector = (transform.position - attackPos).normalized;
            rb.velocity = (Vector2)knockbackVector * knockbackAmount;
        }
    }

    /// <summary>
    /// This coroutine makes the character invulnerable for a specified duration.
    /// It sets isInvulnerable to true, waits for the invulnerability time to elapse, then sets isInvulnerable to false.
    /// </summary>
    private IEnumerator InvulnerabilityCoroutine(float invulnerabilityTime)
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTime);
        isInvulnerable = false;
    }
    private IEnumerator DamageAnimator(float changeTime)
    {
        characterAnim.SetBool("Damage", true);
        yield return new WaitForSeconds(changeTime);
        characterAnim.SetBool("Damage", false);
    }

    private void Die()
    {
        sceneManagement.ChangeScene("GameOver");
        Destroy(gameObject);
    }

    public bool GetIsInvulnerable()
    {
        return isInvulnerable;
    }
}
