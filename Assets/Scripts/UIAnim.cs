using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnim : MonoBehaviour
{
    Animator animator;

    public bool hasInteracted = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (hasInteracted)
        {
            animator.SetBool("interacted", true);
            hasInteracted = false;
        }
    }
}
