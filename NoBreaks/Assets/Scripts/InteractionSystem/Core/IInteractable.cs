// Title: How To Make An Interaction System In Unity (2025)
//Author: Tvtig
//Date: 19 September 2025
//Platform: Youtube
//Code version: Unknown
//Availability: https://youtu.be/Eg7oP7mcNbc?si=wVPhJoTB6LadzRRB

public interface IInteractable
{
    public bool CanInteract();
    public bool Interact(Interactor interactor);
}
