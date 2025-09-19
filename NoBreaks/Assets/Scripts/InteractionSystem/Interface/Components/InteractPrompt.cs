using TMPro;
using UnityEngine;

public class InteractPrompt : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private Vector3 worldOffset = new(0f, 1f, 0f);
    [SerializeField] private string keyHint = "[E]";

    private Camera cam;
    private Transform target;
    private Canvas canvas;
    private RectTransform canvasRect;
    private RectTransform labelRect;

    void Awake()
    {
        cam = Camera.main;
        labelRect = label.rectTransform;
        canvas = label.GetComponentInParent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
        Hide();
    }

    void LateUpdate()
    {
        if (target == null) return;
        if (!label.gameObject.activeSelf)
        {
            label.gameObject.SetActive(true);
        }
        Vector3 wordPos = target.position + worldOffset;
        Vector3 screenPos = cam.WorldToScreenPoint(wordPos);
        Camera uiCam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : cam;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, uiCam, out Vector2 localPoint))
        {
            labelRect.anchoredPosition = localPoint;
        }
    }

    public void Show(IInteractable interactablle)
    {
        if (interactablle == null)
        {
            Hide();
            return;
        }
        target = interactablle.transform;
        label.text = $"{keyHint}{interactablle.DisplayName}";
        label.gameObject.SetActive(true);
    }
    public void Hide()
    {
        label.gameObject.SetActive(false);
        target = null; 
    }
}
