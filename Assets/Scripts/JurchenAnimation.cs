using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class JurchenAnimation : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    AIController aIController;
    [SerializeField] SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        aIController = GetComponent<AIController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (aIController.currentState == AIController.AiState.idle)
        {
            anim.SetBool("isRunning", false);
            anim.SetFloat("Y", -1);
        }
        else
        {
            if (aIController.target.position.x < transform.position.x)
            {
                sprite.flipX = true;
            }
            else sprite.flipX = false;

            anim.SetBool("isRunning", true);
            anim.SetFloat("Y", rb.velocity.normalized.y);
        }


    }
}
