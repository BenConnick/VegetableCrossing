using UnityEngine;

[RequireComponent(typeof(Camera))]
public class IsoMultiCam : MonoBehaviour
{
    public Vector3 offset;
    public float smoothing = 10f;

    private Camera self;

    public Camera GetCamera()
    {
        return self;
    }

    private void Start()
    {
        self = GetComponent<Camera>();
        self.orthographic = true;
    }

    // Update is called once per frame
    private void Update()
    {
        float maxDist = 0f; // largest distance between two characters
        Vector3 midpoint = Vector3.zero; // vector that will represent the midpoint of all characters

        // two player only version
        var chars = Manager.GetCharacters();
        Vector3 toVec = (chars[1].transform.position - chars[0].transform.position);
        midpoint = chars[0].transform.position + toVec * 0.5f;
        maxDist = toVec.magnitude;
        float lerpAmount = Time.deltaTime / (smoothing + 0.01f);
        transform.position = Vector3.Lerp(transform.position, midpoint + offset, lerpAmount);
        //transform.forward = target - transform.position; maybe want this later
        self.orthographicSize = Mathf.Lerp(self.orthographicSize, Mathf.Clamp(maxDist*0.75f, .75f, 1000f), lerpAmount);
    }
}
