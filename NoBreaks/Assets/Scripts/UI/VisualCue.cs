using UnityEngine;
using TMPro;

public class VisualCue : MonoBehaviour
{
    [TextArea] [SerializeField] 
    private string errorMessage;
    
    [SerializeField] private TMP_Text errorText;
    [SerializeField] private GameObject errorCanvas;

    private AudioManager audioManager;
    private PlayerSwapManager controller;

    [SerializeField] private Collider playerCollider;

    private void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        controller = FindAnyObjectByType<PlayerSwapManager>();

        // Make sure the canvas starts off
        if (errorCanvas != null)
            errorCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && controller.activeController == controller.jenniController)
        {
            if (errorCanvas != null && errorText != null)
            {
                errorCanvas.SetActive(true);
                errorText.text = errorMessage;
            }

            if (audioManager != null)
                audioManager.PlayErrorSFX(audioManager.Error);
        }

        if (other.CompareTag("Player") && controller.activeController == controller.ratController && other == playerCollider)
        {
            if (errorCanvas != null && errorText != null)
            {
                errorCanvas.SetActive(true);
                errorText.text = errorMessage;
            }

            if (audioManager != null)
                audioManager.PlayErrorSFX(audioManager.Error);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && controller.activeController == controller.jenniController)
        {
            if (errorCanvas != null && errorText != null)
            {
                errorCanvas.SetActive(true);
                errorText.text = "";
            }
        }

        if (other.CompareTag("Player") && controller.activeController == controller.ratController && other == playerCollider)
        {
            if (errorCanvas != null && errorText != null)
            {
                errorCanvas.SetActive(true);
                errorText.text = "";
            }
        }
    }
}
