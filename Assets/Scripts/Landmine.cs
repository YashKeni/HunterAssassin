using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : MonoBehaviour
{
    [SerializeField] GameObject landmineModel;
    [SerializeField] GameObject explosionParticles;
    [SerializeField] float explosionDamage = 100f;
    [SerializeField] float damageRadius;

    bool activated;
    bool exploded;
    bool inRange;
    float distanceToTarget;

    PlayerHealth player;
    AudioSource audioSource;
    Animator animator;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = FindObjectOfType<PlayerHealth>();
        animator = GetComponent<Animator>();

        explosionParticles.SetActive(false);
    }

    private void Update()
    {
        distanceToTarget = Vector3.Distance(player.transform.position, transform.position);

        if (exploded)
        {
            if (distanceToTarget <= damageRadius)
            {
                DealDamage();
            }
            damageRadius = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!activated)
            {
                BeepingAnimation();
                PlaySoundEffect();
                Destroy(gameObject, 5f);
            }
        }
    }

    void PlaySoundEffect()
    {
        AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
        Invoke("DisableChild", 2f);
        activated = true;
    }

    void DisableChild()
    {
        ActivateParticles();
        landmineModel.SetActive(false);
        exploded = true;
    }

    void ActivateParticles()
    {
        explosionParticles.SetActive(true);
    }

    void DealDamage()
    {
        player.TakeDamage(explosionDamage);
        exploded = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }

    void BeepingAnimation()
    {
        animator.SetTrigger("Activate");
    }
}
