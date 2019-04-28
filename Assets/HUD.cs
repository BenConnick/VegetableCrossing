using UnityEngine;

public class HUD : MonoBehaviour
{
    private static HUD inst;
    public static HUD Instance { get { return inst; } }

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
}
