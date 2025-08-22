using UnityEngine;
using TMPro;

// Title: FIRST PERSON INTERACTION in Unity!
//Author: Kabungus
//Date: 19 August 2025
//Platform: Youtube
//Code version: Unknown
//Availability: https://youtu.be/b7Yf6BFx6js?si=nstQjibMHoaiIcXp



public class HUDController : MonoBehaviour
{
    public static HUDController instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] TMP_Text interactionText;

    public void EnableInteractionText(string text)
    {
        interactionText.text = text + "";
        interactionText.gameObject.SetActive(true);
    }

    public void DisableInteractionText()
    {
        interactionText.gameObject.SetActive(false);
    }

}
