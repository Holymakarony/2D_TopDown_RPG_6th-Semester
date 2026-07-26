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
    }

    // Erhöht den Fortschritt (z.B. Item aufgehoben oder Gegner besiegt)
    public void UpdateProgress(QuestType type, string targetID, int amount)
    {
        List<string> questsToUpdate = new List<string>();

        foreach (var pair in activeQuestsData)
        {
            if (pair.Value.type == type && pair.Value.targetID == targetID)
            {
                questsToUpdate.Add(pair.Key);
            }
        }

        foreach (string questID in questsToUpdate)
        {
            activeQuestsProgress[questID] += amount;
            QuestData data = activeQuestsData[questID];
            
            Debug.Log($"Fortschritt für {data.questName}: {activeQuestsProgress[questID]}/{data.requiredAmount}");

            // Automatischer Abschluss bei "Sprechen", wenn man den NPC triggert
            if (type == QuestType.Talk && activeQuestsProgress[questID] >= data.requiredAmount)
            {
                // Kann beim Questgeber abgegeben werden
                // Soundeffekt hier einfügen
            }
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
            Debug.Log($"Quest erfolgreich beendet!");
        }
    }
}
