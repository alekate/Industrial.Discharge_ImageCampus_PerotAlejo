using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private UIController UIController;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject tutorialUI;

    [SerializeField] private bool isPaused;
    [SerializeField] private bool showTutorial = true;

    private float duration = 1f;
    private float startScale = 1f;
    private Coroutine currentCoroutine;

    public float timeRemaining = 60f; 
    [SerializeField] private TextMeshProUGUI timerText;
    public bool timerIsRunning = true;


    void Update()
    {
        TutorialUI();

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);

            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

        if (UIController.totalEnemies == 0)
        {
            UIController.FinnishGameUI();
            timeRemaining = 0;
            timerIsRunning = false;
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                LoadMainMenu();
            }
        }

        if (timerIsRunning && !isPaused)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay(timeRemaining);
            }

            if(timeRemaining <= 0)
            {
                timeRemaining = 0;
                timerIsRunning = false;
                UIController.LoseGameUI();
                Time.timeScale = 0f; 
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                if(Input.GetKeyDown(KeyCode.Space))
                {
                    LoadGame();
                }
            }
        }

    }

    public void PauseGame()
    {
        isPaused = true;
        gameplayUI.SetActive(false);
        pauseUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        currentCoroutine = StartCoroutine(SlowDownTime());
    }

    public void ResumeGame()
    {
        isPaused = false;
        gameplayUI.SetActive(true);
        pauseUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentCoroutine = StartCoroutine(SpeedUpTime());
    }

    public void TutorialUI()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            showTutorial = !showTutorial;

            if (showTutorial == true)
            {
                tutorialUI.gameObject.SetActive(true);
            }
            if (showTutorial == false)
            {
                tutorialUI.gameObject.SetActive(false);
            }
        }
    }

    public void LoadGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene("Level1");
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadCredits()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Credits");
    }

    private void UpdateTimerDisplay(float timeToDisplay)
    {
        timeToDisplay += 1;
        int hours = Mathf.FloorToInt(timeToDisplay / 3600);
        int minutes = Mathf.FloorToInt((timeToDisplay % 3600) / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("Time Remaining: {0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    private IEnumerator SlowDownTime()
    {
        float elapsed = 0f;

        while (Time.timeScale > 0.01f)
        {
            elapsed += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(startScale, 0f, elapsed / duration);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            yield return null;
        }

        Time.timeScale = 0f;
    }

    private IEnumerator SpeedUpTime()
    {
        float elapsed = 0f;

        while (Time.timeScale < 0.99f)
        {
            elapsed += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(0f, startScale, elapsed / duration);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            yield return null;
        }

        Time.timeScale = 1f;
    }

}

