using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 9f;
    public float crouchSpeed = 2.5f;
    public float jumpForce = 3f;
    public float gravity = -9.81f;

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
    public PickUpObject heldObject;


    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float verticalRotation = 0f;

    private bool isSprinting = false;
    private bool isCrouching = false;
    private bool hasCheckedPickUp = false;
    private bool isInteracting = false;

    public bool GetIsInteracting()
    {
        return isInteracting;
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Update()
    {
        HandleMovement();
        HandleLook();
        HandleCrouchTransition();

        if (heldObject != null)
        {
            heldObject.MoveToHoldPoint(holdPoint.position);
        }
    }

    #region Input Callbacks

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.ReadValueAsButton();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started) // Toggle crouch
            isCrouching = !isCrouching;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        isInteracting = context.performed;
    }

    #endregion

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
    }

    #region 

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
                        }
                    }
                }

                else
                {
                    heldObject.Drop();
                    heldObject = null;
                }
            }
        }
        else
        {
            hasCheckedPickUp = false;
        }
        
    }
    
    #endregion
}
