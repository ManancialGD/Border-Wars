using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestScene : MonoBehaviour
{
    SceneManagement sceneManagement;

    private void Start()
    {
        sceneManagement = FindObjectOfType<SceneManagement>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        CharacterWithSon characterWithSon = other.GetComponentInChildren<CharacterWithSon>();

        if (characterWithSon != null)
        {
            if (characterWithSon.GetHasSon())
            {
                sceneManagement.ChangeScene("Win");
            }
            else Debug.Log("Go take the son.");
        }
    }
}
