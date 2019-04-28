using UnityEngine;
using IndieMarc.TopDown;

[RequireComponent(typeof(Camera))]
public class IsoMultiCam : MonoBehaviour
{
    public Vector3 offset;
    public float smoothing = 10f;

    private Camera self;

    private void Start()
    {
        self = GetComponent<Camera>();
        self.orthographic = true;
    }

    // Update is called once per frame
    void Update()
    {
        float maxDist = 0f; // largest distance between two characters
        Vector3 avg = Vector3.zero; // vector that will represent the midpoint of all characters
        foreach (var character in Manager.GetCharacters())
        {
            avg += character.transform.position;
            // cal
            foreach (var p in Manager.GetCharacters())
            {
                float d = (p.transform.position - character.transform.position).magnitude;
                if (d > maxDist) maxDist = d;
            }
        }
        Vector3 target = avg *= (1f / ((float)Manager.GetCharacters().Count + 1.0001f));
        float lerpAmount = Time.deltaTime / (smoothing + 0.01f);
        transform.position = Vector3.Lerp(transform.position, target + offset, lerpAmount);
        //transform.forward = target - transform.position; maybe want this later
        self.orthographicSize = Mathf.Lerp(self.orthographicSize, Mathf.Clamp(maxDist*0.75f, 0.5f, 1000f), lerpAmount);
    }
}
