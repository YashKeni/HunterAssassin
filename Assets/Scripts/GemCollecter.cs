using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCollecter : MonoBehaviour
{
    [SerializeField] int gemValue;

    private void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<PlayerWallet>().IncreaseGems(gemValue);
            FindObjectOfType<UIAnim>().hasInteracted = true;
            Destroy(gameObject);
        }
    }


}
