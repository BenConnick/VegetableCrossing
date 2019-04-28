using UnityEditor;
using UnityEngine;

public static class Utils
{
    public static Vector3 GetCanvasPostion(Transform worldSpaceTransform)
    {
        if (HUD.Instance == null) return Vector3.zero;
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(worldSpaceTransform.position);

        var canvasScaler = HUD.Instance.CanvasScaler;

        float xRatio = Screen.width / canvasScaler.referenceResolution.x;
        float yRatio = Screen.height / canvasScaler.referenceResolution.y;

        float ratio = (xRatio < yRatio) ? xRatio : yRatio;

        return viewportPoint * ratio;
    }


}

public static class OrderUtils
{
    public static int GetSortOrder(this Transform t, float yOffset = 0)
    {
        return Mathf.RoundToInt((t.position.y + yOffset) * 1000f) * -1;
    }

    [MenuItem("Tools/AutoOrder Immediate &o")]
    public static void OrderAll()
    {
        AutoOrderStatic[] all = Object.FindObjectsOfType<AutoOrderStatic>();
        foreach (var item in all)
        {
            var spriteRenderer = item.GetComponent<SpriteRenderer>();
            Undo.RecordObject(spriteRenderer, "Order by y");
            item.Start();
            PrefabUtility.RecordPrefabInstancePropertyModifications(spriteRenderer);
        }
        Undo.FlushUndoRecordObjects();
    }
}
