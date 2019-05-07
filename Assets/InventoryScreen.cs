using UnityEngine;

public class InventoryScreen : BaseScreen
{
    public InventorySlot[] InventorySlots;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var inv = InventoryManager.GetInventory();
        for (int i = 0; i < inv.Count; i++)
        {
            if (i > InventorySlots.Length)
            {
                Debug.LogError($"Index out of range {i} is bigger than the number of slots in the UI!");
                return;
            }
            InventorySlots[i].Item = inv[i].Item;
            InventorySlots[i].Amount = inv[i].Quantity;
        }
    }
}
