using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{
    [Header("Stamina Main Parameters")]
    public float playerStamina = 100.0f;
    [SerializeField]
    private float maxStamina = 100.0f;
    [HideInInspector]
    public bool hasRegenerated = true;
    [HideInInspector]
    public bool weAreSprinting = false;

    [Header("Stamina Regen Parameters")]
    [Range(0, 50)] [SerializeField] private float staminaDrain = 0.5f;
    [Range(0, 50)] [SerializeField] private float staminaRegen = 0.5f;

    [Header("Stamina Speed Parameters")]
    [SerializeField]
    private int slowedRunSpeed = 4;
    [SerializeField]
    private int normalRunSpeed = 8;

    [Header("Stamina UI Elements")]
    [SerializeField]
    private Image staminaProgressUI = null;
    [SerializeField]
    private CanvasGroup sliderCanvasGroup = null;

    private FPController playerController;

    private void Start()
    {
        playerController = GetComponent<FPController>();
    }

    private void Update()
    {
        if (!weAreSprinting)
        {
            if (playerStamina <= maxStamina - 0.01f)
            {
                playerStamina += staminaRegen * Time.deltaTime;
                UpdateStamina(1);

                if (playerStamina >= maxStamina)
                {
                    playerController.SetSprintSpeed(normalRunSpeed);
                    sliderCanvasGroup.alpha = 0;
                    hasRegenerated = true;
                }
            }
        }
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
                weAreSprinting = false;
                playerController.SetSprintSpeed(slowedRunSpeed);
                sliderCanvasGroup.alpha = 0;
            }
            else 
            {
                weAreSprinting = true;
                playerController.SetSprintSpeed(normalRunSpeed);
            }
        }  
    }
    
    void UpdateStamina(int value)
    {
        staminaProgressUI.fillAmount = playerStamina / maxStamina;

        if (value == 0)
            sliderCanvasGroup.alpha = 0;
        else
            sliderCanvasGroup.alpha = 1;
    }

}
