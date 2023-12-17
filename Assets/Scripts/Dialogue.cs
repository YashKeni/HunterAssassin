using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textDisplay;
    [SerializeField] string[] sentences;
    [SerializeField] float typingSpeed;
    [SerializeField] GameObject continueButton;

    int index;
    int currentSceneIndex;

    private void Start()
    {
        StartCoroutine(AutoType());
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        continueButton.SetActive(false);
    }

    private void Update()
    {
        if (textDisplay.text == sentences[index])
        {
            continueButton.SetActive(true);
        }
    }

    IEnumerator AutoType()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
        continueButton.SetActive(false);
        if (index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(AutoType());
        }
        else
        {
            continueButton.SetActive(false);
            SceneManager.LoadScene(currentSceneIndex + 1);
        }

    }
}
