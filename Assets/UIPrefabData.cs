using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

[CreateAssetMenu(menuName = "ScriptableObjects/UIPrefabData", fileName = "UIPrefabData", order = 1)]
public class UIPrefabData : ScriptableObject
{
    public TextMeshProUGUI H1Label;
    public TextMeshProUGUI H2Label;
    public TextMeshProUGUI PLabel;
    public Button Button;
    public FloatLabel FloatLabel;
    public Tooltip Tooltip;
}
