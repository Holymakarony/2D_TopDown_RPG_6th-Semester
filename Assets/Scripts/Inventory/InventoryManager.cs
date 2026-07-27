using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Schwert")]
    [SerializeField] private WeaponInfo swordInfo;
    [SerializeField] private GameObject swordSlot;
    [SerializeField] private GameObject swordEquippedItem;
    public bool hasSword = true;
    [Header("Bogen")]
    [SerializeField] private WeaponInfo bowInfo;
    [SerializeField] private GameObject bowSlot;
    [SerializeField] private GameObject bowEquippedItem;
    public bool hasBow = false;
    [Header("Stab")]
    [SerializeField] private WeaponInfo staffInfo;
    [SerializeField] private GameObject staffSlot;
    [SerializeField] private GameObject staffEquippedItem;
    public bool hasStaff = false;
    
    
    public void GetSword()
    {
        swordSlot.GetComponent<InventorySlot>().SetWeaponInfo(swordInfo);
        swordEquippedItem.SetActive(true);
        hasSword = true;
    }

    public void GetBow()
    {
        bowSlot.GetComponent<InventorySlot>().SetWeaponInfo(bowInfo);
        bowEquippedItem.SetActive(true);
        hasBow = true;
    }

    public void GetStaff()
    {
        staffSlot.GetComponent<InventorySlot>().SetWeaponInfo(staffInfo);
        staffEquippedItem.SetActive(true);
        hasStaff = true;
    }
}
