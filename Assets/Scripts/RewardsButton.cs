using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardsButton : MonoBehaviour
{
    [Header("Misc")]
    public int gemValue;
    public string itemKey;

    private bool isCollected = false;

    MainMenuUIManager mainMenuUIManager;

    // void Start()
    // {
    //     mainMenuUIManager = FindObjectOfType<MainMenuUIManager>();
    //     isCollected = PlayerPrefs.GetInt(itemKey, 0) == 1;
    // }

    public void Initialize(MainMenuUIManager mmManager)
    {
        mainMenuUIManager = mmManager;
        isCollected = PlayerPrefs.GetInt(itemKey, 0) == 1;
    }

    void Update()
    {
        if (isCollected)
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
        else
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
    }

    public void CollectedGems()
    {
        PlayerPrefs.SetInt(itemKey, 1);
        PlayerPrefs.Save();

        isCollected = true;
    }
}
