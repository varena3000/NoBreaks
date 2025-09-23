using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    // Event that is triggered when this item is collected
    public delegate void ItemCollectedHandler();
    public static event ItemCollectedHandler OnItemCollected;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player collided with the collectible
        if (other.CompareTag("Player"))
        {
            // Notify the collector system an item was collected
            OnItemCollected?.Invoke();

            // Disable or destroy the collected item
            // Here we destroy it, but disabling is an option too
            Destroy(gameObject);
        }
    }
}