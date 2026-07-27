using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    private Dictionary<string, int> activeQuestsProgress = new Dictionary<string, int>();
    private HashSet<string> completedQuests = new HashSet<string>();
    private Dictionary<string, QuestData> activeQuestsData = new Dictionary<string, QuestData>();

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    public void AcceptQuest(QuestData quest)
    {
        if (completedQuests.Contains(quest.questID) || activeQuestsProgress.ContainsKey(quest.questID)) return;

        activeQuestsProgress.Add(quest.questID, 0);
        activeQuestsData.Add(quest.questID, quest);
        Debug.Log($"Quest angenommen: {quest.questName}");

        
        RefreshUI(quest.questID);
    }

    // Erhöht den Fortschritt (z.B. Item aufgehoben oder Gegner besiegt)
    public void UpdateProgress(QuestType type, string targetID, int amount)
    {
        List<string> questsToUpdate = new List<string>();

        foreach (var pair in activeQuestsData)
        {
            // Wir prüfen, ob der Quest-Typ UND die targetID (der Name) übereinstimmen
            if (pair.Value.type == type && pair.Value.targetID == targetID)
            {
                questsToUpdate.Add(pair.Key);
            }
        }

        foreach (string questID in questsToUpdate)
        {
            activeQuestsProgress[questID] += amount;
            
            // UI bei jedem Fortschritt aktualisieren
            RefreshUI(questID);
        }
    }

    public bool IsQuestActive(string questID) => activeQuestsProgress.ContainsKey(questID);
    public bool IsQuestCompleted(string questID) => completedQuests.Contains(questID);

    public bool CanCompleteQuest(string questID)
    {
        if (!activeQuestsProgress.ContainsKey(questID)) return false;
        return activeQuestsProgress[questID] >= activeQuestsData[questID].requiredAmount;
    }

    public void CompleteQuest(string questID)
{
    if (CanCompleteQuest(questID))
    {
        activeQuestsProgress.Remove(questID);
        activeQuestsData.Remove(questID);
        completedQuests.Add(questID);
        
        // Markiere AUCH den ursprünglichen Start-Dialog als erledigt, falls nötig
        if (DialogueSaveManager.Instance != null)
        {
            DialogueSaveManager.Instance.MarkAsCompleted(questID);
        }

        Debug.Log($"Quest erfolgreich beendet!");

        // Blendet das Textfeld an der Seite aus
        if (QuestLogUI.Instance != null)
        {
            QuestLogUI.Instance.HideQuest();
        }
    }
}


    private void RefreshUI(string questID)
    {
        if (QuestLogUI.Instance != null && activeQuestsData.ContainsKey(questID))
        {
            QuestData data = activeQuestsData[questID];
            int currentProgress = activeQuestsProgress[questID];
            
            QuestLogUI.Instance.UpdateQuestDisplay(
                data.questName, 
                data.description, 
                currentProgress, 
                data.requiredAmount,
                data.type
            );
        }
    }
}
