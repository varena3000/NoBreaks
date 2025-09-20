using UnityEngine;

[CreateAssetMenu]
public class AdvancedDialogue : ScriptableObject
{
    public DialogueActors[] actors;

    [Header("Dialogue")]
    [TextArea]
    public string[] Dialogue;

    
}
