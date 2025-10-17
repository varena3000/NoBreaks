using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

//Title:How to make a Dialogue System in Unity
//Author: Brackeys
//Date: 21-08-25
//Code Version: N/A
//Available at: https://www.youtube.com/watch?v=_nRzoTzeyxU

public class DialogueTrigger : MonoBehaviour
{

    public Dialogue dialogue; // Reference to the Dialogue scriptable object 
    public bool istriggered = false;

    public void TriggerDialogue()
    {
        FindFirstObjectByType<DialogueManager>().StartDialogue(dialogue);
        istriggered = true;
    }
}
