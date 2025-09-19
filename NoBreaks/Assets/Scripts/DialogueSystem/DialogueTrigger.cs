using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    //Title:How to make a Dialogue System in Unity
    //Author: Brackeys
    //Date: 21-08-25
    //Code Version: N/A
    //Available at: https://www.youtube.com/watch?v=_nRzoTzeyxU

    public Dialogue dialogue; // Reference to the Dialogue scriptable object 

    public void TriggerDialogue()
    {
        // This method is called to start the dialogue
        FindFirstObjectByType<DialogueManager>().StartDialogue(dialogue);
    }
}
