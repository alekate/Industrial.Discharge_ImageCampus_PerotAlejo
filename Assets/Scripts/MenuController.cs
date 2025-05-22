using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;

    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject tutorialUI;

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image loadingSlider;
    [SerializeField] private float maxTime = 3;

    [SerializeField] private bool isPaused;
    [SerializeField] private bool showTutorial = true;

    private float duration = 1f;
    private float startScale = 1f;
    private Coroutine currentCoroutine;

    private void Start()
    {
        if (loadingScreen != null)
            loadingScreen.SetActive(false);
    }

    private void Update()
    {
        HandleTutorialInput();

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);

            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    private void HandleTutorialInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            showTutorial = !showTutorial;

            if (tutorialUI != null)
                tutorialUI.SetActive(showTutorial);
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        if (gameplayUI != null) gameplayUI.SetActive(false);
        if (pauseUI != null) pauseUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        currentCoroutine = StartCoroutine(SlowDownTime());
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (gameplayUI != null) gameplayUI.SetActive(true);
        if (pauseUI != null) pauseUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentCoroutine = StartCoroutine(SpeedUpTime());
    }

    public void LoadGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        StartCoroutine(LoadGameAsynchronously());
    }

    private IEnumerator LoadGameAsynchronously()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Level1");
        operation.allowSceneActivation = false;

        float onTime = 0f;
        float percentage = 0.9f;

        // Simulación de carga (fake)
        while (onTime < maxTime * percentage)
        {
            onTime += Time.deltaTime * 10f;
            if (loadingSlider != null)
                loadingSlider.fillAmount = Mathf.SmoothStep(loadingSlider.fillAmount, onTime / maxTime, 0.2f);

            yield return null;
        }

        // Esperar que la escena se cargue realmente
        while (operation.progress < 0.89f)
        {
            yield return null;
        }

        // Completar el resto de la barra
        while (onTime < maxTime)
        {
            onTime += Time.deltaTime * 10f;
            if (loadingSlider != null)
                loadingSlider.fillAmount = Mathf.SmoothStep(loadingSlider.fillAmount, onTime / maxTime, 0.2f);

            yield return null;
        }

        loadingScreen.SetActive(false);

        operation.allowSceneActivation = true;
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
