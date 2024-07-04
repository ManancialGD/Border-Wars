using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWithSon : MonoBehaviour
{
    [SerializeField] SpriteRenderer son;
    private bool hasSon;

    // Start is called before the first frame update
    void Start()
    {
        hasSon = false;
        son.enabled = false;
    }

    public void SetHasSon(bool b)
    {
        hasSon = b;
        son.enabled = b;
    }

    public bool GetHasSon()
    {
        return hasSon;
    }
}
