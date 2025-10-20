using UnityEngine;
using TMPro;
using System.Collections;

public class TemporaryTMP : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float displayTime = 3f;     // How long it stays visible
    [SerializeField] private float fadeDuration = 1f;    // Fade-out time

    [Header("Bounce Effect")]
    [SerializeField] private float bounceDuration = 0.5f;
    [SerializeField] private float bounceScale = 1.2f;

    private TextMeshProUGUI tmpText;
    private CanvasGroup canvasGroup;
    private Vector3 originalScale;

    void Awake()
    {
        tmpText = GetComponent<TextMeshProUGUI>();

        // Add a CanvasGroup dynamically if it’s not there (for easy fading)
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        originalScale = transform.localScale;
        canvasGroup.alpha = 0f;
        transform.localScale = Vector3.zero;
    }

    void Start()
    {
        StartCoroutine(AnimateTMP());
    }

    private IEnumerator AnimateTMP()
    {
        // --- Bounce In ---
        float t = 0f;
        while (t < bounceDuration)
        {
            t += Time.deltaTime;
            float normalized = t / bounceDuration;
            float scale = Mathf.Lerp(0f, bounceScale, Mathf.Sin(normalized * Mathf.PI * 0.5f)); // smooth in
            transform.localScale = originalScale * scale;
            canvasGroup.alpha = Mathf.Clamp01(normalized * 2f);
            yield return null;
        }

        transform.localScale = originalScale * bounceScale;

        // --- Ease back to normal scale ---
        t = 0f;
        while (t < 0.15f)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale * bounceScale, originalScale, t / 0.15f);
            yield return null;
        }

        transform.localScale = originalScale;

        // --- Wait for display time ---
        yield return new WaitForSeconds(displayTime);

        // --- Fade Out ---
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            canvasGroup.alpha = alpha;
            yield return null;
        }

        Destroy(gameObject);
    }
}
