using UnityEngine;

// Easy & Modular INTERACTION SYSTEM in Unity (With Outline & Text Prompt)
//Author: The Code Otter
//Date: 19 September 2025
//Platform: Youtube
//Code version: Unknown
//Availability: https://youtu.be/FE0lJljavAM?si=738fBWnjFGj6Xya7

public interface IInteractable
{
    Transform transform { get; }
    string DisplayName { get; }
    string KeyHint { get; }

    bool CanInteract();
    void Interact();
    void OnfocusGained();
    void OnfocusLost();
}
