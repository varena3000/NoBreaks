using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class SceneDialogueTrigger : MonoBehaviour
{
    public AdvancedDialogueSO conversation;
    private AdvanceDialogueManager dialogueManager;
    private bool dialoguePlayed = false;
    [SerializeField]
    private GameObject image;
    private bool imageOn = false;

    void Start()
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

        if (image != null)
        {
            image.SetActive(true);
            imageOn = true;
        }

        dialogueManager.InitiateDialogueWithoutPlayer(conversation);
        dialoguePlayed = true;

         yield return new WaitUntil(() => !dialogueManager.DialogueActivated);

        if (imageOn && image != null)
        {
            image.SetActive(false);
            imageOn = false;
        }
    }
}
