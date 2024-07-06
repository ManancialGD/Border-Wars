using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleArea : MonoBehaviour
{
    private bool hasPlayerEnter;
    [SerializeField] private List<GameObject> enemies; // Use List instead of array
    [SerializeField] private GameObject grid;

    private void Start()
    {
        hasPlayerEnter = false;
        grid.SetActive(false);

        // Register to the enemy death event
        foreach (GameObject enemy in enemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.OnEnemyDied += EnemyDied;
            }
        }
    }

    private void Update()
    {
        if (!hasPlayerEnter) return;

        // Check if all enemies are destroyed
        if (enemies.Count == 0)
        {
            grid.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CharacterMovement characterMovement = other.GetComponent<CharacterMovement>();
        if (characterMovement != null)
        {
            hasPlayerEnter = true;
            grid.SetActive(true);
        }
    }

    private void EnemyDied(GameObject enemy)
    {
        if (!hasPlayerEnter) return;
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }
}
