using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Importing Unity UI namespace for UI elements
using UnityEngine.SceneManagement;
using TMPro; // Importing Scene Management namespace for scene handling

public class DialogueManager : MonoBehaviour
{
    // This script manages the dialogue system in the game.

    //Title:How to make a Dialogue System in Unity
    //Author: Brackeys
    //Date: 21-08-25
    //Code Version: N/A
    //Available at: https://www.youtube.com/watch?v=_nRzoTzeyxU

    
    public TextMeshProUGUI dialogueText; // UI Text element to display the dialogue text

    private Queue<string> sentences; // Queue to hold the sentences of the dialogue

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue (Dialogue dialogue)
    {
        // This method is called to start the dialogue

    

        sentences.Clear(); // Clear any previous sentences

        // Enqueue each sentence from the dialogue into the queue
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence(); // Display the first sentence

    }

    public void DisplayNextSentence()
    {
        // This method displays the next sentence in the dialogue
        if (sentences.Count == 0)
        {
            EndDialogue(); // If no more sentences, end the dialogue
            return;
        }
        string sentence = sentences.Dequeue(); // Dequeue the next sentence
        StopAllCoroutines(); // Stop any currently running coroutines
        StartCoroutine(TypeSentence(sentence)); // Start typing out the sentence character by character
    }

    IEnumerator TypeSentence (string sentence)
    {
        // This coroutine types out the sentence character by character
        dialogueText.text = ""; // Clear the dialogue text
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter; // Append each letter to the dialogue text
            yield return null; // Wait for the next frame
        }
    }

    void EndDialogue()
    {
        // This method is called when the dialogue ends

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
