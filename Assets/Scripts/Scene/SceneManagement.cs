using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    CameraFollow cameraFollow;

    string currentScene;

    [SerializeField] Transform[] jailSpawnPoints;
    [SerializeField] GameObject jailPrefab;

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

        currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Forest")
        {
            int randomIndex = Random.Range(0, jailSpawnPoints.Length);
            Vector3 randomPlace = jailSpawnPoints[randomIndex].position;
            
            Instantiate(jailPrefab, randomPlace, Quaternion.identity);
        }
    }
}
