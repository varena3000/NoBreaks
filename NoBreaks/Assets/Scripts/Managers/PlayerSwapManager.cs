using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSwapManager : MonoBehaviour
{
    [Header("Controllers")]
    public FPController ratController;
    public FPController jenniController;

    [Header("Interactors")]
    public MonoBehaviour ratInteractor;    // assign PlayerInteractor script on Rat
    public MonoBehaviour jenniInteractor;  // assign PlayerInteractor script on Jenni

    private FPController activeController;

    void Start()
    {
        SetActiveCharacter(ratController);
    }

    private void SwapCharacter()
    {
        if (activeController == ratController)
            SetActiveCharacter(jenniController);
        else
            SetActiveCharacter(ratController);
    }

    private void SetActiveCharacter(FPController controller)
    {
        // Disable both controllers
        ratController.enabled = false;
        jenniController.enabled = false;

        // Disable both cameras
        if (ratController.characterCamera != null)
            ratController.characterCamera.gameObject.SetActive(false);
        if (jenniController.characterCamera != null)
            jenniController.characterCamera.gameObject.SetActive(false);

        // Disable AudioListeners
        if (ratController.characterCamera != null)
        {
            var audio = ratController.characterCamera.GetComponent<AudioListener>();
            if (audio) audio.enabled = false;
        }
        if (jenniController.characterCamera != null)
        {
            var audio = jenniController.characterCamera.GetComponent<AudioListener>();
            if (audio) audio.enabled = false;
        }

        // Disable interactors
        if (ratInteractor != null) ratInteractor.enabled = false;
        if (jenniInteractor != null) jenniInteractor.enabled = false;

        // Enable chosen controller
        activeController = controller;
        activeController.enabled = true;

        // Enable camera + AudioListener
        if (activeController.characterCamera != null)
        {
            activeController.characterCamera.gameObject.SetActive(true);
            var audio = activeController.characterCamera.GetComponent<AudioListener>();
            if (audio) audio.enabled = true;
        }

        // Enable correct interactor
        if (activeController == ratController && ratInteractor != null)
            ratInteractor.enabled = true;
        else if (activeController == jenniController && jenniInteractor != null)
            jenniInteractor.enabled = true;
    }

    #region Input Callbacks
    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (activeController != null)
            activeController.OnMove(ctx);
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        if (activeController != null)
            activeController.OnLook(ctx);
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (activeController != null)
            activeController.OnSprint(ctx);
    }

    public void OnCrouch(InputAction.CallbackContext ctx)
    {
        if (activeController != null)
            activeController.OnCrouch(ctx);
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (activeController != null)
            activeController.OnJump(ctx);
    }

    public void OnPickUp(InputAction.CallbackContext ctx)
    {
        if (activeController != null)
            activeController.OnPickUp(ctx);
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            SwapCharacter();
    }
    #endregion
}
