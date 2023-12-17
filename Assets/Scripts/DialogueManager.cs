using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public Image actorImage;
    public TextMeshProUGUI actorName;
    public TextMeshProUGUI messageText;

    public GameObject dialogueBox;
    public GameObject startConvoButton;
    public GameObject continueConvoButton;
    [SerializeField] SceneLoader sceneLoader;

    Message[] currentMessages;
    Actor[] currentActors;

    int activeMessage = 0;
    int currentSceneIndex;
    public static bool isActive = false;

    void Start()
    {
        dialogueBox.SetActive(false);
        continueConvoButton.SetActive(false);
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    void Update()
    {

    }

    public void OpenDialogue(Message[] messages, Actor[] actors)
    {
        dialogueBox.SetActive(true);
        startConvoButton.SetActive(false);
        continueConvoButton.SetActive(true);

        currentMessages = messages;
        currentActors = actors;
        activeMessage = 0;
        isActive = true;
        DisplayMessage();
    }

    void DisplayMessage()
    {
        Message messageToDisplay = currentMessages[activeMessage];
        messageText.text = messageToDisplay.message;

        Actor actorToDisplay = currentActors[messageToDisplay.actorId];
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;
    }

    public void NextMessage()
    {
        activeMessage++;
        if (activeMessage < currentMessages.Length)
        {
            DisplayMessage();
        }
        else
        {
            isActive = false;
            sceneLoader.LoadScene(currentSceneIndex + 1);
            // SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }
}
