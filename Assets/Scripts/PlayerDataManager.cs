using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    private const string GemCountKey = "GemCount";

    private static PlayerDataManager instance;

    public static PlayerDataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerDataManager>();
            }
            return instance;
        }
    }

    public static void SaveGemCount(int gemCount)
    {
        PlayerPrefs.SetInt(GemCountKey, gemCount);
        PlayerPrefs.Save();
    }

    public static int LoadGemCount()
    {
        return PlayerPrefs.GetInt(GemCountKey, 0);
    }
}
