using UnityEngine;

[CreateAssetMenu]
public class AdvancedDialogueSO : ScriptableObject
{
    public DialogueActors[] actors;

    [Header("Dialogue")]
    [TextArea]
    public string[] Dialogue;

    
}
