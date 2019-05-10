using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InventoryManager
{
    public const int MAX_INV_SLOTS = 18;

    private static readonly List<InventoryItem> cachedInventory = new List<InventoryItem>();
    private static float lastReadTime;

    public static IList<InventoryItem> GetInventory()
    {
        if (SaveManager.GetLastChangeTime() > lastReadTime)
        {
            cachedInventory.Clear();
            for (int i = 0; i < MAX_INV_SLOTS; i++)
            {
                cachedInventory.Add(new InventoryItem()
                {
                    Item = (ItemType)SaveManager.GetInt(SaveManager.Category.InventoryItemTypes, i),
                    Quantity = SaveManager.GetInt(SaveManager.Category.InventoryItemQuantities, i)
                });
            }
            lastReadTime = Time.time;
        }
        return cachedInventory;
    }

    public static bool AddItem(ItemType item, int quantity)
    {
        // first, look for a matching type
        for (int slot = 0; slot < MAX_INV_SLOTS; slot++)
        {
            // if the type matches
            int typeInSlot = SaveManager.GetInt(SaveManager.Category.InventoryItemTypes, slot);
            if ((int)item == typeInSlot)
            {
                // first, get the amount already in
                int amountInSlot = SaveManager.GetInt(SaveManager.Category.InventoryItemQuantities, slot);
                // add the slot amount 
                SaveManager.SetInt(SaveManager.Category.InventoryItemQuantities, slot, amountInSlot + quantity);
                return true; // done
            }
        }
        // no match found, look for an available slot
        for (int slot = 0; slot < MAX_INV_SLOTS; slot++)
        {
            // if the slot is vacant
            int typeInSlot = SaveManager.GetInt(SaveManager.Category.InventoryItemTypes, slot);
            if ((int)ItemType.Default == typeInSlot)
            {
                // assign this slot 
                SaveManager.SetInt(SaveManager.Category.InventoryItemTypes, slot, (int)item);
                // set to the amount 
                SaveManager.SetInt(SaveManager.Category.InventoryItemQuantities, slot, quantity);
                return true; // done
            }
        }
        // no available slots
        return false;
    }

    public static bool RemoveItem(ItemType item, int quantity)
    {
        int amountToRemove = quantity; // we will subtract from this
        for (int slot = 0; slot < MAX_INV_SLOTS; slot++)
        {
            // if the type matches
            int typeInSlot = SaveManager.GetInt(SaveManager.Category.InventoryItemTypes, slot);
            if ((int)item == typeInSlot)
            {
                int amountInSlot = SaveManager.GetInt(SaveManager.Category.InventoryItemQuantities, slot);
                if (amountInSlot > 0)
                {
                    // remove amount from slot, or set slot to zero
                    SaveManager.SetInt(SaveManager.Category.InventoryItemQuantities, slot, Mathf.Max(0, amountInSlot - amountToRemove));
                    // if the 
                    if (amountToRemove >= amountInSlot)
                        SaveManager.SetInt(SaveManager.Category.InventoryItemTypes, slot, (int)ItemType.Default);
                    // track how much is left to remove
                    amountToRemove -= amountInSlot;
                }
            }
            // if there is none left to remove, success
            if (amountToRemove <= 0)
            {
                return true;
            }
        }
        return false;
    }
}

public struct InventoryItem
{
    public ItemType Item;
    public int Quantity;
}
