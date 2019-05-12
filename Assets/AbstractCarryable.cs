using System;
using UnityEngine;

public abstract class AbstractCarryable : MonoBehaviour, IInteractionTrigger, ICarryable
{
    protected int playerId = -1;

    public void DoInteraction(int id)
    {
        var pc = Manager.GetP(id);
        if (playerId >= 0) pc.Drop();
        else pc.PickUp(this);
    }

    public string GetTooltipText(int playerId)
    {
        return $"Pick Up";
    }

    public bool IsInteractable(int _)
    {
        return playerId < 0;
    }

    public bool PickUp(int id)
    {
        if (playerId >= 0) return false;
        playerId = id;
        GetComponent<Collider2D>().enabled = false;
        return true;
    }

    public bool PutDown(int id)
    {
        if (playerId < 0) return false;
        GetComponent<Collider2D>().enabled = true;
        playerId = -1;
        return true;
    }

}
