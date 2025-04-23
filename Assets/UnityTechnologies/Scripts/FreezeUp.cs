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

    // List of words for each difficulty
    private Dictionary<string, string[]> difficultyWords = new Dictionary<string, string[]>()
    {
        { "Easy", new string[] {"e", "m", "c", "2"} },
        { "Normal", new string[] {"go", "move", "run", "bolt"} },
        { "Hard", new string[] {"escape", "break", "please"} },
        { "Diabolical", new string[] { "nigerundayo" } }
    };

    void Start()
    {
        // Get Difficulty
        string selectedDifficulty = DifficultySelection.difficultyLevel;

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
                minFreezeInterval = 7f;
                maxFreezeInterval = 12f;
                break;
            case "Diabolical":
                minFreezeInterval = 5f;
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

            Debug.Log(keyPressed);

            if (keyPressed == targetWord[index].ToString())
            {
                index++;

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
                index = 0;
            }
        }
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
                freezeMessage.text = "Type the word: " + targetWord;
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
}
