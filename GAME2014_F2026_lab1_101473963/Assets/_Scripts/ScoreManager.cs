using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI Reference")]
    public TextMeshProUGUI scoreText;

    private int score = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (scoreText == null)
        {
            GameObject obj = GameObject.Find("ScoreText");
            if (obj != null)
            {
                scoreText = obj.GetComponent<TextMeshProUGUI>();
            }
        }

        UpdateScoreUI();
    }

    // Adds points when enemies are shot
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
