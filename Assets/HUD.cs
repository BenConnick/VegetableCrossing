using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class HUD : MonoBehaviour
{
    private static HUD inst;
    public static HUD Instance { get { return inst; } }
    public UIPrefabData Assets;
    public CanvasScaler CanvasScaler;

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
        FloatLabel label = Instantiate(Instance.Assets.FloatLabel, Instance.transform);
        label.SetText(message);
    }
}
