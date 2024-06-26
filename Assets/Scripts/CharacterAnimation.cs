using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] Animator anim;
    CharacterMovement mov;

    private void Start()
    {
        mov = GetComponent<CharacterMovement>();
    }
    private void Update()
    {
        anim.SetBool("isWalking", mov.GetIsMoving());
    }
}
