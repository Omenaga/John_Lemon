using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float easyTime = 75f;
    public float normalTime = 60f;
    public float hardTime = 50f;
    public float diabolicalTime = 45f;

    private float currentTime;
    private GameEnding gameEnding;

    public TextMeshProUGUI timerText;

    void Start()
    {
        gameEnding = FindObjectOfType<GameEnding>();

        switch (DifficultySelection.difficultyLevel)
        {
            case "Easy":
                currentTime = easyTime;
                break;
            case "Normal":
                currentTime = normalTime;
                break;
            case "Hard":
                currentTime = hardTime;
                break;
            case "Diabolical":
                currentTime = diabolicalTime;
                break;
            default:
                currentTime = normalTime;
                break;
        }
    }

    void Update()
    {
        currentTime -= Time.deltaTime;
        currentTime = Mathf.Clamp(currentTime, 0f, Mathf.Infinity);

        // Format and display the time
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        int milliseconds = Mathf.FloorToInt((currentTime % 1f) * 1000f);

        timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);

        if (currentTime <= 0)
        {
            if (gameEnding != null)
            {
                gameEnding.CaughtPlayer();
            }
        }
    }
}
