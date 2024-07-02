using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSwordVisually : MonoBehaviour
{
    [SerializeField] SpriteRenderer swordSprite;
    [SerializeField] GameObject weaponParent;
    [SerializeField] GameObject swordGO;

    CharacterAttack attack;

    private void Start()
    {
        attack = GetComponentInParent<CharacterAttack>();
    }

    private void FixedUpdate()
    {
        if (!attack.IsAttacking)
        {
            RotateSword();
        }
    }

    void RotateSword()
    {
        // Get the mouse position in world coordinates
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the direction vector from the sword to the mouse
        Vector3 direction = mousePos - transform.position;

        // Calculate the angle in degrees from the direction vector
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (angle > 90 || angle < -90)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Euler(0, 180, transform.rotation.eulerAngles.z);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        // Adjust the sorting order based on the angle
        if (angle > 0)
        {
            swordSprite.sortingOrder = 0;
        }
        else
        {
            swordSprite.sortingOrder = 1;
        }

    }
}
