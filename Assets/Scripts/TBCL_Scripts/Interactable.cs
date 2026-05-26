using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private GameObject interactPrompt;
    
    private PlayerController playercontroller;
    
    public UnityEvent interactEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.GetComponent<PlayerController>()){ return; }
        
        interactPrompt.SetActive(true);
        playercontroller = collision.GetComponent<PlayerController>();
        playercontroller.infrontOfInteractable = true;
        playercontroller.interactable = this.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!collision.GetComponent<PlayerController>()){ return; }
        
        interactPrompt.SetActive(false);
        playercontroller.infrontOfInteractable = false;
        playercontroller.interactable = null;
        playercontroller = null;
    }

    public void Interact()
    {
        interactEvent.Invoke();
    }
}
