using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    PlayerInputs playerInputs;
    Rigidbody2D rb;
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool isMoving;
    private Vector2 input;

    private void Awake()
    {
        playerInputs = GetComponent<PlayerInputs>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        input = playerInputs.GetPlayerInput();
        Flip();
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(input.x) > 0.001f || Mathf.Abs(input.y) > 0.001f) isMoving = true;
        else isMoving = false;
        input = input.normalized;
        rb.velocity = input * movementSpeed;
    }
    
    private void Flip()
    {
        if (input.x > 0.01f)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }
        else if (input.x < -0.01f)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        }
    }

    public bool GetIsMoving()
    {
        return isMoving;
    }
}
