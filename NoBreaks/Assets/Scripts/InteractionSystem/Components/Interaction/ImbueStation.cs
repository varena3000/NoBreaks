using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class ImbueStation : MonoBehaviour
{
    private PlayerSwapManager swapManager;

    private void Awake()
    {
        swapManager = FindAnyObjectByType<PlayerSwapManager>();
    }

    public void HandleImbue()
    {
        // Only swap if a player is active
        if (swapManager == null || swapManager.activeController == null) return;

        // Add distance check if needed
        swapManager.SwapCharacter();
    }
}
