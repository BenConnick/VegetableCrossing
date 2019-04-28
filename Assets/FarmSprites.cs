using UnityEngine;
using FarmState = FarmPlot.FarmState;
using PlantType = FarmPlot.PlantType;

[CreateAssetMenu(menuName = "ScriptableObjects/FarmSprites", fileName = "FarmSprites", order = 1)]
public class FarmSprites : ScriptableObject
{
    [Header("Empty Plot")]
    public Sprite FarmPlot;

    [Header("Rabbit Farm")]
    public Sprite RabbitSeed;
    public Sprite RabbitSapling;
    public Sprite RabbitHarvestable;

    public Sprite GetPlotSprite(FarmState state, PlantType plant)
    {
        switch (state)
        {
            case FarmState.Empty:
                return FarmPlot;
            case FarmState.Seeded:
                switch (plant)
                {
                    case PlantType.Rabbit:
                        return RabbitSeed;
                }
                break;
            case FarmState.Sapling:
                switch (plant)
                {
                    case PlantType.Rabbit:
                        return RabbitSapling;
                }
                break;
            case FarmState.Harvestable:
                switch (plant)
                {
                    case PlantType.Rabbit:
                        return RabbitHarvestable;
                }
                break;
        }
        Debug.LogError($"FarmSpirtes: Could not find {plant}(plant) - {state}");
        return FarmPlot; // default to empty;
    }
}
