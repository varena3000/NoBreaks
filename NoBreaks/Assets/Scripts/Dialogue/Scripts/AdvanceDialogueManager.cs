using System;
using System.Collections;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdvanceDialogueManager : MonoBehaviour
{
    // The NPC dialogue we are currently stepping through.
    private AdvancedDialogueSO currentConversation;
    private int stepNum;
    private bool dialogueActivated;
    public bool DialogueActivated
    {
        get => dialogueActivated;
        set => dialogueActivated = value;
    }
    
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

    // Input Action
    private InputAction onInteract;

    // Reference to the PlayerSwapManager and player freeze
    private PlayerSwapManager swapManager;

    private NPCDialogue npc;
    private GameObject Battery;

    public GameObject audioSource;
    private npcFocus npcFocus;

    private void Awake()
    {
       
        // Find the "Interact" action
        onInteract = InputSystem.actions.FindAction("Interact");

        actor = GameObject.Find("ActorText")?.GetComponent<TMP_Text>();
        portrait = GameObject.Find("Portrait")?.GetComponent<Image>();
        dialogueText = GameObject.Find("DialogueText")?.GetComponent<TMP_Text>();
        
        // Find PlayerSwapManager in scene
        swapManager = UnityEngine.Object.FindAnyObjectByType<PlayerSwapManager>();
    }

    private void Start()
    {
        npcFocus = FindAnyObjectByType<npcFocus>();
        Battery = GameObject.Find("Battery");
        npc = FindAnyObjectByType<NPCDialogue>();

        if (dialogueCanvas != null)
            dialogueCanvas.SetActive(false);
    }

    private void Update()
    {
        if (dialogueActivated && onInteract.WasPerformedThisFrame() && canContinueText)
        {
            //Freeze player
            swapManager.activeController.enabled = false;

            //Cancel dialogue if there are no lines of dialogue remaining
            if (stepNum >= currentConversation.actors.Length)
            {
                TurnOffDialogue();
                npc.DialogueInitiated = false;
            }

            //Continue dialogue
            else
                PlayDialogue();
            
        }
    }

    public void PlayDialogue()
    {
        if (swapManager.jenniStamina != null)
        {
            swapManager.jenniStamina.enabled = false;
            swapManager.jenniStamina.playerStamina = 100.0f;
        }
            
        
        SetActorInfo();

        //Display Dialogue
        actor.text = currentSpeaker;
        portrait.sprite = currentPortrait;

        if((SceneManager.GetActiveScene().name == "2. MortalEngines"))
        npcFocus.FocusOnNPC();

        ///Keep the routine from running multiple times at the same time.
        if (typeWriterRoutine != null)
            StopCoroutine(typeWriterRoutine);

        if (stepNum < currentConversation.Dialogue.Length)
            typeWriterRoutine = StartCoroutine(TypeWriterEffect(dialogueText.text = currentConversation.Dialogue[stepNum]));

        dialogueCanvas.SetActive(true);
        stepNum += 1;

        if (npc.ConversationIndex == 2 && npc.HasBattery == true)
        {
            if(Battery != null)
            {
                Battery.SetActive(false);
                Battery = null;
            }
        }
        
        audioSource.SetActive(false);
    }

    private void SetActorInfo()
    {

        if (currentConversation == null || currentConversation.actors == null || stepNum >= currentConversation.actors.Length)
            return;
        
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
        bool skip = false;

        yield return new WaitForSeconds(.5f);

        foreach (char letter in line.ToCharArray())
        {
            if (!skip && onInteract.WasPerformedThisFrame())
            {
                skip = true;
            }

            if (skip)
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

        }

        yield return new WaitForSeconds(1f);  
        canContinueText = true;
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
    }
    
    public void InitiateDialogueWithoutPlayer(AdvancedDialogueSO dialogue)
    {
        if (dialogue == null)
        {
            dialogueActivated = false;
            return;
        }
            
        
        currentConversation = dialogue;
        stepNum = 0;
        dialogueActivated = true;

        if (dialogueCanvas == null)
            dialogueCanvas.SetActive(true);

        // Ensure player is frozen
        if (swapManager != null && swapManager.activeController != null)
        {
            swapManager.activeController.enabled = false;
        }

        PlayDialogue();

        if (stepNum >= currentConversation.actors.Length)
            {
                stepNum = 0;
                dialogueActivated = false;
                dialogueCanvas.SetActive(false);
                dialogueActivated = false;

                swapManager.activeController.enabled = true;
            }
    }

    public void TurnOffDialogue()
    {
        if (swapManager.jenniStamina.enabled == false)
            swapManager.jenniStamina.enabled = true;

        stepNum = 0;
        dialogueActivated = false;
        dialogueCanvas.SetActive(false);

        //Unfreeze player
        swapManager.activeController.enabled = true;

        // Dialogue conditions
        if (npc.ConversationIndex == 0 && npc.HasBattery == false)
        {
            npc.ConversationIndex = 0;
        }
        else
            npc.ConversationIndex++;

        audioSource.SetActive(true);
        
    }

    #region Conditions for dialogue to continue

    
    
    #endregion
}

public enum DialogueActors
{
    Jenni,
    Mochi,
    Mechanic,
    Yota,
    NewsReporter
}
