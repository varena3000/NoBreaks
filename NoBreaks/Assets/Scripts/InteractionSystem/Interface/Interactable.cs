using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private string displayName = "Interact";

    [SerializeField]
    private bool isEnabled = true;

    [SerializeField]
    public UnityEvent onInteract;

    public string DisplayName => displayName;
    public bool CanInteract() => isEnabled;
    private Outline outline;

     private void Awake()
    {
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineColor = Color.green;
        outline.OutlineWidth = 1f;
        outline.enabled = false;
    }
   

    public void Interact()
    {
        onInteract?.Invoke();
    }

    public void OnfocusGained()
    {
        outline.enabled = true;
    }

    public void OnfocusLost()
    {
        outline.enabled = false;
    }
}
