using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioManager : MonoBehaviour
{
    [Header("Audios")]
    [SerializeField] AudioSource swordSwoosh;
    [SerializeField] AudioSource enemyDamage;
    [SerializeField] AudioSource enemySteps;

    public void PlaySwordSwooshSound()
    {
        swordSwoosh.pitch = Random.Range(.8f, 1.2f);
        swordSwoosh.Play();
    }
    public void PlayEnemyDamageSound()
    {
        enemyDamage.pitch = Random.Range(.8f,1.2f);
        enemyDamage.Play();
    }
    public void PlayEnemyStepsSound()
    {
        enemySteps.pitch = Random.Range(.8f, 1.2f);
        enemySteps.Play();
    }
}
