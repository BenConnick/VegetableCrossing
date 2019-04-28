using UnityEngine;

public class HUD : MonoBehaviour
{
    private static HUD inst;
    public static HUD Instance { get { return inst; } }
    public UIPrefabData Assets;

    private void Awake()
    {
        // unity-style singleton
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Debug.LogError("Cannot have multiple HUD objects!");
            Destroy(gameObject);
        }
    }

    public static void ShowToast()
    {
        Instantiate(Instance.Assets.FloatLabel, Instance.transform);
    }
}
