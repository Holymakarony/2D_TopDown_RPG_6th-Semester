using UnityEngine;
using TMPro;

public class QuestLogUI : MonoBehaviour
{
    public static QuestLogUI Instance { get; private set; }

    [Header("UI-Komponenten")]
    public TextMeshProUGUI questTitleText;
    public TextMeshProUGUI questDescriptionText;
    public GameObject questPanel; // Das übergeordnete UI-Fenster zum Ein-/Ausblenden

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Am Anfang des Spiels das Quest-Fenster ausblenden, falls keine Quest aktiv ist
        HideQuest();
    }

    // Zeigt die Questdaten und den aktuellen Fortschritt an
    public void UpdateQuestDisplay(string title, string description, int current, int required, QuestType type)
    {
        questPanel.SetActive(true);
        questTitleText.text = title;

        // Unterscheidung für die Textanzeige je nach Quest-Typ
        if (type == QuestType.Talk)
        {
            string status = current >= required ? "Bereit zur Abgabe" : "Noch nicht gesprochen";
            questDescriptionText.text = $"{description}\n\n<color=#FFCC00>Status: {status}</color>";
        }
        else
        {
            questDescriptionText.text = $"{description}\n\n<color=#FFCC00>Fortschritt: {current} / {required}</color>";
        }
    }

    public void HideQuest()
    {
        if (questPanel != null)
        {
            questPanel.SetActive(false);
        }
    }
}
