using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JailScript : MonoBehaviour
{
    [SerializeField] SpriteRenderer son;
    CharacterWithSon characterWithSon;
    private bool jailHasBool;

    // Start is called before the first frame update
    void Start()
    {
        characterWithSon = FindObjectOfType<CharacterWithSon>();
        jailHasBool = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Something entered");
        CharacterMovement characterMovement = other.GetComponent<CharacterMovement>();

        if (characterMovement != null)
        {
            son.enabled = false;
            characterWithSon.SetHasSon(true);
        }
    }
}
