using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class ImbueStation : MonoBehaviour
{
    private PlayerSwapManager swapManager;
    private Interactable interactable;

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
        swapManager = FindAnyObjectByType<PlayerSwapManager>();
        if (swapManager == null)
            Debug.LogError("ImbueStation: PlayerSwapManager not found in scene!");
    }

    private void OnEnable()
    {
        if (interactable != null)
            interactable.onInteract.AddListener(HandleInteract);
    }

    private void OnDisable()
    {
        if (interactable != null)
            interactable.onInteract.RemoveListener(HandleInteract);
    }

    private void HandleInteract()
    {
        // Only swap if a player is active
        if (swapManager == null || swapManager.activeController == null) return;

        // Optional: Add distance check if needed
        swapManager.SwapCharacter();
    }
}
