using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DifficultySelection : MonoBehaviour
{
    public Button easyButton;
    public Button normalButton;
    public Button hardButton;
    public Button diabolicalButton;
    public Button quitButton;

    public static string difficultyLevel;

    void Start()
    {
        easyButton.onClick.AddListener(SetEasyDifficulty);
        normalButton.onClick.AddListener(SetNormalDifficulty);
        hardButton.onClick.AddListener(SetHardDifficulty);
        diabolicalButton.onClick.AddListener(SetDiabolicalDifficulty);
        quitButton.onClick.AddListener(QuitGame);
    }

    void SetEasyDifficulty()
    {
        difficultyLevel = "Easy";
        LoadNextScene();
    }

    void SetNormalDifficulty()
    {
        difficultyLevel = "Normal";
        LoadNextScene();
    }

    void SetHardDifficulty()
    {
        difficultyLevel = "Hard";
        LoadNextScene();
    }

    void SetDiabolicalDifficulty()
    {
        difficultyLevel = "Diabolical";
        LoadNextScene();
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
