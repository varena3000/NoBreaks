using UnityEngine;
using UnityEngine.Events;

// Title: FIRST PERSON INTERACTION in Unity!
//Author: Kabungus
//Date: 19 August 2025
//Platform: Youtube
//Code version: Unknown
//Availability: https://youtu.be/b7Yf6BFx6js?si=nstQjibMHoaiIcXp


public class Interactable : MonoBehaviour
{
    //Outline outline;
    public string message;

    public UnityEvent onInteraction;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //outline = GetComponent<Outline>();
        //DisableOutline();
    }

    public void Interact()
    {
        onInteraction.Invoke();
    }

    public void DisableOutline()
    {
        //outline.enabled = false;
    }

    public void EnableOutline()
    {
        //outline.enabled = true;
    }

}
