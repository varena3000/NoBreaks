using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    
    public AdvancedDialogueSO[] conversation;

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
            advanceDialogueManager.InitiateDialogue(this);
            dialogueInitiated = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            advanceDialogueManager.TurnOffDialogue();
            dialogueInitiated = false;
        }
    }
}
