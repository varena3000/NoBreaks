using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float radius = 2f;
    [SerializeField] private LayerMask interactableLayers;
    [SerializeField] private InteractPrompt prompt;

    private Collider[] buffer = new Collider[32];
    private IInteractable focused;

    private ImbueStation imbue;

    //PlayerInputs
    [SerializeField]
    private InputActionAsset input;
    private InputAction onInteract;
    private InputAction onImbue;

    void Awake()
    {
        imbue = FindAnyObjectByType<ImbueStation>();

        var map = input.FindActionMap("OnFoot", true);
        onInteract = map.FindAction("Interact", true);
        onImbue = map.FindAction("Imbue", true);
    }

    private void Update()
    {
        IInteractable nearest = FindNearestInteractable();
        UpdateFocus(nearest);

        #region Inputs for interactions

        if (focused != null && onInteract.IsPressed())
        {
            if (focused.CanInteract())
                focused.Interact();
        }
        if (focused != null && focused is MonoBehaviour mb && mb.gameObject.name == "Imbue" && onImbue.triggered)
        {
            if (mb.TryGetComponent<ImbueStation>(out var station))
            {
                station.HandleImbue();
            }
        }
        
        #endregion
    }

    private IInteractable FindNearestInteractable()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, radius, buffer, interactableLayers, QueryTriggerInteraction.Collide);
        IInteractable nearest = null;
        float bestDistSq = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            Collider col = buffer[i];
            if (col == null) continue;
            IInteractable interactable = col.GetComponentInParent<IInteractable>();
            if (interactable == null) continue;
            if (!interactable.CanInteract()) continue;
            float distSq = (col.transform.position - transform.position).sqrMagnitude;
            if (distSq < bestDistSq)
            {
                bestDistSq = distSq;
                nearest = interactable;
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
