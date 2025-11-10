using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class openDrawers : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;
    private bool playerInRange = false;

    public InputActionAsset input;
    private InputAction Open;


    void Start()
    {
        animator = GetComponent<Animator>();

        var map = input.FindActionMap("OnFoot", true);
        Open = map.FindAction("Open", true);

        Open.performed += OnOpen;
        Open.Enable();
    }

      private void OnDestroy()
    {
        Open.performed -= OnOpen;
    }

    private void OnTriggerEnter(Collider other)
    {
        playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        playerInRange = false;
    }

    private void OnOpen(InputAction.CallbackContext context)
    {
        if (playerInRange)
        {
            isOpen = !isOpen; // Toggle state
            animator.SetBool("isOpen", isOpen);
        }
    }
}
