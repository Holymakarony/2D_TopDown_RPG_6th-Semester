using UnityEngine;
using UnityEngine.Events;

public class PickUpWeaponHandler : MonoBehaviour
{

    public UnityEvent weaponCheckOnStartEvent;
    public UnityEvent weaponPickUpEvent;
    [Header("Visualisierung")]
    public GameObject interactionIndicator; 

    private bool playerInZone = false;

    private void Start() 
    {
        weaponCheckOnStartEvent.Invoke();
    }

    void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.E)) 
        {
            weaponPickUpEvent.Invoke();
        }
    }

    public void GetSword()
    {
        GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>().GetSword();
        gameObject.SetActive(false);
    }

    public void GetBow()
    {
        GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>().GetBow();
        gameObject.SetActive(false);
    }

    public void GetStaff()
    {
        GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>().GetStaff();
        gameObject.SetActive(false);
    }

    public void CheckSword()
    {
        if(GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>().hasSword == true)
        {
            gameObject.SetActive(false);
        }
    }

    public void CheckBow()
    {
        if(GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>().hasBow == true)
        {
            gameObject.SetActive(false);
        }
    }
    
    public void CheckStaff()
    {
        if(GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>().hasStaff == true)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            if (interactionIndicator != null)
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
