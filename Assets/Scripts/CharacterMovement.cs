using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    GameObject characterSprite;
    [SerializeField] Animator CharacterAnim;
    PlayerInputs playerInputs;
    Rigidbody2D rb;
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool isMoving;
    private Vector2 input;
    float lastInputY;

    private void Start()
    {
        SpriteRenderer characterSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        characterSprite = characterSpriteRenderer.GameObject();
        playerInputs = GetComponent<PlayerInputs>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        input = playerInputs.GetPlayerInput();
        Flip();

        CharacterAnim.SetBool("isWalking", isMoving);

        if (lastInputY < 0) CharacterAnim.SetFloat("Y", -1);
        else if (lastInputY > 0) CharacterAnim.SetFloat("Y", 1);
        else if (lastInputY == 0 && input.x != 0) CharacterAnim.SetFloat("Y", -1);

        lastInputY = input.y;
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(input.x) > 0.001f || Mathf.Abs(input.y) > 0.001f) isMoving = true;
        else isMoving = false;

        input = input.normalized; // Turn into the direction
        rb.velocity = input * movementSpeed;
    }

    /// <summary>
    /// This flips the character sprite based on the input direction.
    /// It rotates the character sprite to face the direction of movement.
    /// </summary>
    private void Flip()
    {
        if (input.x > 0.01f)
        {
            characterSprite.transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }
        else if (input.x < -0.01f)
        {
            characterSprite.transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        }
    }

    public bool GetIsMoving()
    {
        return isMoving;
    }
}
