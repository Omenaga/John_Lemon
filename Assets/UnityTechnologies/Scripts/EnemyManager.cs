using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject easyEnemies;
    public GameObject normalEnemies;
    public GameObject hardEnemies;
    public GameObject diabolicalEnemies;

    void Start()
    {
        string difficulty = DifficultySelection.difficultyLevel;

        DisableAll();

        switch (difficulty)
        {
            case "Easy":
                easyEnemies.SetActive(true);
                break;
            case "Normal":
                normalEnemies.SetActive(true);
                break;
            case "Hard":
                hardEnemies.SetActive(true);
                break;
            case "Diabolical":
                diabolicalEnemies.SetActive(true);
                break;
        }
    }

    void DisableAll()
    {
        easyEnemies.SetActive(false);
        normalEnemies.SetActive(false);
        hardEnemies.SetActive(false);
        diabolicalEnemies.SetActive(false);
    }
}
