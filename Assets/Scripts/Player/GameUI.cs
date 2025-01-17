using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] private float gameTimer = 60f;
    [SerializeField] GameObject endGameUI;

    [SerializeField] TextMeshProUGUI maxScoreText;
    [SerializeField] TextMeshProUGUI currentScoreText;
    private int currentScore;
    private bool gameFinished = false;

    private void OnEnable()
    {
        Health.OnDeath += IncreaseScore;
        endGameUI.SetActive(false);
    }

    private void Update()
    {
        DecreaseTimer();
    }

    private void IncreaseScore(Health health)
    {
        currentScore++;
        score.text = currentScore.ToString();
    }

    private void DecreaseTimer()
    {
        if(gameFinished && Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }

        if (gameFinished)
            return;

        gameTimer -= Time.unscaledDeltaTime;
        timer.text = gameTimer.ToString("0.0");
        
        if (gameTimer < 0)
        {
            gameFinished = true;
            OpenEndGameUI();
        }
    }

    private void OpenEndGameUI()
    {
        Time.timeScale = 0f;
        endGameUI.SetActive(true);

        int maxScore = PlayerPrefs.GetInt("MaxScore", defaultValue: 0);
        maxScoreText.text = "Max Score : " + maxScore;
        currentScoreText.text = "Score : " + currentScore.ToString();

        if (currentScore > maxScore)
        {
            PlayerPrefs.SetInt("MaxScore", currentScore);
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
