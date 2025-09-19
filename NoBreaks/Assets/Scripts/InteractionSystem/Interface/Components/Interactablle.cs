using UnityEngine;
using UnityEngine.Events;

public class Interactablle : MonoBehaviour, IInteractable
{
    [SerializeField]
    private string displayName = "Interact";

    [SerializeField]
    private bool isEnabled = true;

    [SerializeField]
    private UnityEvent onInteract;


    public string DisplayName => displayName;

    public bool CanInteract() => isEnabled;
   

    public void Interact()
    {
        onInteract?.Invoke();
    }

    public void OnfocusGained()
    {
        throw new System.NotImplementedException();
    }

    public void OnfocusLost()
    {
        throw new System.NotImplementedException();
    }
}
