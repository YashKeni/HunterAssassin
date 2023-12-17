using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    [SerializeField] int gemsAmount = 0;
    [SerializeField] AudioClip gemSound;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        LoadGemCount();
    }

    public int GetCurrentGems()
    {
        return gemsAmount;
    }

    public void IncreaseGems(int gemvalue)
    {
        gemsAmount += gemvalue;
        audioSource.PlayOneShot(gemSound);

        SaveGemCount();
    }

    private void SaveGemCount()
    {
        PlayerDataManager.SaveGemCount(gemsAmount);
    }

    private void LoadGemCount()
    {
        gemsAmount = PlayerDataManager.LoadGemCount();
    }
}
