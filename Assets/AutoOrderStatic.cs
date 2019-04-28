using UnityEngine;
using UnityEditor;

public class AutoOrderStatic : MonoBehaviour
{
    public float yOffset;
    // Start is called before the first frame update
    public void Start()
    {
        GetComponent<SpriteRenderer>().sortingOrder = transform.GetSortOrder(yOffset);
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