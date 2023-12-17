using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnSpawn : MonoBehaviour
{
    private void Update()
    {
        Destroy(gameObject, 5f);
    }
}
