using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.LowLevelPhysics;

public class FPController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 9f;
    public float crouchSpeed = 2.5f;
    public float jumpForce = 3f;
    public float gravity = -9.8f;

    [Header("Look Settings")]
    public Transform cameraTransform;
    public float lookSensitivity = 2f;
    public float verticalLookLimit = 90f;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f;
    public float standingHeight = 2f;
    public float crouchTransitionSpeed = 8f;

    [Header("Pickup")]
    public float pickupRange = 3f;
    public Transform holdPoint;
    private PickUpObject heldObject;

    [Header("Imbue")]
    public Camera characterCamera;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float verticalRotation = 0f;

    private bool isSprinting = false;
    private bool isCrouching = false;
    private bool hasCheckedPickUp = false;
    private bool isWalking = false;
    private bool isInteracting = false;


    private bool isJumping = false;
    private bool isFalling = false;
    private bool isGrounded = false;

    private Animator animator;

    [HideInInspector]
    public StaminaController _staminaController;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        _staminaController = GetComponent<StaminaController>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Update()
    {
        if (!enabled) 
            return;
        
            HandleMovement();
            HandleFootsteps();
            HandleLook();
            HandleCrouchTransition();

        if (heldObject != null)
        {
            heldObject.MoveToHoldPoint(holdPoint.position);
        }

        //calling Stamina drain
        if (isSprinting && moveInput.magnitude > 0.1f)
            _staminaController.Sprinting();
        else
            _staminaController.weAreSprinting = false;

        //stop stamiba drain when !sprinting and crouching
        if (isCrouching)
        {
            _staminaController.weAreSprinting = false;
            _staminaController.hasRegenerated = false;
        }
        else
        {
            _staminaController.hasRegenerated = true;
        }

        //walking
        //isWalking = moveInput.magnitude > 0.1f && controller.isGrounded;
        // animator.SetBool("isWalking", isWalking);

        //Jumping
        if (!controller.isGrounded)
        {
            isFalling = true;
            //animator.SetBool("isFalling", true)
        }

        if (isFalling == true && controller.isGrounded)
        {
            isJumping = false;
            isGrounded = true;
            isFalling = false;

            audioManager.PlayActionSFX(audioManager.Landing);
            ////animator.SetBool("isGrounded", true)
        }
    }

    #region Input Callbacks

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        isWalking = true;

        if(isWalking == true)
        {
            //play walking animation
            //animator.SetBool("isWalking", true);
        }
        else if(!isWalking && controller.isGrounded && isCrouching == false)
        {
            //play idle animation
            //animator.SetBool("iswalking", false);
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started && moveInput.magnitude > 0.1f)
        {
            isSprinting = true;
            _staminaController.weAreSprinting = true;
        }
        else if (context.canceled)
        {
            isSprinting = false;
            _staminaController.weAreSprinting = false;
        }
        
        if (context.started && moveInput.magnitude > 0.1f && isGrounded == true)
        {
            isSprinting = true;
            //animator.SetBool("isSprinting", true);
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isCrouching = !isCrouching;
            audioManager.PlayActionSFX(audioManager.Crouch);

            if(isCrouching == true)
            {
                //Crouching down animation
                //animator.SetBool("isCrouching", true)
            }
            else
            {
                //crouching up animation
                //animator.SetBool("isCrouching", false)
            }
        }
            
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && controller.isGrounded)
        {
            isJumping = true;
            isGrounded = false;
            ////animator.SetBool("isGrounded", false)

            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            audioManager.PlayActionSFX(audioManager.Jump);

            //animator.SetBool("isJumping", true);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        isInteracting = context.performed;
    }

    public void OnPickUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!hasCheckedPickUp)
            {
                if (heldObject == null)
                {
                    hasCheckedPickUp = true;
                    Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
                    if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
                    {
                        PickUpObject pickUp = hit.collider.GetComponent<PickUpObject>();
                        if (pickUp != null)
                        {
                            pickUp.PickUp(holdPoint);
                            heldObject = pickUp;
                            audioManager.PlaySFX(audioManager.Interact);
                        }
                    }
                }

                else
                {
                    heldObject.Drop();
                    heldObject = null;
                    audioManager.PlaySFX(audioManager.Interact);
                }
            }
        }
        else
        {
            hasCheckedPickUp = false;
        }

    }
    #endregion
    
    #region Handle

    public void HandleMovement()
    {
        float currentSpeed = isCrouching ? crouchSpeed : (isSprinting ? sprintSpeed : walkSpeed);

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Gravity
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void HandleLook()
    {
        //Mouse
        float mouseX = lookInput.x * lookSensitivity;
        float mouseY = lookInput.y * lookSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

    }

    private void HandleCrouchTransition()
    {
        float targetHeight = isCrouching ? crouchHeight : standingHeight;

        if (controller.height != targetHeight)
        {
            controller.height = Mathf.Lerp(controller.height, targetHeight, crouchTransitionSpeed * Time.deltaTime);
        }

        if(controller.height == crouchHeight)
        {
            //play crouch up animation
        }
        else if(controller.height == standingHeight)
        {
            //play crouch down animation
        }
    }

    private void HandleFootsteps()
    {
        bool isMoving = moveInput.magnitude > 0.1f && controller.isGrounded;

        if (isMoving && !audioManager.sfxSource.isPlaying)
        {
            if (!isCrouching)
                audioManager.PlaySFX(audioManager.Walk);
        }

        if (!isMoving && audioManager.sfxSource.isPlaying)
        {
            audioManager.StopSFX(audioManager.Walk);
        }
    }


    public void SetSprintSpeed(float speed)
    {
        sprintSpeed = speed;
    }
    #endregion

}
