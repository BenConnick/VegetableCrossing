using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FarmPlot : MonoBehaviour, IInteractionTrigger
{
    public int PlotId;
    public FarmSprites Sprites;
    public SpriteRenderer SR;

    public enum FarmState { Empty, Seeded, Sapling, Harvestable }
    public enum PlantType { Default, Rabbit }

    private FarmState state; // internal state, always set through SetState, even internally
    private PlantType plant; // internal state, always set through SetState, even internally

    private DateTime endTime;

    public void SetState(FarmState stateToSet, PlantType? plantToSet = null)
    {
        if (plantToSet.HasValue) plant = plantToSet.Value;
        state = stateToSet;
        SaveManager.SetFarmState(PlotId, state);
        SaveManager.SetFarmPlant(PlotId, plant);
        UpdateSprite();
        SaveManager.Save();
    }

    void Start()
    {
        SetState(SaveManager.GetFarmState(PlotId), SaveManager.GetFarmPlant(PlotId));
    }

    private void UpdateSprite()
    {
        SR.sprite = Sprites.GetPlotSprite(state, plant);
    }

    public bool IsInteractable()
    {
        return state != FarmState.Sapling;
    }

    public string GetTooltipText(int playerId)
    {
        string ret = "Error";
        switch (state)
        {
            case FarmState.Empty:
                ret = "Plant";
                break;
            case FarmState.Seeded:
                ret = "Water";
                break;
            case FarmState.Harvestable:
                ret = "Harvest";
                break;
        }
        return ret + playerId.ToPlayerTag();
    }

    public Action GetInteractAction(int playerId)
    {
        var chars = Manager.GetCharacters();
        var pc = chars[playerId];
        switch (state)
        {
            case FarmState.Empty:
                return () => { SetState(FarmState.Seeded, pc.GetHeldSeed()); };
            case FarmState.Seeded:
                return () => { SetState(FarmState.Sapling); };
            case FarmState.Harvestable:
                return () => { SetState(FarmState.Empty); };
        }
        return null;
    }
}