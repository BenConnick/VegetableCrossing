using UnityEngine;

[RequireComponent(typeof(Camera))]
public class IsoMultiCam : MonoBehaviour
{
    public Vector3 offset;

    private Camera self;
    private Transform target;

    private void Start()
    {
        self.orthographic = true;
    }

    // Update is called once per frame
    void Update()
    {
        float dist = 0.5f;
        transform.position = target.position + offset;
        transform.forward = target.position - transform.position;
        self.orthographicSize = Mathf.Clamp(dist, 0.5f, 1000f);
    }


}
