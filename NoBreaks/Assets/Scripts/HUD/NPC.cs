using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using NUnit.Framework;
using UnityEditor.MPE;

// Title: UNITY 2D NPC DIALOGUE SYSTEM TUTORIAL
//Author: diving_squid
//Date: 22 August 2025
//Platform: Youtube
//Code version: Unknown
//Availability: https://youtu.be/1nFNOyCalzo?si=gx8LNragL5Rp4cGJ


public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    private string[] dialogue;
    private int index;

    private FPController fPController;
    private Coroutine typingMethod;
    public string[] dialogueComplete;
    public string[] dialogueincomplete;

    public float wordSpeed;
    public bool playerIsClose;
    private bool isInteracting;
    private bool doneTyping = false;
    private bool checkedTyping;
    private bool hasObject;

    // Update is called once per frame
    void Update()
    {
        if (hasObject)
        {
            dialogue = dialogueComplete;
        }
        else
        {
            dialogue = dialogueincomplete;
        }

        if (doneTyping && isInteracting)
        {
            doneTyping = false;
            NextLine();
        }

        if (playerIsClose)
        {
            isInteracting = fPController.GetIsInteracting();
        }

        OnInteract();
    }

    public void OnInteract()
    {
        if (isInteracting && playerIsClose)
        {
            if (!checkedTyping)
            {
                checkedTyping = true;
                if (!dialoguePanel.activeInHierarchy)
                {
                    dialoguePanel.SetActive(true);
                    typingMethod = StartCoroutine(Typing());
                }
            }
        }
        else
        {
            checkedTyping = false;
        }
    }

    IEnumerator Typing()
    {
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }

        doneTyping = true;
    }

    public void NextLine()
    {

        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            zeroText();
        }
    }

    public void zeroText()
    {
        StopAllCoroutines();

        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<FPController>() != null)
        {
            playerIsClose = true;
            fPController = other.gameObject.GetComponent<FPController>();
        }
        if (other.gameObject.name == "Battery")
        {
            hasObject = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<FPController>() != null)
        {

            zeroText();

            playerIsClose = false;
            fPController = null;
            isInteracting = false;
        }
        if (other.gameObject.tag == "Key")
        {
            hasObject = false;
        }
    }
}
