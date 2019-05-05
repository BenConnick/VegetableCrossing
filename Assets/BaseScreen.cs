using UnityEngine;

[RequireComponent(typeof(Canvas))]
public abstract class BaseScreen : MonoBehaviour
{
    public Canvas Canvas
    {
        get { return GetComponent<Canvas>(); } // optimize here if issues
    }
}
