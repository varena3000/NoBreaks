using System;
using System.Collections;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AdvanceDialogueManager : MonoBehaviour
{
    // The NPC dialogue we are currently stepping through
    private AdvancedDialogueSO currentConversation;
    public int conversationID = 0;
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
    public AdvancedDialogueSO[] specificConversationsSO;

    [SerializeField] private float typingSpeed = 0.02f;
    private Coroutine typeWriterRoutine;
    private bool canContinueText = true;

    // Specific Conversations
    private bool hasQuestObject = false;
    private AdvancedDialogueSO specificConversations;

    // Player Freeze
    private FPController playerMove;

    // Input Action
    private InputAction onInteract;

    // Reference to the PlayerSwapManager
    private PlayerSwapManager swapManager;

    private void Awake()
    {
        // Find the "Interact" action
        onInteract = InputSystem.actions.FindAction("Interact");

        // Find PlayerSwapManager in scene
        swapManager = UnityEngine.Object.FindAnyObjectByType<PlayerSwapManager>();
        if (swapManager == null)
            Debug.LogError("AdvanceDialogueManager: PlayerSwapManager not found in scene!");
    }

    private void Start()
    {
        if (swapManager == null) return;

        // Get the active player from swap manager
        playerMove = swapManager.activeController;

        // Assign UI elements
        dialogueCanvas = GameObject.Find("DialogueCanvas");
        if (dialogueCanvas == null)
        {
            Debug.LogError("AdvanceDialogueManager: DialogueCanvas not found!");
            return;
        }

        actor = GameObject.Find("ActorText")?.GetComponent<TMP_Text>();
        portrait = GameObject.Find("Portrait")?.GetComponent<Image>();
        dialogueText = GameObject.Find("DialogueText")?.GetComponent<TMP_Text>();

        dialogueCanvas.SetActive(false);
    }

    private void Update()
    {
        if (swapManager == null || swapManager.activeController != swapManager.jenniController) return;

        IUpdateDialogue();
    }

    private void IUpdateDialogue()
    {
        if (dialogueActivated && onInteract != null && onInteract.WasPerformedThisFrame() && canContinueText)
        {
            // Freeze player movement
            if (playerMove != null)
                playerMove.enabled = false;

            // End dialogue if finished
            if (stepNum >= currentConversation.actors.Length)
            {
                TurnOffDialogue();
            }
            else
            {
                PlayDialogue();
            }
        }
    }

    private void PlayDialogue()
    {
        if (currentConversation == null) return;

        SetActorInfo();

        // Display dialogue
        if (actor != null) actor.text = currentSpeaker;
        if (portrait != null) portrait.sprite = currentPortrait;

        // Stop previous typewriter
        if (typeWriterRoutine != null)
            StopCoroutine(typeWriterRoutine);

        if (stepNum < currentConversation.Dialogue.Length && dialogueText != null)
        {
            typeWriterRoutine = StartCoroutine(TypeWriterEffect(currentConversation.Dialogue[stepNum]));
        }

        dialogueCanvas.SetActive(true);
        stepNum++;
    }

    private void SetActorInfo()
    {
        if (currentConversation == null) return;

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
        if (dialogueText == null) yield break;

        dialogueText.text = "";
        canContinueText = false;
        bool addingRichTextTag = false;
        yield return new WaitForSeconds(0.5f);

        foreach (char letter in line.ToCharArray())
        {
            if (onInteract != null && onInteract.WasPerformedThisFrame())
            {
                dialogueText.text = line;
                break;
            }

            if (letter == '<' || addingRichTextTag)
            {
                addingRichTextTag = true;
                dialogueText.text += letter;
                if (letter == '>')
                    addingRichTextTag = false;
            }
            else
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        canContinueText = true;
    }

    public void InitiateDialogue(NPCDialogue npcDialogue)
    {
        if (npcDialogue == null || swapManager == null) return;
        if (swapManager.activeController != swapManager.jenniController) return;

        currentConversation = npcDialogue.conversation[conversationID];
        dialogueActivated = true;
        stepNum = 0;
    }

    public void TurnOffDialogue()
    {
        stepNum = 0;
        dialogueActivated = false;

        if (dialogueCanvas != null)
            dialogueCanvas.SetActive(false);

        if (playerMove != null)
            playerMove.enabled = true;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (swapManager == null || swapManager.activeController != swapManager.jenniController) return;

        if (collision != null && collision.CompareTag("QuestObject"))
        {
            hasQuestObject = true;
            conversationID = 1;
            Debug.Log("Is working");
        }
    }
}

public enum DialogueActors
{
    Jenni,
    Rat,
    Mechanic,
}
