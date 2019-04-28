using UnityEngine;

public class AlignToISOGrid : MonoBehaviour
{
    public Vector3 offset;

    public const float GRID_SIZE = 0.2f;
    private const float GRID_Y = GRID_SIZE * Utils.ONE_OVER_SQRT2;

    public void DoAlign()
    {
        transform.position = new Vector3(
            Mathf.Round(transform.position.x / GRID_SIZE) * GRID_SIZE,
            Mathf.Round(transform.position.y / GRID_Y) * GRID_Y,
            transform.position.z);
    }
}
