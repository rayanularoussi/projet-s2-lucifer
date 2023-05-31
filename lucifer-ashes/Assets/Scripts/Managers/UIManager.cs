using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerInventory playerInventory;
    EquipementWindowUI equipmentWindowUI;

    [Header("UI Windows")]
    public GameObject hudWindow;
    public GameObject selectWindow;
    public GameObject weaponInventoryWindow;

    [Header("Weapon Inventory")]
    public GameObject weaponInventorySlotPrefab;
    public Transform weaponInventorySlotsParent;
    WeaponInventorySlot[] weaponInventorySlots;

    private void Awake()
    {
        equipmentWindowUI = FindObjectOfType<EquipementWindowUI>();
    }
    
    private void Start()
    {
        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
        equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
    }

    public void UpdateUI()
    {
        #region Weapon Inventory Slots
        for(int i = 0; i < weaponInventorySlots.Length; i++)
        {
            if(i < playerInventory.weaponsInventory.Count)
            {
                if(weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
                {
                    Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                    weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                }
                weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
            }
            else
            {
                weaponInventorySlots[i].ClearInventorySlot();
            }
        }


        #endregion
    }

    public void OpenSelectWindow()
    {
        selectWindow.SetActive(true);
        Cursor.visible = true;
    }

    public void CloseSelectWindow()
    {
        selectWindow.SetActive(false);
        Cursor.visible = true;
    }

    public void CloseAllInventoryWindows()
    {
        weaponInventoryWindow.SetActive(false);
    }
}