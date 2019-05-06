using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FarmPlot : MonoBehaviour, IInteractionTrigger
{
    public int Id;
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
        SaveManager.SetFarmState(Id, state);
        SaveManager.SetFarmPlant(Id, plant);
        UpdateSprite();
        SaveManager.Save();
        RetriggerColliders();
    }

    void Start()
    {
        Manager.Register(this);
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

    public void DoInteraction(int playerId)
    {
        var chars = Manager.GetCharacters();
        var pc = chars[playerId];
        switch (state)
        {
            // plant a seed
            case FarmState.Empty:
                SetState(FarmState.Seeded, pc.GetHeldSeed());
                break;
            // water the plant
            case FarmState.Seeded:
                SetState(FarmState.Sapling);
                break;
            // harvest the plant
            case FarmState.Harvestable:
                SetState(FarmState.Empty);
                break;
        }
    }

    private void RetriggerColliders()
    {
        StartCoroutine(Utils.ColliderOnOff(GetComponent<Collider2D>()));
    }
}
