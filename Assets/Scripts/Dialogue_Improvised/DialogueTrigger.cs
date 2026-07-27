using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Quest-System (Optional)")]
    public QuestData associatedQuest;

    [Header("Standard Dialog-Daten")]
    public DialogueData dialogue;
    public string dialogueID;

    [Header("Visualisierung")]
    public GameObject interactionIndicator; 

    private DialogueManager manager;
    private bool playerInZone = false;
    private bool isAlreadyCompleted = false;
    private DialogueData currentActiveDialogue;

    void Start()
    {
        manager = FindFirstObjectByType<DialogueManager>();
        UpdateDialogueState();
    }

    // Bestimmt, welcher Dialog basierend auf dem Quest-Status geladen werden muss
    void UpdateDialogueState()
    {
        // Standard-Zustand zurücksetzen
        isAlreadyCompleted = false; 
        currentActiveDialogue = dialogue;

        // 1. Priorität: Gibt es eine Quest, und können wir sie abgeben?
        if (associatedQuest != null && QuestManager.Instance != null)
        {
            string qID = associatedQuest.questID;

            if (QuestManager.Instance.IsQuestCompleted(qID))
            {
                isAlreadyCompleted = true; // Quest komplett fertig -> NPC schweigt
            }
            else if (QuestManager.Instance.CanCompleteQuest(qID))
            {
                currentActiveDialogue = associatedQuest.questCompletedDialogue; // Genug Gold gesammelt!
                isAlreadyCompleted = false; // Wichtig: NICHT sperren, da wir sprechen wollen!
            }
            else if (QuestManager.Instance.IsQuestActive(qID))
            {
                currentActiveDialogue = associatedQuest.questIncompleteDialogue; // Noch nicht genug Gold
                isAlreadyCompleted = false;
            }
            else
            {
                currentActiveDialogue = associatedQuest.questAcceptedDialogue; // Erstkontakt / Quest annehmen
            }
        }
        // 2. Priorität: Wenn KEINE Quest verknüpft ist, greift der normale DialogueSaveManager
        else if (DialogueSaveManager.Instance != null && DialogueSaveManager.Instance.IsDialogueCompleted(dialogueID))
        {
            isAlreadyCompleted = true;
        }

        if (interactionIndicator != null) interactionIndicator.SetActive(false);
    }


    void Update()
    {
        if (isAlreadyCompleted) return;

        if (playerInZone && Input.GetKeyDown(KeyCode.E)) 
        {
            if (manager != null)
            {
                if (manager.dialogueCanvas.activeSelf)
                {
                    manager.DisplayNextLine();

                    if (!manager.dialogueCanvas.activeSelf) // Dialog gerade beendet
                    {
                        OnDialogueEnded();
                    }
                }
                else
                {
                    UpdateDialogueState(); 
                    
                    if (interactionIndicator != null) interactionIndicator.SetActive(false);
                    manager.StartDialogue(currentActiveDialogue, dialogueID);
                }
            }
        }
    }

    void OnDialogueEnded()
    {
        if (associatedQuest != null && QuestManager.Instance != null)
        {
            string qID = associatedQuest.questID;

            if (!QuestManager.Instance.IsQuestActive(qID) && !QuestManager.Instance.IsQuestCompleted(qID))
            {
                // War der Erst-Dialog: Quest jetzt offiziell starten
                QuestManager.Instance.AcceptQuest(associatedQuest);
            }
            else if (QuestManager.Instance.CanCompleteQuest(qID))
            {
                // War der Abgabe-Dialog: Quest beenden
                QuestManager.Instance.CompleteQuest(qID);
            }
        }

        UpdateDialogueState();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isAlreadyCompleted) return;
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            if (interactionIndicator != null && (manager == null || !manager.dialogueCanvas.activeSelf))
                interactionIndicator.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            if (interactionIndicator != null) interactionIndicator.SetActive(false);
        }
    }
}
