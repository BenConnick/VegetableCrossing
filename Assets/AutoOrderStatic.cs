using UnityEngine;

public class AutoOrderStatic : MonoBehaviour
{
    public float yOffset;
    // Start is called before the first frame update
    public void Start()
    {
        GetComponent<SpriteRenderer>().sortingOrder = transform.GetSortOrder(yOffset);
    }    
}