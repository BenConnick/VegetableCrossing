using System;
using UnityEngine;

public interface ICarryable
{
    bool PickUp(int playerId);
    bool PutDown(int playerId);
    Transform transform { get; }
}
