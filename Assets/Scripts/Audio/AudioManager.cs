using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audios")]
    [SerializeField] AudioSource swordSwoosh;
    [SerializeField] AudioSource playerDamage;
    [SerializeField] AudioSource playerSteps;

    [SerializeField] AudioSource enemyDamage;

    private static AudioManager instance;

    void Awake()
    {
        InitializeSingleton();
    }

    // Initialize the singleton instance
    private void InitializeSingleton()
    {
        // If an instance already exists and it's not this one, destroy this object
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set the instance to this object and mark it to not be destroyed
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySwordSwooshSound()
    {
        swordSwoosh.pitch = Random.Range(.8f, 1.2f);
        swordSwoosh.Play();
    }
    public void PlayPlayerDamageSound()
    {
        playerDamage.pitch = Random.Range(.8f,1.2f);
        playerDamage.Play();
    }
    public void PlayPlayerStepsSound()
    {
        playerSteps.pitch = Random.Range(0.8f, 1.2f);
        playerSteps.Play();
    }
    public void PlayEnemyDamageSound()
    {
        enemyDamage.pitch = Random.Range(.8f,1.2f);
        enemyDamage.Play();
    }
}
