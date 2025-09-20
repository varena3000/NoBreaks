using TMPro;
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

    //Input Action
    private InputAction onInteract;
     //private PlayerInput playerInput;

    void Awake()
    {
        //playerInput = GetComponent<PlayerInput>();
        onInteract = InputSystem.actions.FindAction("Interact");
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogueCanvas = GameObject.Find("DialogueCanvas");
        actor = GameObject.Find("ActorText").GetComponent<TMP_Text>();
        portrait = GameObject.Find("Portrait").GetComponent<Image>();
        dialogueText = GameObject.Find("DialogueText").GetComponent<TMP_Text>();

        dialogueCanvas.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueActivated && onInteract.WasPerformedThisFrame())
        {
            //Cancel dialogue if there are no lines of dialogue remaining
            if (stepNum >= currentConversation.actors.Length)
                TurnOffDialogue();

            //Continue dialogue
            else
                PlayDialogue();
        }
    }

    void PlayDialogue()
    {
        SetActorInfo();

        //Display Dialogue
        actor.text = currentSpeaker;
        portrait.sprite = currentPortrait;

        dialogueText.text = currentConversation.Dialogue[stepNum];
        dialogueCanvas.SetActive(true);
        stepNum += 1;
    }
    
    void SetActorInfo()
    {
            for (int i = 0; i < actorSO.Length; i++)
            {
                if(actorSO[i].name == currentConversation.actors[stepNum].ToString())
                {
                    currentSpeaker = actorSO[i].actorName;
                    currentPortrait = actorSO[i].actorPortrait;
                }
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
    }
}

public enum DialogueActors
{
    Jenni,
    Rat,
    Fairy,
};
