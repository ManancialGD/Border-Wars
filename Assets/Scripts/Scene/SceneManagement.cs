using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    [SerializeField] GameObject text;
    CameraFollow cameraFollow;
    string currentScene;
    bool start;
    [SerializeField] Transform[] jailSpawnPoints;
    [SerializeField] GameObject jailPrefab;

    /// <summary>
    /// This will initialize the CameraFollow component and set the target for the camera to follow.
    /// It finds the CameraFollow component attached to the main camera and sets the character position
    /// as the target for the camera to follow.
    /// </summary>
    void Start()
    {
        start = false;
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
            Time.timeScale = 0;
            text.SetActive(true);
            start = true;
        }
        else if (currentScene == "OpeningScene")
        {
            StartCoroutine(ChangeSceneAfterCutScene());
        }
    }

    private void Update()
    {
        if (!start) return;
        if (Input.GetButtonDown("Attack"))
        {
            Time.timeScale = 1;
            text.SetActive(false);
        }
    }

    IEnumerator ChangeSceneAfterCutScene()
    {
        yield return new WaitForSeconds(8);
        ChangeScene("Forest");
    }
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
