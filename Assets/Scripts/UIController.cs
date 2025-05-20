using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Crosshair")]
    [SerializeField] private TMP_Text crosshairText;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color shootColor = Color.red;
    [SerializeField] private float crosshairChangeDuration = 0.1f;

    [Header("Timer")]
    [SerializeField] private TMP_Text timerText;
    private float elapsedTime = 0f;

    [Header("Health")]
    [SerializeField] private TMP_Text healthText;

    [Header("WhiteFlash")]
    public Image whiteFlashImage;
    public float flashDuration = 1f;

    public void UpdateHealth(float current, float max)
    {
        healthText.text = $"HP: {current}/{max}";
    }

    private void Start()
    {
        SetDefaultCrosshair();
    }

    private void Update()
    {
        UpdateTimer();
    }

    public void FlashRedCrosshair()
    {
        StopAllCoroutines();
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        crosshairText.color = shootColor;
        yield return new WaitForSeconds(crosshairChangeDuration);
        SetDefaultCrosshair();
    }

    private void SetDefaultCrosshair()
    {
        crosshairText.color = defaultColor;
    }

    private void UpdateTimer()
    {
        elapsedTime += Time.deltaTime;

        int hours = Mathf.FloorToInt(elapsedTime / 3600f);
        int minutes = Mathf.FloorToInt((elapsedTime % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timerText.text = $"{hours:00}:{minutes:00}:{seconds:00}";
    }

    public void PlayWhiteFlash()
    {
        StartCoroutine(WhiteFlashCoroutine());
    }

    private IEnumerator WhiteFlashCoroutine()
    {
        Color color = whiteFlashImage.color;

        float t = 0;
        while (t < flashDuration / 2f)
        {
            t += Time.deltaTime;

            color.a = Mathf.Lerp(0, 1, t / (flashDuration / 2f));

            whiteFlashImage.color = color;

            yield return null;
        }

        t = 0;
        while (t < flashDuration / 2f)
        {
            t += Time.deltaTime;

            color.a = Mathf.Lerp(1, 0, t / (flashDuration / 2f));

            whiteFlashImage.color = color;

            yield return null;
        }
    }
}
