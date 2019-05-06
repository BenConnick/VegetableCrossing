using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemSpriteData", fileName = "ItemSpriteData", order = 1)]
public class ItemSpriteData : ScriptableObject
{
    public Sprite Default;
    public Sprite RabbitSeed;
    public Sprite Rabbit;

    public Sprite GetSprite(ItemType item)
    {
        switch(item)
        {
            case ItemType.RabbitSeed:
                return RabbitSeed;
            case ItemType.Rabbit:
                return Rabbit;
        }
        return Default;
    }
}
