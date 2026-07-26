using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI-Elemente")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image portraitImage; // Das neue UI-Bildfeld im Inspector
    public GameObject dialogueCanvas;

    [Header("Einstellungen")]
    public float typingSpeed = 0.05f;

    private Queue<DialogueLine> linesQueue = new Queue<DialogueLine>();
    private bool isTyping = false;
    private string currentFullText = "";
    private string currentDialogueID; // Merkt sich die ID des aktuellen Dialogs

    public void StartDialogue(DialogueData dialogue, string dialogueID)
    {
        if (DialogueSaveManager.Instance != null && DialogueSaveManager.Instance.IsDialogueCompleted(dialogueID))
        {
            return; 
        }
        
        currentDialogueID = dialogueID; // ID speichern
        dialogueCanvas.SetActive(true);
        linesQueue.Clear();

        foreach (DialogueLine line in dialogue.lines)
        {
            linesQueue.Enqueue(line);
        }

        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = currentFullText;
            isTyping = false;
            return;
        }

        if (linesQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = linesQueue.Dequeue();
        nameText.text = currentLine.speakerName;
        currentFullText = currentLine.text;

        // Porträt-Logik: Zeigen wenn vorhanden, sonst ausblenden
        if (currentLine.speakerPortrait != null)
        {
            portraitImage.gameObject.SetActive(true);
            portraitImage.sprite = currentLine.speakerPortrait;
        }
        else
        {
            portraitImage.gameObject.SetActive(false); // Versteckt das Bild, wenn kein Porträt zugewiesen ist
        }

        StartCoroutine(TypeText(currentLine.text));
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        dialogueCanvas.SetActive(false);

        // Melde dem Save-Manager, dass dieser Dialog fertig ist
        if (DialogueSaveManager.Instance != null)
        {
            DialogueSaveManager.Instance.MarkAsCompleted(currentDialogueID);
        }
    }
}
