using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int Score = 0;
    public TMP_Text scoreText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        Score += amount;
        UpdateScoreUI();
        //Debug.Log($"Score: {Score}");
    }

    public void ResetScore()
    {
        Score = 0;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score : " + Score;
    }
}
