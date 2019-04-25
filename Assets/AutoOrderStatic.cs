using UnityEngine;

public class AutoOrderStatic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sortingOrder = transform.GetSortOrder();
    }
    
}

public static class OrderUtils
{
    public static int GetSortOrder(this Transform t)
    {
        return Mathf.RoundToInt(t.position.y * 10000f) * -1;
    }
}