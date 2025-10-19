using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    // Event that is triggered when this item is collected
    public delegate void ItemCollectedHandler();
    public static event ItemCollectedHandler OnItemCollected;

    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;
            OnItemCollected?.Invoke();
            Destroy(gameObject);
        }
    }
}