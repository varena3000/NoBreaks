using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private InputActionAsset pauseInput;

    private InputAction esc;
    private bool isPaused = false;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    private void Start()
    {
        var map = pauseInput.FindActionMap("OnFoot", true);
        esc = map.FindAction("Pause", true);
        esc.Enable(); // Enable the input action

        esc.performed += _ => TogglePause();
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        audioManager.PlaySFX(audioManager.Menu);

        // Pause or resume time
        Time.timeScale = isPaused ? 0 : 1;

        // Handle cursor visibility & lock state
        if (isPaused)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; // Free movement
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; // Locked to center
        }
    }

    private void OnDestroy()
    {
        esc.performed -= _ => TogglePause();
        audioManager.PlaySFX(audioManager.Menu);
    }
}
