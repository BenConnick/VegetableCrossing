﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(menuName = "ScriptableObjects/UIPrefabData", fileName = "UIPrefabData", order = 1)]
public class UIPrefabData : ScriptableObject
{
    [Header("Core")]
    public TextMeshProUGUI H1Label;
    public TextMeshProUGUI H2Label;
    public TextMeshProUGUI PLabel;
    public Button Button;
    public FloatLabel FloatLabel;
    public Tooltip Tooltip;

    [Header("Other")]
    public InventorySlot InventorySlot;
}
