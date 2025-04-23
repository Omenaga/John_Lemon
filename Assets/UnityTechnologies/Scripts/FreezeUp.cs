using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FreezeUp : MonoBehaviour
{
    public PlayerMovement player;
    public TextMeshProUGUI freezeMessage;
    public float minFreezeInterval;
    public float maxFreezeInterval;

    private string targetWord;
    private int index = 0;
    private bool waitingForInput = false;

    string selectedDifficulty = "Normal";

    // List of words for each difficulty
    private Dictionary<string, string[]> difficultyWords = new Dictionary<string, string[]>()
    {
        { "Easy", new string[] {"e", "m", "c", "2", "go"} },
        { "Normal", new string[] {"move", "run", "bolt", "break", "ghost"} },
        { "Hard", new string[] {"escape", "please", "motion", "gargoyle", "breakout"} },
        { "Diabolical", new string[] {"nigerundayo", "diabolical", "pseudonism", "thorough", "through", "though", "LohnJemon"} }
    };

    void Start()
    {
        // Get Difficulty
        selectedDifficulty = DifficultySelection.difficultyLevel;

        // Difficulty Settings
        switch (selectedDifficulty)
        {
            case "Easy":
                minFreezeInterval = 10f;
                maxFreezeInterval = 20f;
                break;
            case "Normal":
                minFreezeInterval = 10f;
                maxFreezeInterval = 15f;
                break;
            case "Hard":
                minFreezeInterval = 8f;
                maxFreezeInterval = 12.5f;
                break;
            case "Diabolical":
                minFreezeInterval = 7f;
                maxFreezeInterval = 10f;
                break;
        }

        StartCoroutine(FreezeRoutine());
    }

    void Update()
    {
        if (waitingForInput && Input.anyKeyDown)
        {
            string keyPressed = Input.inputString.ToLower();

            if (!string.IsNullOrEmpty(keyPressed))
            {
                Debug.Log(keyPressed);

                if (keyPressed == targetWord[index].ToString())
                {
                    index++;
                    UpdateFreezeMessage();

                    if (index >= targetWord.Length)
                    {
                        // Player is unfrozen
                        waitingForInput = false;
                        player.isFrozen = false;
                        index = 0;
                    }
                }
                else
                {
                    // Restart Progress
                    StartCoroutine(FlashRed());
                    index = 0;
                }
            }
        }
    }

    void UpdateFreezeMessage()
    {
        if (freezeMessage == null || string.IsNullOrEmpty(targetWord))
        {
            return;
        }

        string sameText = "Type the word: ";

        for (int i = 0; i < targetWord.Length; i++)
        {
            if (i < index)
            {
                // Past Letters
                sameText += $"<color=green>{targetWord[i]}</color>";
            }
            else if (i == index)
            {
                // Current Letter
                sameText += $"<color=yellow>{targetWord[i]}</color>";
            }
            else
            {
                // Remaining Letters
                sameText += $"<color=white>{targetWord[i]}</color>";
            }
        }

        freezeMessage.text = sameText;
    }

    IEnumerator FreezeRoutine()
    {
        while (true)
        {
            // Activates after random range of time
            float waitTime = Random.Range(minFreezeInterval, maxFreezeInterval);
            yield return new WaitForSeconds(waitTime);

            // Select a random word from the list based on difficulty
            string[] words = difficultyWords[selectedDifficulty];
            targetWord = words[Random.Range(0, words.Length)]; // Randomly select a word

            // Freezes player and sets up user input
            player.isFrozen = true;
            waitingForInput = true;
            index = 0;

            if (freezeMessage != null)
            {
                // Show message
                UpdateFreezeMessage();
                freezeMessage.gameObject.SetActive(true);
            }

            // Waits till move is typed
            yield return new WaitUntil(() => waitingForInput == false);

            if (freezeMessage != null)
            {
                // Hide message
                freezeMessage.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator FlashRed()
    {
        freezeMessage.text = $"<color=red>Type the word: {targetWord}</color>";
        yield return new WaitForSeconds(0.5f);

        UpdateFreezeMessage();
    }
}
