using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public ItemSpriteData ItemData;
    public Image ItemImage;
    public TextMeshProUGUI Counter;

    public ItemType Item;
    public int Amount;

    public void Update()
    {
        // TODO optimize
        ItemImage.sprite = ItemData.GetSprite(Item);
        Counter.text = Amount.ToString();
    }
}
