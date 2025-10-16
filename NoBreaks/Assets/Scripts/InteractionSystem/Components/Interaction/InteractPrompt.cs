using System.Reflection.Emit;
using TMPro;
using UnityEngine;

public class InteractPrompt : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private Vector3 worldOffset = new(0f, 1f, 0f);

    [SerializeField]
    private Camera cam;
    private Transform target;
    private Canvas canvas;
    private RectTransform canvasRect;
    private RectTransform labelRect;

    void Awake()
    {
        cam = FindAnyObjectByType<Camera>();
        labelRect = label.rectTransform;
        canvas = label.GetComponentInParent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();

        Hide();
    }

    void LateUpdate()
    {
        if (target == null)
            return;
        
        if(cam == null || !cam.gameObject.activeInHierarchy)
        {
            cam = FindAnyObjectByType<Camera>();
        }
            
        if (!label.gameObject.activeSelf)
        {
            label.gameObject.SetActive(true);
        }

        Vector3 worldPos = target.position + worldOffset;
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

        // Hide if behind camera
        if (screenPos.z < 0f)
        {
            if (label.gameObject.activeSelf)
            {
                label.gameObject.SetActive(false);
            }
            return;
        }
        
        if (!label.gameObject.activeSelf)
        {
            label.gameObject.SetActive(true);
        }

        Camera uiCam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : cam;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, uiCam, out Vector2 localPoint))
        {
            labelRect.anchoredPosition = localPoint;
        }
    }

    public void Show(IInteractable interactable)
    {
        if (interactable == null)
        {
            Hide();
            return;
        }
        target = interactable.transform;
        label.text = $"{interactable.KeyHint}{interactable.DisplayName}";
        label.gameObject.SetActive(true);

    }

    public void Hide()
    {
        label.gameObject.SetActive(false);
        target = null;
    }
}
