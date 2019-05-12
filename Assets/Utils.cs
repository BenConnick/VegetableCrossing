using System.Collections;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public static class Utils
{
    public const float ONE_OVER_SQRT2 = 0.7071067811f;

    public static Vector3 GetCanvasPostion(Transform worldSpaceTransform)
    {
        if (HUD.Instance == null) return Vector3.zero;
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(worldSpaceTransform.position);

        var canvasScaler = HUD.Instance.CanvasScaler;

        float xRatio = canvasScaler.referenceResolution.x / Screen.width;
        float yRatio = canvasScaler.referenceResolution.y / Screen.height;

        float ratio = (xRatio > yRatio) ? xRatio : yRatio;

        return new Vector3(screenPoint.x * ratio - Screen.width * 0.5f * ratio, screenPoint.y * ratio - Screen.height * 0.5f * ratio);
    }

    public static IEnumerator ColliderOnOff(Collider2D collider)
    {
        collider.enabled = false;
        yield return null;
        collider.enabled = true;
    }
}

public static class OrderUtils
{
    public static int GetSortOrder(this Transform t, float yOffset = 0)
    {
        return Mathf.RoundToInt((t.position.y + yOffset) * 1000f) * -1;
    }

    [MenuItem("Tools/AutoOrder Immediate &o")]
    public static void OrderAndAlignAll()
    {
        AlignAllToGrid();
        OrderAll();
        Undo.FlushUndoRecordObjects();
    }

    private static void OrderAll()
    {
        AutoOrderStatic[] all = Object.FindObjectsOfType<AutoOrderStatic>();
        foreach (var item in all)
        {
            var spriteRenderer = item.GetComponent<SpriteRenderer>();
            Undo.RecordObject(spriteRenderer, "Order by y");
            item.Start();
            PrefabUtility.RecordPrefabInstancePropertyModifications(spriteRenderer);
        }
        // Undo.FlushUndoRecordObjects(); covered by outer func
    }

    private static void AlignAllToGrid()
    {
        AlignToISOGrid[] all = Object.FindObjectsOfType<AlignToISOGrid>();
        foreach (var item in all)
        {
            Undo.RecordObject(item.transform, "Order by y");
            item.DoAlign();
        }
        // Undo.FlushUndoRecordObjects(); covered by outer func
    }
}

public interface IInteractionTrigger
{
    string GetTooltipText(int playerId);
    void DoInteraction(int playerId);
    bool IsInteractable(int playerId);
    Transform transform { get; }
}
