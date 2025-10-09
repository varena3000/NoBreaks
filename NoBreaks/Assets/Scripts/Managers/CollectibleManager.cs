using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CollectibleManager : MonoBehaviour
{
    [Tooltip("Number of items needed to trigger the event")]
    public int itemsNeededToTrigger = 5;

    private int collectedItemCount = 0;
    public TMP_Text collectedText;

    // UnityEvent to be invoked when required number of items collected.
    [Tooltip("Configure events to trigger once required items collected")]
    public UnityEvent OnRequiredItemsCollected;

    private void OnEnable()
    {
        CollectibleItem.OnItemCollected += HandleItemCollected;
    }

    private void OnDisable()
    {
        CollectibleItem.OnItemCollected -= HandleItemCollected;
        //Counter
        collectedText.text = "Collected " + collectedItemCount;
    }

    private void HandleItemCollected()
    {
        collectedItemCount += 1;
        Debug.Log($"Collected {collectedItemCount}/{itemsNeededToTrigger} items.");

        if (collectedItemCount >= itemsNeededToTrigger)
        {
            Debug.Log("Collected required number of items!");
            OnRequiredItemsCollected?.Invoke();

            // Optional: Reset count if you want to trigger the event multiple times
            // collectedItemCount = 0;
        }
    }
}