using System;
using System.Collections;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AdvanceDialogueManager : MonoBehaviour
{
    //The NPC DIALOGUE we are currently stepping through
    private AdvancedDialogueSO currentConversation;
    private int stepNum;
    private bool dialogueActivated;

    //UI References
    public GameObject dialogueCanvas;
    private TMP_Text actor;
    private Image portrait;
    private TMP_Text dialogueText;

    private string currentSpeaker;
    private Sprite currentPortrait;

    public ActorSO[] actorSO;
    public AdvancedDialogueSO[] specificConversationsSO;

    //Typewriter effect
    [SerializeField]
    private float typingSpeed = 0.02f;
    private Coroutine typeWriterRoutine;
    private bool canContinueText = true;

    //Specific Conversations
    private bool hasQuestObject = false;
    private AdvancedDialogueSO specificConversations;

    /*
        HUD to Deactivate
        private GameObject HUDCanvas;
    */

    //Player Freeze
    private FPController playerMove;

    //Input Action
    private InputAction onInteract;
    //private PlayerInput playerInput;

    void Awake()
    {
        //Find button
        onInteract = InputSystem.actions.FindAction("Interact");
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*
            Access the HUD you want to turn off
            HUDCanvas = GameObject.Find("HUDCanvas");
        */

        //Find player FP Controller
        playerMove = GameObject.Find("Player").GetComponent<FPController>();

        dialogueCanvas = GameObject.Find("DialogueCanvas");
        actor = GameObject.Find("ActorText").GetComponent<TMP_Text>();
        portrait = GameObject.Find("Portrait").GetComponent<Image>();
        dialogueText = GameObject.Find("DialogueText").GetComponent<TMP_Text>();

        dialogueCanvas.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueActivated && onInteract.WasPerformedThisFrame() && canContinueText)
        {
            /*
                Deactivate HUD
                HUDCanvas.SetActive(false);
            */

            //Freeze player
            playerMove.enabled = false;

            //Cancel dialogue if there are no lines of dialogue remaining
            if (stepNum >= currentConversation.actors.Length)
                TurnOffDialogue();

            //Continue dialogue
            else
                PlayDialogue();
                
            /*Play specific dialogue
            if (hasQuestObject == true)
                PlaySpecificDialouge();
            */
        }
    }

    void PlayDialogue()
    {
        SetActorInfo();

        //Display Dialogue
        actor.text = currentSpeaker;
        portrait.sprite = currentPortrait;

        //Keep the routine from running multiple times at the same time.
        if (typeWriterRoutine != null)
            StopCoroutine(typeWriterRoutine);

        if (stepNum < currentConversation.Dialogue.Length)
            typeWriterRoutine = StartCoroutine(TypeWriterEffect(dialogueText.text = currentConversation.Dialogue[stepNum]));

        /*if has object == true play specific conversation
        if(stepNum < specificConversations.Dialogue.Length && hasQuestObject == true)
            typeWriterRoutine = StartCoroutine(TypeWriterEffect(dialogueText.text = specificConversations.Dialogue[stepNum]));
        */

        dialogueCanvas.SetActive(true);
        stepNum += 1;
    }

    //for when condition is met
    void PlaySpecificDialouge()
    {
        specificConversations = specificConversationsSO[0];
    }

    void SetActorInfo()
    {
        for (int i = 0; i < actorSO.Length; i++)
        {
            if (actorSO[i].name == currentConversation.actors[stepNum].ToString())
            {
                currentSpeaker = actorSO[i].actorName;
                currentPortrait = actorSO[i].actorPortrait;
            }
        }
    }

    private IEnumerator TypeWriterEffect(string line)
    {
        dialogueText.text = "";
        canContinueText = false;
        bool addingRichTextTag = false;
        yield return new WaitForSeconds(.5f);

        foreach (char letter in line.ToCharArray())
        {
            if (onInteract.WasPerformedThisFrame())
            {
                dialogueText.text = line;
                break;
            }

            //Check to see if we are working with rich text tags
            if (letter == '<' || addingRichTextTag)
            {
                addingRichTextTag = true;
                dialogueText.text += letter;

                if (letter == '>')
                    addingRichTextTag = false;
            }

            //If not using rich text tags
            else
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }

            canContinueText = true;
        }
    }

    public void InitiateDialogue(NPCDialogue npcDialogue)
    {
        //the array we are currently stepping through
        currentConversation = npcDialogue.conversation[0];

        dialogueActivated = true;
    }

    public void TurnOffDialogue()
    {
        stepNum = 0;

        dialogueActivated = false;
        dialogueCanvas.SetActive(false);

        /*
                Activate HUD
                HUDCanvas.SetActive(True);
        */

        //Unfreeze player
        playerMove.enabled = true;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "QuestObject")
        {
            hasQuestObject = true;
        }
        else
            hasQuestObject = false;
    }
}


public enum DialogueActors
{
    Jenni,
    Rat,
    Fairy,
};
