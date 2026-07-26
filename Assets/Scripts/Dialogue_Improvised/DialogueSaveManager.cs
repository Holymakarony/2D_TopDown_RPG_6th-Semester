using System.Collections.Generic;
using UnityEngine;

public class DialogueSaveManager : MonoBehaviour
{
    public static DialogueSaveManager Instance { get; private set; }

    // Speichert die IDs aller bereits abgespielten Dialoge
    private HashSet<string> completedDialogues = new HashSet<string>();

    void Awake()
    {
        // Singleton-Pattern: Verhindert, dass das Objekt beim Szenenwechsel verdoppelt wird
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Registriert einen Dialog als "erledigt"
    public void MarkAsCompleted(string dialogueID)
    {
        if (!string.IsNullOrEmpty(dialogueID))
        {
            completedDialogues.Add(dialogueID);
        }
    }

    // Prüft, ob der Dialog schon einmal lief
    public bool IsDialogueCompleted(string dialogueID)
    {
        return completedDialogues.Contains(dialogueID);
    }
}
