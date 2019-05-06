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
}

public struct InventoryItem
{
    public ItemType Item;
    public int Quantity;
}
