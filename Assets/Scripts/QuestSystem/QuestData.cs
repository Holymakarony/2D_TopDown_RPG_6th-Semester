using UnityEngine;

public enum QuestType { Gather, Kill, Talk }

[CreateAssetMenu(fileName = "NeueQuest", menuName = "Quests/Quest")]
public class QuestData : ScriptableObject
{
    public string questID;
    public string questName;
    [TextArea(3, 5)]
    public string description;

    public QuestType type;
    
    [Header("Ziele")]
    [Tooltip("Name des Items, Gegners oder NPCs")]
    public string targetID; 
    public int requiredAmount = 1; // Wie viel gesammelt/getötet werden muss (für 'Talk' immer 1)

    [Header("Dialoge")]
    public DialogueData questAcceptedDialogue; // Wird beim Annehmen abgespielt
    public DialogueData questIncompleteDialogue; // Wenn man noch nicht fertig ist
    public DialogueData questCompletedDialogue; // Bei der Abgabe
}
