using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

// Title: How To Make An Interaction System In Unity (2025)
//Author: Tvtig
//Date: 19 September 2025
//Platform: Youtube
//Code version: Unknown
//Availability: https://youtu.be/Eg7oP7mcNbc?si=wVPhJoTB6LadzRRB

public class Interactor : MonoBehaviour
{
    [SerializeField]
    private float _castDistance = 5f;

    [SerializeField]
    private Vector3 _raycastOffset = new Vector3(0, 1f, 0);

    [SerializeField]
    private GameObject _interactionUI;

    private void Update()
    {
        if (DoInteractionTest(out IInteractable interactable))
        {
            if (interactable.CanInteract())
            {
                _interactionUI.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact(this);
                }
            }
            else
            {
                _interactionUI.SetActive(false);
            }
        }
        else
        {
            _interactionUI.SetActive(false);
        }
    }


    private bool DoInteractionTest(out IInteractable interactable)
    {
        interactable = null;

        Ray ray = new Ray(transform.position + _raycastOffset, transform.forward);

        Debug.DrawRay(ray.origin, ray.direction, Color.red, 3f);

        if(Physics.Raycast(ray, out RaycastHit hitInfo, _castDistance))
        {
            interactable = hitInfo.collider.GetComponent<IInteractable>();

            if(interactable != null)
            {
                return true;
            }

            return false;
        }

        return false;
    }

}
