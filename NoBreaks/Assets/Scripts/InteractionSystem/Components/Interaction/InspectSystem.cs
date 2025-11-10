using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Cursor = UnityEngine.Cursor;

public class InspectSystem : MonoBehaviour
{

    [SerializeField] private TMP_Text infoText;

    [SerializeField] private Canvas inspectableCanvas;
    [SerializeField] private GameObject offset;

    [SerializeField]
    private float rotationSensitivity = 100f;

    [SerializeField] private InputActionAsset input;

    private SceneDialogueTrigger hud;

    private PlayerSwapManager activePlayer;
    private Interactable interactable;
    private InteractPrompt prompt;
    private GameObject targetObject;
    private Transform examinedObject;

    private InputAction onInteract;
    private InputAction lookAction;

    private bool isExamining = false;

    AudioManager audioManager;

    private Dictionary<Transform, Vector3> originalPositions = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, Quaternion> originalRotations = new Dictionary<Transform, Quaternion>();
    private Dictionary<Transform, Rigidbody> originalRigidbodies = new Dictionary<Transform, Rigidbody>();

    void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        hud = FindAnyObjectByType<SceneDialogueTrigger>();

        if (input == null)
        {
            return;
        }

        var map = input.FindActionMap("OnFoot", true);
        if (map == null)
        {
            return;
        }
        onInteract = map.FindAction("Interact", true);
        lookAction = map.FindAction("Look", true);

        inspectableCanvas.enabled = false;
        activePlayer = FindAnyObjectByType<PlayerSwapManager>();
        interactable = FindAnyObjectByType<Interactable>();
        prompt = FindAnyObjectByType<InteractPrompt>();

        targetObject = GameObject.Find("PlayerRat Variant");
    }

    void OnEnable()
    {
        onInteract?.Enable();
        lookAction?.Enable();
    }

    void OnDisable()
    {
        onInteract?.Disable();
        lookAction?.Disable();
    }

    void Update()
    {
        if (onInteract != null && onInteract.WasPressedThisFrame())
        {
            HandleInteraction();
        }

        if (isExamining)
        {
            RotateExaminedObject();
        }
    }

    void HandleInteraction()
    {
        if (!isExamining)
        {
            // Start examining if we hit an inspectable object
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Inspectable"))
                {
                    examinedObject = hit.transform;

                    // Store original transform
                    originalPositions[examinedObject] = examinedObject.position;
                    originalRotations[examinedObject] = examinedObject.rotation;

                    // Handle Rigidbody if present
                    Rigidbody rb = examinedObject.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        originalRigidbodies[examinedObject] = rb;
                        rb.isKinematic = true;
                    }

                    // Get description from the InspectableObject component
                    var inspectable = hit.collider.GetComponent<InspectText>();
                    if (inspectable != null)
                    {
                        infoText.text = inspectable.description;
                    }
                    else
                    {
                        infoText.text = "";
                    }

                    StartExamination();
                }
            }
        }
        else
        {
            StopExamination();
        }
    }

    void StartExamination()
    {
        isExamining = true;
        inspectableCanvas.enabled = true;
        hud.HudOff();
        audioManager.PlaySFX(audioManager.Interact);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (activePlayer != null)
            activePlayer.activeController.enabled = false;

        // Move object to offset and parent it
        examinedObject.SetParent(offset.transform, true);
        examinedObject.position = offset.transform.position;
        examinedObject.rotation = offset.transform.rotation;
    }

    void StopExamination()
    {
        isExamining = false;
        inspectableCanvas.enabled = false;
        hud.HudOn();
        audioManager.PlaySFX(audioManager.Interact);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (activePlayer != null)
            activePlayer.activeController.enabled = true;

        interactable.OnfocusGained();
        prompt.Show(interactable);   

        if (examinedObject != null)
        {
            // Restore transform and parent
            examinedObject.SetParent(null, true);
            examinedObject.position = originalPositions[examinedObject];
            examinedObject.rotation = originalRotations[examinedObject];

            // Restore Rigidbody state
            if (originalRigidbodies.TryGetValue(examinedObject, out Rigidbody rb))
            {
                rb.isKinematic = false;
            }

            examinedObject = null;
        }
    }

    void RotateExaminedObject()
    {
        if (examinedObject == null)
            return;

        // Smoothly move to offset
        examinedObject.position = Vector3.Lerp(examinedObject.position, offset.transform.position, 0.2f);

        Vector2 lookDelta = lookAction.ReadValue<Vector2>();
        float rotationSpeed = rotationSensitivity * Time.deltaTime;

        examinedObject.Rotate(Vector3.up, -lookDelta.x * rotationSpeed, Space.Self);
        examinedObject.Rotate(Vector3.right, lookDelta.y * rotationSpeed, Space.Self);
    }
}
