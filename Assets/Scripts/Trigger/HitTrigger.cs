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
}
