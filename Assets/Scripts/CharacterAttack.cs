using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    [SerializeField] private GameObject weaponRotatorGO;
    [SerializeField] private int attackPower = 20;
    [SerializeField] private Transform weaponPos;
    [SerializeField] private bool inCooldown = false;
    [SerializeField] private float cooldownTime = 0.2f;
    [SerializeField] private Animator weaponAnim;

    public bool IsAttacking { get; private set; }
    private bool wasAttacking;

    private void Update()
    {
        if (Input.GetButtonDown("Attack") && !IsAttacking && !inCooldown)
        {
            RotateWeapon();
            weaponAnim.SetBool("Attack", true);
        }
        else if (!IsAttacking)
        {
            weaponAnim.SetBool("Attack", false);
        }

        if (IsAttacking)
        {
            CheckForHit();
        }

        if (wasAttacking && !IsAttacking)
        {
            StartCoroutine(CoolDownCoroutine());
        }

        wasAttacking = IsAttacking;
    }

    /// <summary>
    /// This coroutine handles the cooldown period after an attack.
    /// It sets inCooldown to true, waits for the cooldown time to elapse, then sets inCooldown to false.
    /// </summary>
    private IEnumerator CoolDownCoroutine()
    {
        inCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        inCooldown = false;
    }

    /// <summary>
    /// This will rotate the weapon to face the mouse cursor position.
    /// It calculates the direction from the character to the mouse position and rotates the weapon accordingly.
    /// </summary>
    private void RotateWeapon()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        weaponRotatorGO.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    /// <summary>
    /// This checks for any enemies hit during the attack and applies damage to them.
    /// It uses a circular overlap check to find colliders within range and damages any enemies found.
    /// </summary>
    private void CheckForHit()
    {
        Collider2D[] hittenObjects = Physics2D.OverlapCircleAll(weaponPos.position, 8f);

        foreach (Collider2D hittenObject in hittenObjects)
        {
            EnemyHealth enemyHealth = hittenObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.Damage(attackPower, transform.position, 100);
            }
        }
    }

    public void SetIsAttacking(bool isAttacking)
    {
        IsAttacking = isAttacking;
    }
}