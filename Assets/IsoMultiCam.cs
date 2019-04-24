using UnityEngine;

public class IsoMultiCam : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
        transform.forward = target.position - transform.position;
    }
}
