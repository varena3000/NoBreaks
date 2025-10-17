using System;
using System.Collections;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AdvanceDialogueManager : MonoBehaviour
{
    // The NPC dialogue we are currently stepping through.
    private AdvancedDialogueSO currentConversation;
    private int stepNum;
    private bool dialogueActivated;

    // UI References
    public GameObject dialogueCanvas;
    private TMP_Text actor;
    private Image portrait;
    private TMP_Text dialogueText;

    private string currentSpeaker;
    private Sprite currentPortrait;

    public ActorSO[] actorSO;

    [SerializeField]
    private float typingSpeed = 0.02f;
    
    private Coroutine typeWriterRoutine;
    private bool canContinueText = true;

    // Player Freeze
    private FPController playerMove;

    // Input Action
    private InputAction onInteract;

    // Reference to the PlayerSwapManager
    private PlayerSwapManager swapManager;
    private FPController activePlayer;

    private NPCDialogue npc;
    private GameObject Battery;

    private void Awake()
    {
        // Find the "Interact" action
        onInteract = InputSystem.actions.FindAction("Interact");

        // Find PlayerSwapManager in scene
        swapManager = UnityEngine.Object.FindAnyObjectByType<PlayerSwapManager>();
    }

    private void Start()
    {
        Battery = GameObject.Find("Battery");
        npc = FindAnyObjectByType<NPCDialogue>();

        // Get the active player from swap manager
        activePlayer = swapManager.activeController;

        //Find player FP Controller
        playerMove = swapManager.activeController.GetComponent<FPController>();

        dialogueCanvas = GameObject.Find("DialogueCanvas");
        actor = GameObject.Find("ActorText")?.GetComponent<TMP_Text>();
        portrait = GameObject.Find("Portrait")?.GetComponent<Image>();
        dialogueText = GameObject.Find("DialogueText")?.GetComponent<TMP_Text>();

        dialogueCanvas.SetActive(false);

        if (swapManager == null) return;
    }

    private void Update()
    {
        if (dialogueActivated && onInteract.WasPerformedThisFrame() && canContinueText)
        {

            //Freeze player
            playerMove.enabled = false;

            //Cancel dialogue if there are no lines of dialogue remaining
            if (stepNum >= currentConversation.actors.Length)
            {
                TurnOffDialogue();
                npc.dialogueInitiated = false;
            }

            //Continue dialogue
            else
                PlayDialogue();
        }
        


        if (swapManager == null || swapManager.activeController != swapManager.jenniController) return;
    }

    private void PlayDialogue()
    {

        SetActorInfo();

        //Display Dialogue
        actor.text = currentSpeaker;
        portrait.sprite = currentPortrait;

        ///Keep the routine from running multiple times at the same time.
        if (typeWriterRoutine != null)
            StopCoroutine(typeWriterRoutine);

        if (stepNum < currentConversation.Dialogue.Length)
            typeWriterRoutine = StartCoroutine(TypeWriterEffect(dialogueText.text = currentConversation.Dialogue[stepNum]));

        dialogueCanvas.SetActive(true);
        stepNum += 1;

        if (npc.conversationIndex == 2 && npc.hasBattery == true)
            Destroy(Battery);

    }

    private void SetActorInfo()
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

    public void InitiateDialogue(NPCDialogue npcDialogue, int convoIndex)
    {
        if (npcDialogue == null || swapManager == null) 
            return;
        if (swapManager.activeController != swapManager.jenniController) 
            return;

        if (convoIndex >= 0 && convoIndex < npcDialogue.conversation.Length)
            currentConversation = npcDialogue.conversation[convoIndex];

        else
            currentConversation = npcDialogue.conversation[0];
        
        dialogueActivated = true;

        playerMove = swapManager.activeController.GetComponent<FPController>();
    }

    public void TurnOffDialogue()
    {
        stepNum = 0;
        dialogueActivated = false;
        dialogueCanvas.SetActive(false);
        npc.conversationIndex++;

        //Unfreeze player
        playerMove.enabled = true;

        // Dialogue conditions
        if (npc.conversationIndex == 2 && npc.hasBattery == false)
        {
            npc.conversationIndex = 0;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (swapManager == null || swapManager.activeController != swapManager.jenniController) return;
    }

    #region Conditions for dialogue to continue

    
    
    #endregion
}

public enum DialogueActors
{
    Jenni,
    Rat,
    Mechanic,
}
