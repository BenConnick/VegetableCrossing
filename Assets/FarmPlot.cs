using System;
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
        RetriggerColliders();
    }

    public FarmState GetState()
    {
        return state;
    }

    void Start()
    {
        Manager.Register(this);
    }

    private void UpdateSprite()
    {
        SR.sprite = Sprites.GetPlotSprite(state, plant);
    }

    public bool IsInteractable(int playerId)
    {
        switch (state)
        {
            // plant a seed
            case FarmState.Empty:
                return Manager.GetP(playerId).Has<SeedBag>();
            // water the plant
            case FarmState.Seeded:
                return Manager.GetP(playerId).Has<WateringCan>();
            // growing
            case FarmState.Sapling:
                return false; // cannot interrupt
            // harvest the plant
            case FarmState.Harvestable:
                return !Manager.GetP(playerId).Has<ICarryable>(); // nothing held
        }
        return false; // default
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
        var pc = Manager.GetP(playerId);
        switch (state)
        {
            // plant a seed
            case FarmState.Empty:
                if (InventoryManager.RemoveItem(ItemType.RabbitSeed, 1))
                    SetState(FarmState.Seeded, pc.GetHeldSeed());
                break;
            // water the plant
            case FarmState.Seeded:
                if (pc.Has<WateringCan>()) { 
                    SetState(FarmState.Sapling);
                    // begin growth timer
                    SaveManager.SetFarmDoneTime(Id, DateTime.Now.AddSeconds(2f));
                }
                break;
            // harvest the plant
            case FarmState.Harvestable:
                if (InventoryManager.AddItem(ItemType.Rabbit, 1))
                    SetState(FarmState.Empty);
                break;
        }
    }

    private void RetriggerColliders()
    {
        StartCoroutine(Utils.ColliderOnOff(GetComponent<Collider2D>()));
    }

    public void OnFinishGowing()
    {
        SetState(FarmState.Harvestable);
    }
}
