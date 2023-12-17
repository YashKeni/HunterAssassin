using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float timeAfterDrop = 2f;
    [SerializeField] float force;

    Rigidbody rb;
    SphereCollider sphereCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        sphereCollider = GetComponent<SphereCollider>();

        StartCoroutine(CollectAfterTime());
    }

    void Update()
    {
        Invoke("GoToPlayer", timeAfterDrop);
    }

    private void GoToPlayer()
    {
        Vector3 distance = player.transform.position - transform.position;


        distance = distance.normalized;
        distance *= force;

        rb.AddForce(distance);
        DestoryOverAll();
    }

    void DestoryOverAll()
    {
        Destroy(gameObject, 5f);
    }

    IEnumerator CollectAfterTime()
    {
        sphereCollider.enabled = false;
        yield return new WaitForSeconds(1f);
        sphereCollider.enabled = true;
    }
}
