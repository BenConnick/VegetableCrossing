using UnityEngine;

public class AutoOrderStatic : MonoBehaviour
{
    public float yOffset;
    // Start is called before the first frame update
    void Start()
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
}