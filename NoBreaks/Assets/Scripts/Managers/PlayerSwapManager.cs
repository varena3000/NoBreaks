using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerSwapManager : MonoBehaviour
{
    [Header("Player Controllers (root GameObjects!)")]
    public FPController ratController;
    public FPController jenniController;

    [Header("Interactors (scripts on player)")]
    public MonoBehaviour ratInteractor;
    public MonoBehaviour jenniInteractor;

    [Header("Stamina (scripts on player)")]
    public StaminaController jenniStamina;
    public StaminaController ratStamina;

    [Header("Scene Settings")]
    public int targetSceneIndexForAutoLoad = 2;

    [HideInInspector]
    public FPController activeController;

    public event Action OnActiveCharacterChanged;

    // Static ensures last active character persists across scenes
    private static string lastActiveCharacter = "Rat";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        // Initialize the active player on first scene load
        FPController toActivate = (lastActiveCharacter == "Jenni") ? jenniController : ratController;
        SetActiveCharacter(toActivate);
    }

    
    // Swaps the active player
    public void SwapCharacter()
    {
        FPController toActivate = (activeController == ratController) ? jenniController : ratController;
        SetActiveCharacter(toActivate);

        // Auto-load next scene if Jenni is active and scene matches
        if (activeController == jenniController)
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            if (currentScene == targetSceneIndexForAutoLoad)
            {
                SceneManager.LoadScene(currentScene + 1);
            }
        }
    }

    // Activates the selected controller and disables the other
    private void SetActiveCharacter(FPController controller)
    {
        if (activeController == controller) return; // Already active, no changes needed

        activeController = controller;
        lastActiveCharacter = (controller == ratController) ? "Rat" : "Jenni";

        // Enable controller
        activeController.gameObject.SetActive(true);
        activeController.enabled = true;

        // Enable and disable the correct FPController
        if (activeController == ratController)
        {
            if (ratController != null)
                ratController.enabled = true;
                
            if (jenniController != null)
                jenniController.enabled = false;
                
            jenniController.gameObject.SetActive(false);
        }
        else
        {
            if (jenniController != null) 
                jenniController.enabled = true;
            if (ratController!= null)
                ratController.enabled = false;
                
            ratController.gameObject.SetActive(false);
        }

        // Enable camera & audio
        if (activeController.characterCamera != null)
        {
            activeController.characterCamera.gameObject.SetActive(true);
            var audio = activeController.characterCamera.GetComponent<AudioListener>();
            if (audio != null) audio.enabled = true;
        }

        // Enable correct interactor, disable the other
        if (activeController == ratController)
        {
            if (ratInteractor != null)
                ratInteractor.enabled = true;

            if (jenniInteractor != null)
                jenniInteractor.enabled = false;

            jenniController.gameObject.SetActive(false);
        }
        else
        {
            if (jenniInteractor != null)
                jenniInteractor.enabled = true;
            if (ratInteractor != null)
                ratInteractor.enabled = false;

            ratController.gameObject.SetActive(false);
        }

        //Enable and disable the correct PlayerStamina script
        if (activeController == ratController)
        {
            if (ratStamina != null)
            {
                ratStamina.enabled = true;
                jenniStamina.playerStamina = 100.0f;
            }
                
            if (jenniStamina != null)
                jenniStamina.enabled = false;

            jenniController.gameObject.SetActive(false);
            
        }
        else
        {
            if (jenniStamina != null)
            {
                jenniStamina.enabled = true;
                ratStamina.playerStamina = 100.0f;
            }
                
            if (ratStamina != null)
                ratStamina.enabled = false;

            ratController.gameObject.SetActive(false);
        }

        // Make active player persistent
        DontDestroyOnLoad(activeController.gameObject);

        // Fire event for other systems (barriers, UI, etc.)
        OnActiveCharacterChanged?.Invoke();
    }

    #region Input Callbacks
    public void OnMove(InputAction.CallbackContext ctx) => activeController?.OnMove(ctx);
    public void OnLook(InputAction.CallbackContext ctx) => activeController?.OnLook(ctx);
    public void OnSprint(InputAction.CallbackContext ctx) => activeController?.OnSprint(ctx);
    public void OnCrouch(InputAction.CallbackContext ctx) => activeController?.OnCrouch(ctx);
    public void OnJump(InputAction.CallbackContext ctx) => activeController?.OnJump(ctx);
    public void OnPickUp(InputAction.CallbackContext ctx) => activeController?.OnPickUp(ctx);
    public void OnInteract(InputAction.CallbackContext ctx) => activeController?.OnInteract(ctx);
    #endregion

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Determine which controller should be active
        FPController toActivate = (lastActiveCharacter == "Jenni") ? jenniController : ratController;

        // Only swap if needed
        if (activeController != toActivate)
        {
            SetActiveCharacter(toActivate);
        }

        // Move the active player to the spawn point if it exists
        if (activeController != null)
        {
            GameObject spawn = GameObject.Find("PlayerSpawnPoint");
            if (spawn != null)
            {
                activeController.transform.position = spawn.transform.position;
                activeController.transform.rotation = spawn.transform.rotation;
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.name == "Battery")
        {
            Destroy(collision.gameObject);
            SceneManager.LoadScene(targetSceneIndexForAutoLoad);
        }
    }
}
