using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] ParticleSystem shootingParticles;
    [SerializeField] AudioClip gunShotSFX;
    [SerializeField] DifficultyGenerator difficultyGenerator;

    AudioSource audioSource;
    PlayerHealth target;
    Animator animator;


    string currentSceneName;
    string difficultyLevel;
    int difficultyDamage;

    void Start()
    {
        target = FindObjectOfType<PlayerHealth>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        shootingParticles = GetComponentInChildren<ParticleSystem>();
        currentSceneName = SceneManager.GetActiveScene().name;

        foreach (DifficultyGenerator.Levels levels in difficultyGenerator.myLevels)
        {
            if (currentSceneName == levels.levelName)
            {
                difficultyLevel = levels.levelName;
                difficultyDamage = levels.enemyDamage;
            }
        }
    }

    public void AttackHitEvent()
    {
        if (target == null)
            return;



        if (currentSceneName == difficultyLevel)
        {
            target.TakeDamage(difficultyDamage);
            shootingParticles.Play();
            PlayGunShotEffect();
        }



    }

    void PlayGunShotEffect()
    {
        if (gunShotSFX != null)
        {
            AudioSource.PlayClipAtPoint(gunShotSFX, transform.position);
        }
    }
}
