using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] TMP_Text gemCountText;

    [Header("Reward Buttons")]
    [SerializeField] RewardsButton button1;
    [SerializeField] RewardsButton button2;
    [SerializeField] RewardsButton button3;

    public int gemCount;

    void Start()
    {
        button1.Initialize(this);
        button2.Initialize(this);
        button3.Initialize(this);

        gemCount = PlayerDataManager.LoadGemCount();
        gemCountText.text = gemCount.ToString();
    }

    void Update()
    {
        gemCountText.text = gemCount.ToString();
    }

    public void IncreaseGems(RewardsButton button)
    {
        gemCount += button.gemValue;
        PlayerDataManager.SaveGemCount(gemCount);
    }

    public void DecreaseGems()
    {
        gemCount -= FindObjectOfType<ShopItemButton>().gemValue;
        PlayerDataManager.SaveGemCount(gemCount);
    }
}
