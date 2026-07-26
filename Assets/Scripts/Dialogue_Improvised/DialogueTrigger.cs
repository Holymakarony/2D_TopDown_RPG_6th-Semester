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
        if (DialogueSaveManager.Instance != null && DialogueSaveManager.Instance.IsDialogueCompleted(dialogueID))
        {
            isAlreadyCompleted = true;
        }

        currentActiveDialogue = dialogue; // Standard

        if (associatedQuest != null && QuestManager.Instance != null)
        {
            string qID = associatedQuest.questID;

            if (QuestManager.Instance.IsQuestCompleted(qID))
            {
                isAlreadyCompleted = true; // NPC hat nichts mehr zu sagen
            }
            else if (QuestManager.Instance.CanCompleteQuest(qID))
            {
                currentActiveDialogue = associatedQuest.questCompletedDialogue;
                isAlreadyCompleted = false; // Muss für die Abgabe wieder sprechbar sein
            }
            else if (QuestManager.Instance.IsQuestActive(qID))
            {
                currentActiveDialogue = associatedQuest.questIncompleteDialogue;
                isAlreadyCompleted = false;

                // Falls es eine "Spreche mit jemandem"-Quest ist und DIESER NPC das Ziel ist:
                if (associatedQuest.type == QuestType.Talk && associatedQuest.targetID == dialogueID)
                {
                    QuestManager.Instance.UpdateProgress(QuestType.Talk, dialogueID, 1);
                }
            }
            else
            {
                // Quest wurde noch nicht angenommen -> Spiele den Dialog, der die Quest startet
                currentActiveDialogue = associatedQuest.questAcceptedDialogue;
            }
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
