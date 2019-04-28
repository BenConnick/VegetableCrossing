using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class HUD : MonoBehaviour
{
    private static HUD inst;
    public static HUD Instance { get { return inst; } }
    public UIPrefabData Assets;
    public CanvasScaler CanvasScaler;
    
    private readonly List<Tooltip> tooltips = new List<Tooltip>();

    private void Awake()
    {
        // unity-style singleton
        if (inst == null)
        {
            // initialize
            inst = this;
            CanvasScaler = GetComponent<CanvasScaler>();
        }
        else
        {
            Debug.LogError("Cannot have multiple HUD objects!");
            Destroy(gameObject);
        }
    }

    public static void ShowToast(string message)
    {
        if (Instance == null) return;
        FloatLabel label = Instantiate(Instance.Assets.FloatLabel, Instance.transform);
        label.SetText(message);
    }

    public static Tooltip ShowTooltip(string message, Transform worldspaceTarget)
    {
        if (Instance == null) return null;
        Tooltip tooltip = Instantiate(Instance.Assets.Tooltip, Instance.transform);
        tooltip.worldspaceTarget = worldspaceTarget;
        tooltip.SetText(message);
        Instance.tooltips.Add(tooltip);
        return tooltip;
    }

    public static void HideTooltip(Tooltip tt)
    {
        if (Instance == null) return;
        Instance.tooltips.Remove(tt);
        Destroy(tt.gameObject);
    }
}
