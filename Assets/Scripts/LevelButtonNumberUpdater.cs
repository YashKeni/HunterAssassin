using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonNumberUpdater : MonoBehaviour
{
    [SerializeField] SceneLoader sceneLoader;

    void Start()
    {
        TMP_Text levelButtonText = GetComponentInChildren<TMP_Text>();

        string buttonName = gameObject.name;
        int startIndex = buttonName.IndexOf("(");
        int endIndex = buttonName.IndexOf(")");

        if (startIndex != -1 && endIndex != -1)
        {
            string numberString = buttonName.Substring(startIndex + 1, endIndex - startIndex - 1);

            if (int.TryParse(numberString, out int levelNumber))
            {
                levelButtonText.text = $"{levelNumber}";

                Button levelButton = GetComponent<Button>();
                levelButton.onClick.AddListener(() => LoadLevel(levelNumber + 2));
            }
        }
    }

    void LoadLevel(int levelNumber)
    {
        sceneLoader.LoadScene(levelNumber);
    }
}
