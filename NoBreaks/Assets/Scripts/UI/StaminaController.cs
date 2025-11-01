using UnityEngine;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{
    [Header("Stamina Main Parameters")]
    [Range(0, 100)] public float playerStamina = 100f;
    [SerializeField] private float maxStamina = 100f;
    [HideInInspector] public bool hasRegenerated = true;
    [HideInInspector] public bool weAreSprinting = false;

    [Header("Stamina Regen Parameters")]
    [Range(0, 50)] [SerializeField] private float staminaDrain = 50f;
    [Range(0, 50)] [SerializeField] private float staminaRegen = 20f;

    [Header("Stamina Speed Parameters")]
    [SerializeField] private int slowedRunSpeed = 6;
    [SerializeField] private int normalRunSpeed = 10;

    [Header("Stamina UI Elements")]
    [SerializeField] private Image staminaProgressUI = null;
    [SerializeField] private CanvasGroup sliderCanvasGroup = null;

    private FPController playerController;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        playerController = GetComponent<FPController>();
    }

    private void Update()
    {
        // Regeneration logic
        if (!weAreSprinting)
        {
            if (playerStamina <= maxStamina - 0.01f)
            {
                playerStamina += staminaRegen * Time.deltaTime;
                UpdateStamina(1);

                if (playerStamina >= maxStamina)
                {
                    sliderCanvasGroup.alpha = 0;
                    playerController.SetSprintSpeed(normalRunSpeed);
                    hasRegenerated = true;
                }
            }

            // Stop sprint SFX when not sprinting
            if (audioManager.actionSource.isPlaying && audioManager.actionSource.clip == audioManager.Sprint)
                audioManager.StopActionSFX(audioManager.Sprint);
        }

        if (playerStamina < 0)
            MinStamina();
    }

    public void Sprinting()
    {
        if (hasRegenerated)
        {
            weAreSprinting = true;
            playerStamina -= staminaDrain * Time.deltaTime;
            UpdateStamina(1);

            if (playerStamina <= 0)
            {
                playerStamina = 0;
                sliderCanvasGroup.alpha = 0;
                weAreSprinting = false;
                playerController.SetSprintSpeed(slowedRunSpeed);

                // Stop sprint sound
                if (audioManager.actionSource.clip == audioManager.Sprint && audioManager.actionSource.isPlaying)
                    audioManager.actionSource.Stop();
            }
            else
            {
                // Set normal sprint speed
                playerController.SetSprintSpeed(normalRunSpeed);

                audioManager.StopSFX(audioManager.Walk);

                // Play sprint sound if not already playing
                if (audioManager.actionSource.clip != audioManager.Sprint)
                {
                    audioManager.actionSource.clip = audioManager.Sprint;
                    audioManager.actionSource.loop = true;
                    audioManager.actionSource.Play();
                }
                else if (!audioManager.actionSource.isPlaying)
                {
                    audioManager.actionSource.Play();
                }
            }
        }
    }

    void MinStamina()
    {
        if (playerStamina < 0)
            playerStamina = 0;
    }

    void UpdateStamina(int value)
    {
        staminaProgressUI.fillAmount = playerStamina / maxStamina;

        if (value == 0)
            sliderCanvasGroup.alpha = 0;
        else
            sliderCanvasGroup.alpha = 1;
    }

    private void OnDisable()
    {
        if (sliderCanvasGroup != null)
            sliderCanvasGroup.alpha = 0;

        // Stop any sprinting SFX when disabled
        if (audioManager != null)
            audioManager.StopSFX(audioManager.Sprint);
    }
}
