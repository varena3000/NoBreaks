using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Interactable))]
public class ImbueStation : MonoBehaviour
{
    private PlayerSwapManager swapManager;
    private Interactable interactable;
    private InputAction Imbue;

    private void Awake()
    {
        Imbue = InputSystem.actions.FindAction("Imbue");
        
        interactable = GetComponent<Interactable>();
        swapManager = FindAnyObjectByType<PlayerSwapManager>();
    }

    private void OnEnable()
    {
        if (Imbue != null)
            Imbue.performed += OnImbue;
    }

    private void OnDisable()
    {
        if (Imbue != null)
            Imbue.performed -= OnImbue;
    }

    private void HandleInteract()
    {
        // Only swap if a player is active
        if (swapManager == null || swapManager.activeController == null) return;

        // Add distance check if needed
        swapManager.SwapCharacter();
    }
    #region Input Callback

    private void OnImbue(InputAction.CallbackContext context)
    {
        HandleInteract();
    }
    
    #endregion
}
