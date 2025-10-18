using NUnit.Framework;
using TMPro;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{

    public AdvancedDialogueSO[] conversation;
    private int conversationIndex = 0;
    public int ConversationIndex
    {
        get => conversationIndex;
        set => conversationIndex = value;
    }

    private AdvanceDialogueManager advanceDialogueManager;
    private bool dialogueInitiated;
    public bool DialogueInitiated
    {
        get => dialogueInitiated;
        set => dialogueInitiated = value;
    }


    //Conditions
    private bool hasBattery = false;
    public bool HasBattery
    {
        get => hasBattery;
        set => hasBattery = value;
    }

    void Start()
    {
        advanceDialogueManager = GameObject.Find("DialogueManager").GetComponent<AdvanceDialogueManager>();
    }

    private void OnTriggerStay(Collider Playercollision)
    {
        if (Playercollision.gameObject.tag == "Player" && !dialogueInitiated)
        {
            advanceDialogueManager.InitiateDialogue(this, conversationIndex);

            if (hasBattery == true && conversationIndex < 2)
            {
                conversationIndex = 2;
            }


            dialogueInitiated = true;
        }
    }

    private void OnTriggerExit(Collider Playercollision)
    {
        if (Playercollision.gameObject.tag == "Player")
        {
            dialogueInitiated = false;
            advanceDialogueManager.DialogueActivated = false;
        }
    }

    #region Conditions for dialogue to continue

    private void OnTriggerEnter(Collider Batterycollision)
    {
        if(Batterycollision.gameObject.name == "Battery")
        {
            hasBattery = true;
        }
    }
    
    #endregion
}
