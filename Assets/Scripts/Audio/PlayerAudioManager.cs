using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [Header("Audios")]
    [SerializeField] AudioSource swordSwoosh;
    [SerializeField] AudioSource playerDamage;
    [SerializeField] AudioSource playerSteps;

    public void PlaySwordSwooshSound()
    {
        swordSwoosh.pitch = Random.Range(.8f, 1.2f);
        swordSwoosh.Play();
    }
    public void PlayPlayerDamageSound()
    {
        playerDamage.pitch = Random.Range(1f,1.4f);
        playerDamage.Play();
    }
    public void PlayPlayerStepsSound()
    {
        playerSteps.pitch = Random.Range(0.8f, 1.2f);
        playerSteps.Play();
    }
}
