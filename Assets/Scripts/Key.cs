using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] public int keyCount = 0;

    public int GetCurrentKeyValue()
    {
        return keyCount;
    }

    public void ReduceCurrentKey(int keySpent)
    {
        keyCount -= keySpent;
    }

    public void IncreaseCurrentKey(int keyPickValue)
    {
        keyCount += keyPickValue;
    }
}
