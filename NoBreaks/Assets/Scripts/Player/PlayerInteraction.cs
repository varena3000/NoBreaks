using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float playerReach = 3f;
    Interactable currentInteractable;

    // Update is called once per frame
    void Update()
    {

    }
    
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed  && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
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
}
