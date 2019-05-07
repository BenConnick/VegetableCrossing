using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ScreenPrefabData", fileName = "ScreenPrefabData", order = 1)]
public class ScreenPrefabData : ScriptableObject
{
    public StartScreen StartScreen;
    public InventoryScreen InventoryScreen;
}
