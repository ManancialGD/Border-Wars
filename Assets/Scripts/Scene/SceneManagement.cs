using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneManagement : MonoBehaviour
{
    CameraFollow cameraFollow;

    /// <summary>
    /// This will initialize the CameraFollow component and set the target for the camera to follow.
    /// It finds the CameraFollow component attached to the main camera and sets the character position
    /// as the target for the camera to follow.
    /// </summary>
    void Start()
    {
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            Transform characterPos = FindObjectOfType<CharacterMovement>().GetComponent<Transform>();
            cameraFollow.SetTarget(characterPos);
        }
    }
}
