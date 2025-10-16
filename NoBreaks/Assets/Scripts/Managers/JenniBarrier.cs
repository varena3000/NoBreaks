using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class JenniBarrier : MonoBehaviour
{
    private PlayerSwapManager playerSwapManager;
    [SerializeField]
    private Collider barrierCollider;

    private void Awake()
    {
        barrierCollider = GetComponent<Collider>();
        barrierCollider.isTrigger = false; // Ensure it's a physical barrier
    }

    private void OnEnable()
    {
        // Try to find the manager every time this object becomes active
        playerSwapManager = FindAnyObjectByType<PlayerSwapManager>();
        if (playerSwapManager != null)
        {
            playerSwapManager.OnActiveCharacterChanged += HandleActiveCharacterChanged;
            HandleActiveCharacterChanged(); // Set initial state
        }
        else
        {
            Debug.LogWarning("JenniBarrier: PlayerSwapManager not found in scene yet.");
        }

        // Also listen to scene load to re-find the manager
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        if (playerSwapManager != null)
            playerSwapManager.OnActiveCharacterChanged -= HandleActiveCharacterChanged;

        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        // Re-find PlayerSwapManager after a scene loads
        playerSwapManager = FindAnyObjectByType<PlayerSwapManager>();
        if (playerSwapManager != null)
        {
            playerSwapManager.OnActiveCharacterChanged += HandleActiveCharacterChanged;
            HandleActiveCharacterChanged();
        }
    }

    private void HandleActiveCharacterChanged()
    {
        if (playerSwapManager != null && playerSwapManager.activeController != null)
        {
            // Enable the collider only if Jenni is active
            bool isJenni = playerSwapManager.activeController == playerSwapManager.jenniController;
            barrierCollider.enabled = isJenni;
        }
        else
        {
            // No active controller, disable collider just in case
            barrierCollider.enabled = false;
        }
    }
}
