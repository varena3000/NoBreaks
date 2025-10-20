using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Events;

public class CollectibleManager : MonoBehaviour
{
    [Tooltip("Number of items needed to trigger the event")]
    public int itemsNeededToTrigger = 5;

    private int remainingItems;
    public TMP_Text collectedText;

    [Header("SoundEffects")]
    public AudioSource audioSource;
    public AudioClip[] collectSounds;
    private int currentSoundIndex = 0;

    [Tooltip("Configure events to trigger once required items collected")]
    public UnityEvent OnRequiredItemsCollected;

    private void Start()
    {
        PlayNextSound();
        remainingItems = itemsNeededToTrigger;
        UpdateCounterUI();
    }

    private void OnEnable()
    {
        CollectibleItem.OnItemCollected += HandleItemCollected;
    }

    private void OnDisable()
    {
        CollectibleItem.OnItemCollected -= HandleItemCollected;
    }

    private void HandleItemCollected()
    {
        if (remainingItems <= 0)
            return;

        remainingItems--;
        Debug.Log($"Remaining items: {remainingItems}/{itemsNeededToTrigger}");

        PlayNextSound();
        UpdateCounterUI();

        if (remainingItems <= 0)
        {
            Debug.Log("Collected required number of items!");
            OnRequiredItemsCollected?.Invoke();
        }
    }

    private void PlayNextSound()
    {
        if(audioSource != null && collectSounds.Length > 0)
        {
            audioSource.PlayOneShot(collectSounds[currentSoundIndex]);

            currentSoundIndex = (currentSoundIndex + 1) % collectSounds.Length;
        }
    }

    private void UpdateCounterUI()
    {
        if (collectedText != null)
        {
            collectedText.text = $"Remaining: {remainingItems}";
        }
    }
}
