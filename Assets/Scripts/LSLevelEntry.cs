using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LSLevelEntry : MonoBehaviour
{
    public bool canLoadLevel;
    public string levelName;
    public string levelToCheck;
    public string displayName;
    public bool _levelUnlocked;
    private bool _levelLoading;

    public TextMeshProUGUI lNameText;
    public GameObject lNamePanel;
    public GameObject imagePanel;
    public GameObject blackScreenPanel; // Panel for fade-to-black effect

    public float animationDuration = 1.0f;
    private Vector3 targetScale = new Vector3(3, 3, 3);
    private Color targetColor = new Color(1, 1, 1, 0.9f);

    public InputActionProperty rightPrimaryButton;

    private void Start()
    {
        lNamePanel.SetActive(false);
        imagePanel.SetActive(false);
        blackScreenPanel.SetActive(false); // Hide black screen at start
        canLoadLevel = false;
    }

    private void Update()
    {
        if (rightPrimaryButton.action.IsPressed() && canLoadLevel && !_levelLoading)
        {
            canLoadLevel = false;
            StartCoroutine(LevelLoadWaiter());
            _levelLoading = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canLoadLevel = true;
            StartCoroutine(AnimatePanel(imagePanel, targetScale, targetColor));
            lNamePanel.SetActive(true);
            lNameText.text = displayName;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canLoadLevel = false;
            StartCoroutine(AnimatePanel(imagePanel, Vector3.zero, new Color(1, 1, 1, 0f)));
            lNamePanel.SetActive(false);
        }
    }

    public IEnumerator LevelLoadWaiter()
    {
        // **Fade to black before loading**
        StartCoroutine(FadeToBlack(true));

        yield return new WaitForSeconds(2f); // Wait for fade

        SceneManager.LoadScene(levelName);

        // **Fade from black after loading**
        StartCoroutine(FadeToBlack(false));
    }

    private IEnumerator FadeToBlack(bool fadeIn)
    {
        CanvasGroup canvasGroup = blackScreenPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("El panel de pantalla negra necesita un CanvasGroup.");
            yield break;
        }

        blackScreenPanel.SetActive(true);
        float elapsedTime = 0f;
        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / animationDuration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;

        if (!fadeIn) blackScreenPanel.SetActive(false);
    }

    private IEnumerator AnimatePanel(GameObject panel, Vector3 targetScale, Color targetColor)
    {
        RectTransform rectTransform = panel.GetComponent<RectTransform>();
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();

        if (rectTransform == null || canvasGroup == null)
        {
            Debug.LogError("El panel necesita componentes RectTransform y CanvasGroup.");
            yield break;
        }

        Vector3 initialScale = rectTransform.localScale;
        Color initialColor = new Color(1, 1, 1, canvasGroup.alpha);
        float elapsedTime = 0f;

        panel.SetActive(true);

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;

            rectTransform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            canvasGroup.alpha = Mathf.Lerp(initialColor.a, targetColor.a, t);

            yield return null;
        }

        rectTransform.localScale = targetScale;
        canvasGroup.alpha = targetColor.a;

        if (targetScale == Vector3.zero) panel.SetActive(false);
    }
}
