using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float radius = 2f;
    [SerializeField] private LayerMask interactableLayers;
    [SerializeField] private InteractPrompt prompt;

    private Collider[] buffer = new Collider[32];
    private IInteractable focused;

    private void Update()
    {
        IInteractable nearest = FindNearestInteractablle();
        UpdateFocus(nearest);

        if(focused != null && Input.GetKeyDown(KeyCode.E))
        {
            if (focused.CanInteract()) focused.Interact();
        }
    }

    private IInteractable FindNearestInteractablle()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, radius, buffer, interactableLayers, QueryTriggerInteraction.Collide);
        IInteractable nearest = null;
        float bestDistSq = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            Collider col = buffer[i];
            if (col == null) continue;
            IInteractable interactablle = col.GetComponentInParent<IInteractable>();
            if (interactablle == null) continue;
            if (!interactablle.CanInteract()) continue;
            float distSq = (col.transform.position - transform.position).sqrMagnitude;
            if (distSq < bestDistSq)
            {
                bestDistSq = distSq;
                nearest = interactablle;
            }
        }
        return nearest;
    }

    private void UpdateFocus(IInteractable nearest)
    {
        if (ReferenceEquals(focused, nearest)) return;
        focused?.OnfocusLost();
        focused = nearest;
        if (focused != null)
        {
            focused.OnfocusGained();
            prompt.Show(focused);
        }
        else
        {
            prompt.Hide();
        }

    }
}
