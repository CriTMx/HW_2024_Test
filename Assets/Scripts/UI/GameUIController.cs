using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUIController : MonoBehaviour
{

    // TMP GUI components
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TextMeshProUGUI gameOverText;

    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private TextMeshProUGUI pauseText;

    [SerializeField] private GameObject scoreboard;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;

    private void Awake()
    {
        PlayerDeath.OnPlayerDeath += ShowGameOverScreen;

        // Disable game over screen at the start of the game
        gameOverScreen.SetActive(false);
    }


    void Start()
    {
        // Initialize scoreboard text
        scoreText.text = "Score\n" + PlayerScore.GetScore().ToString();
        timeText.text = "Time\n" + PlayerScore.GetGameTime().ToString("#.#") + "s";
    }

    void Update()
    {
        scoreText.text = "Score\n" + PlayerScore.GetScore().ToString();
        timeText.text = "Time\n" + PlayerScore.GetGameTime().ToString("#.#") + "s";

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePauseScreen();
        }
    }

    void ShowGameOverScreen()
    {
        /* Summarize score and add it as text 
         * on the game over screen,
         * disable scoreboard UI and 
         * show game over screen */

        Debug.Log(PlayerScore.GetScore());
        Debug.Log(PlayerScore.GetGameTime());

        string gameOverResultText = 
            "Game Over!" + "\nScore: " 
            + PlayerScore.GetScore().ToString() + 
            "\nTime: " + PlayerScore.GetGameTime().ToString("#.#") + "s";

        gameOverText.text = gameOverResultText;
        gameOverScreen.SetActive(true);
        PlayerScore.ResetScoreboard();
        scoreboard.SetActive(false);
    }

    void TogglePauseScreen()
    {
        pauseScreen.SetActive(!pauseScreen.activeSelf);

        if (!pauseScreen.activeSelf)
        {
            Time.timeScale = 1f;
            return;
        }

        UpdatePauseScreenText();
        Time.timeScale = 0f;
    }

    void UpdatePauseScreenText()
    {
        pauseText.text =
            "Game paused" + "\nCurrent Score: "
            + PlayerScore.GetScore().ToString();
    }


    private void OnDestroy()
    {
        PlayerDeath.OnPlayerDeath -= ShowGameOverScreen;
    }

    private void OnDisable()
    {
        PlayerDeath.OnPlayerDeath -= ShowGameOverScreen;
    }

}
