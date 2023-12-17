using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour
{
    [Header("Player Prefab")]
    [SerializeField] GameObject playerPrefab;

    [Header("Speed Powerup")]
    [SerializeField] float speedMultiplier;
    MainMenuUIManager mainMenuUIManager;

    [Header("Misc")]
    public int gemValue;
    public string itemKey;

    private bool isPurchased = false;

    void Start()
    {
        mainMenuUIManager = FindObjectOfType<MainMenuUIManager>();
        isPurchased = PlayerPrefs.GetInt(itemKey, 0) == 1;
        UpdateInteractability();
    }

    void Update()
    {
        if (mainMenuUIManager.gemCount <= gemValue || isPurchased)
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
        else
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
    }

    public void SpeedUp()
    {
        if (!isPurchased)
        {
            playerPrefab.GetComponent<PlayerMovement>().playerMoveSpeed *= speedMultiplier;

            PlayerPrefs.SetInt(itemKey, 1);
            PlayerPrefs.Save();

            isPurchased = true;
            UpdateInteractability();
        }
    }


    void UpdateInteractability()
    {
        gameObject.GetComponent<Button>().interactable = !isPurchased;
    }
}
