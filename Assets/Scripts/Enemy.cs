using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public int health = 100;


    private void Start()
    {

    }

    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }

    }

    public void DecreaseHealth(int hitpoint)
    {
        health = health - hitpoint;
    }

}
