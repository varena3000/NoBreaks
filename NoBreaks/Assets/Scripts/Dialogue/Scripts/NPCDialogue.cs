using NUnit.Framework;
using TMPro;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{

    public AdvancedDialogueSO[] conversation;
    public int conversationIndex = 0;

    private AdvanceDialogueManager advanceDialogueManager;
    public bool dialogueInitiated;

     //Conditions
    public bool hasBattery = false;

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
