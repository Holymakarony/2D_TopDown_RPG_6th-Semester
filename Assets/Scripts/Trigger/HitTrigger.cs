using System;
using UnityEngine;
using UnityEngine.Events;

public class HitTrigger : MonoBehaviour
{
    public bool magicTrigger;   
    public UnityEvent TriggerEvent;


    public  void OnHitTrigger()
    {
        TriggerEvent.Invoke();
    }

    public void BackToMainMenu()
    {
        // "Canvas" muss aktiv sein. "DialoguePanel" kann deaktiviert sein.
        GameObject mainMenu = GameObject.Find("UI_Canvas").transform.Find("MainMenu").gameObject;
        mainMenu.SetActive(true);
    }
}
