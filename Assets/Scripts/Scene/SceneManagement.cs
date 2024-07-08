using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    [SerializeField] private GameObject text;
    private CameraFollow cameraFollow;
    private string currentScene;
    [SerializeField] private Transform[] jailSpawnPoints;
    [SerializeField] private GameObject jailPrefab;

    /// <summary>
    /// Initializes the CameraFollow component and sets the target for the camera to follow.
    /// It finds the CameraFollow component attached to the main camera and sets the character position
    /// as the target for the camera to follow.
    /// </summary>
    void Start()
    {
        InitializeCameraFollow();
        currentScene = SceneManager.GetActiveScene().name;
        HandleSceneSpecificLogic();
    }

    /// <summary>
    /// Finds the CameraFollow component and sets the character position as the target for the camera to follow.
    /// </summary>
    private void InitializeCameraFollow()
    {
        cameraFollow = Camera.main?.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            Transform characterPos = FindObjectOfType<CharacterMovement>()?.GetComponent<Transform>();
            if (characterPos != null)
            {
                cameraFollow.SetTarget(characterPos);
            }
            else
            {
                Debug.LogWarning("CharacterMovement component not found.");
            }
        }
        else
        {
            Debug.LogWarning("CameraFollow component not found on main camera.");
        }
    }

    /// <summary>
    /// Handles logic specific to the current scene.
    /// </summary>
    private void HandleSceneSpecificLogic()
    {
        if (currentScene == "Forest")
        {
            SpawnRandomJail();
        }
        else if (currentScene == "OpeningScene")
        {
            StartCoroutine(ChangeSceneAfterTime("Forest", 14f));
        }
        else if (currentScene == "Lusofona")
        {
            StartCoroutine(ChangeSceneAfterTime("MainMenu", 4f));
        }
    }

    /// <summary>
    /// Spawns the jail prefab at a random spawn point.
    /// </summary>
    private void SpawnRandomJail()
    {
        if (jailSpawnPoints != null && jailSpawnPoints.Length > 0 && jailPrefab != null)
        {
            int randomIndex = Random.Range(0, jailSpawnPoints.Length);
            Vector3 randomPlace = jailSpawnPoints[randomIndex].position;
            Instantiate(jailPrefab, randomPlace, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Jail spawn points or jail prefab not set properly.");
        }
    }

    /// <summary>
    /// Changes the scene after a specified time.
    /// </summary>
    /// <param name="sceneName">The name of the scene to change to.</param>
    /// <param name="time">The time to wait before changing the scene.</param>
    /// <returns></returns>
    private IEnumerator ChangeSceneAfterTime(string sceneName, float time)
    {
        yield return new WaitForSeconds(time);
        ChangeScene(sceneName);
    }

    /// <summary>
    /// Changes the current scene to the specified scene.
    /// </summary>
    /// <param name="sceneName">The name of the scene to change to.</param>
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
