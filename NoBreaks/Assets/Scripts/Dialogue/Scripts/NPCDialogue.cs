using UnityEngine;

public class NPCDialogue : MonoBehaviour
{

    public AdvancedDialogueSO[] conversation;
    private int conversationIndex = 0;

    private AdvanceDialogueManager advanceDialogueManager;
    private bool dialogueInitiated;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        advanceDialogueManager = GameObject.Find("DialogueManager").GetComponent<AdvanceDialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && !dialogueInitiated)
        {
            advanceDialogueManager.InitiateDialogue(this, conversationIndex);
            dialogueInitiated = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            advanceDialogueManager.TurnOffDialogue();
            conversationIndex++;

            if (conversationIndex >= conversation.Length)
                conversationIndex = 0;

            dialogueInitiated = false;
        }
    }
}
