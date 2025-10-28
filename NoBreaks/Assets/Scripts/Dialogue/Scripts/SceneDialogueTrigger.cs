using System.Collections;
using Mono.Cecil.Cil;
using UnityEngine;

public class SceneDialogueTrigger : MonoBehaviour
{
    public AdvancedDialogueSO conversation;
    private AdvanceDialogueManager dialogueManager;
    private bool dialoguePlayed = false;

    [SerializeField]
    private TemporaryTMP tmp1;
    [SerializeField]
    private TemporaryTMP tmp2;
    [SerializeField]
    private TemporaryTMP tmp3;

    [SerializeField]
    private GameObject Yota;
    [SerializeField]
    private GameObject itemCanvas;
    [SerializeField]
    private GameObject Hud;
    [SerializeField]
    private GameObject playerIcon;
    [SerializeField]
    private GameObject instructionText;

    private bool imageOn = false;

    private void Awake()
    {
        tmp1.enabled = false;
        tmp2.enabled = false;
        tmp3.enabled = false;
    }

    private void Start()
    {
        dialogueManager = GameObject.Find("DialogueManager").GetComponent<AdvanceDialogueManager>();

        if (dialogueManager != null && !dialoguePlayed)
        {
             StartCoroutine(StartDialogueNextFrame());
        }
    }
    
    private IEnumerator StartDialogueNextFrame()
    {
        yield return null;

        if (Yota != null)
        {
            Yota.SetActive(true);
            itemCanvas.SetActive(false);
            Hud.SetActive(false);
            playerIcon.SetActive(false);
            instructionText.SetActive(false);

            imageOn = true;
        }

        dialogueManager.InitiateDialogueWithoutPlayer(conversation);
        dialoguePlayed = true;

         yield return new WaitUntil(() => !dialogueManager.DialogueActivated);

        if (imageOn && Yota != null)
        {
            Yota.SetActive(false);
            itemCanvas.SetActive(true);
            Hud.SetActive(true);
            playerIcon.SetActive(true);
            instructionText.SetActive(true);

            tmp1.enabled = true;
            tmp2.enabled = true;
            tmp3.enabled = true;

            imageOn = false;
        }
    }
}
