using UnityEngine;

public class AssetManager : MonoBehaviour
{
    public UIPrefabData UIPrefabs;
    public ScreenPrefabData ScreenPrefabs;

    private static AssetManager inst;
    public static AssetManager Inst { get { return inst; } }
    // Start is called before the first frame update
    void Awake()
    {
        if (inst != null)
        {
            Debug.LogError("Cannot instantiate multiple Asset Managers");
            Destroy(gameObject);
            return;
        }
        inst = this;

        Manager.Init();
        Manager.PerFrameUpdate();
    }

    private void Update()
    {
        Manager.PerFrameUpdate();
    }
}
