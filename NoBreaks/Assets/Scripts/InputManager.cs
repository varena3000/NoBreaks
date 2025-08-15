using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "player walk";

    [Header("Action Name References")]
    [SerializeField] private string Movement = "Movement";
    [SerializeField] private string Look = "Look";
    [SerializeField] private string Jump = "Jump";
    [SerializeField] private string Sprint = "Sprint";

    private InputAction MovementAction;
    private InputAction LookAction;
    private InputAction JumpAction;
    private InputAction SprintAction;

    public Vector2 MovementInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool SprintTriggered { get; private set; }

    private void Awake()
    {
        InputActionMap mapReference = playerControls.FindActionMap(actionMapName);

        MovementAction = mapReference.FindAction(Movement);
        LookAction = mapReference.FindAction(Look);
        JumpAction = mapReference.FindAction(Jump);
        SprintAction = mapReference.FindAction(Sprint);

        SubscribeActionValueToInputEvents();
    }

    public void SubscribeActionValueToInputEvents()
    {
        MovementAction.performed += inputInfo => MovementInput = inputInfo.ReadValue<Vector2>();
        MovementAction.canceled += inputInfo => MovementInput = Vector2.zero;

        LookAction.performed += inputInfo => LookInput = inputInfo.ReadValue<Vector2>();
        LookAction.canceled += inputInfo => LookInput = Vector2.zero;

        JumpAction.performed += inputInfo => JumpTriggered = true;
        JumpAction.canceled += inputInfo => JumpTriggered = false;

        SprintAction.performed -= inputInfo => SprintTriggered = true;
        SprintAction.canceled -= inputInfo => SprintTriggered = false;
    }

    private void OnEnable()
    {
        playerControls.FindActionMap(actionMapName).Enable();
    }

    private void OnDisable()
    {
        playerControls.FindActionMap(actionMapName).Disable();
    }
}
