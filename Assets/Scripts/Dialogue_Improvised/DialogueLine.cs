using UnityEngine;

[System.Serializable]
public struct DialogueLine
{
    public string speakerName;
    public Sprite speakerPortrait; // Das neue Feld für das NPC-Bild
    [TextArea(3, 10)]
    public string text;
}

[CreateAssetMenu(fileName = "NeuerDialog", menuName = "DialogSystem/Dialog")]
public class DialogueData : ScriptableObject
{
    public DialogueLine[] lines;
}
