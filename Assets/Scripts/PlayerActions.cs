using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] bool hasAttacked;

    [Header("Collectibles")]
    [SerializeField] Transform gems;
    [SerializeField] float spawnRadius;
    [SerializeField] int minGems;
    [SerializeField] int maxGems;

    [Header("Effects")]
    [SerializeField] ParticleSystem bloodParticles;
    [SerializeField] GameObject bloodSplash;
    [SerializeField] AudioClip knifeSFX;

    Animator animator;
    AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (hasAttacked)
        {
            animator.SetBool("Attack", false);
            hasAttacked = false;
        }
    }

    IEnumerator SpawnGems(Vector3 spawnLocation)
    {
        yield return null;
        int noOfGems = Random.Range(minGems, maxGems);
        for (int i = 0; i < noOfGems; i++)
        {
            Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;
            randomPosition.y = 0.1f;
            Vector3 spawnPosition = spawnLocation + randomPosition;

            Instantiate(gems, spawnPosition, Quaternion.identity);
        }

    }

    void KnifeStabSound()
    {
        if (knifeSFX != null)
        {
            audioSource.PlayOneShot(knifeSFX);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (!hasAttacked)
            {
                KnifeStabSound();
                bloodParticles.Play();
                bloodSplash.transform.localScale = new Vector3(1f, 1f, 1f);
                Instantiate(bloodSplash, other.gameObject.transform.position, Quaternion.identity);
                StartCoroutine(SpawnGems(other.gameObject.transform.position));
                Destroy(other.gameObject);
                hasAttacked = true;
            }
            else
            {
                return;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
