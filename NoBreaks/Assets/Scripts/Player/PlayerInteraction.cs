using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float playerReach = 3f;
    Interactable currentInteractable;
    private Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void CheckInteraction()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        //if colliders with anything within Player reach
        if (Physics.Raycast(ray, out hit, playerReach))
        {
            if (hit.collider.tag == "Interactable") //if looking at an interactable object
            {
                Interactable newInteractable = hit.collider.GetComponent<Interactable>();

                //if there is a currentInteractable and it is not the newInteractable
                if (currentInteractable && newInteractable != currentInteractable)
                {
                    SetNewCurrentInteractable(newInteractable);
                }

                if (newInteractable.enabled)
                {
                    SetNewCurrentInteractable(newInteractable);
                }

                else // if new interactable is not enabled
                {
                    DisableCurrentInteractable();
                }
            }

            else // if not an interactable
            {
                DisableCurrentInteractable();
            }
        }

        else // if nothing in reach
        {
            DisableCurrentInteractable();
        }
    }

    void SetNewCurrentInteractable(Interactable newInteractable)
    {
        currentInteractable = newInteractable;
        currentInteractable.EnableOutline();
        HUDController.instance.EnableInteractionText(currentInteractable.message);
    }

    void DisableCurrentInteractable()
    {
        HUDController.instance.DisableInteractionText();
        if (currentInteractable)
        {
            currentInteractable.DisableOutline();
            currentInteractable = null;
        }
    }

    public void Interaction(Transform holdPoint)
    {
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
    }

    public void Drop()
    {
        rb.useGravity = true;
        transform.SetParent(null);
    }



    public void MoveToHoldPoint(Vector3 targetPostion)
    {
        rb.MovePosition(targetPostion);
    }

    #region Callback

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (heldObject == null)
        {
            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
            {
                PlayerInteraction interaction = hit.collider.GetComponent<PlayerInteraction>();
                if (interaction != null)
                {
                    interaction.Interaction(holdPoint);
                    heldObject = interaction;
                }
            }
        }

        else
        {
            heldObject.Drop();
            heldObject = null;
        }
    }
    #endregion

    // Update is called once per frame
    private void Update()
    {
        HandleMovement();
        HandleLook();

        if (heldObject != null)
        {
            heldObject.MoveToHoldPoint(holdPoint.position);
        }
    }

}
