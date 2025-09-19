using System.Collections;
using System.Collections.Generic;  
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    //Title:How to make a Dialogue System in Unity
    //Author: Brackeys
    //Date: 21-08-25
    //Code Version: N/A
    //Available at: https://www.youtube.com/watch?v=_nRzoTzeyxU

   
    [TextArea(3, 10)] // Allows for multi-line text input in the Unity Inspector
    public string [] sentences;
}
