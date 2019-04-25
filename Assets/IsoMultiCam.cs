using UnityEngine;
using IndieMarc.TopDown;

[RequireComponent(typeof(Camera))]
public class IsoMultiCam : MonoBehaviour
{
    public Vector3 offset;

    private Camera self;
    private TopDownCharacter[] players;

    private void Start()
    {
        self = GetComponent<Camera>();
        self.orthographic = true;
    }

    // Update is called once per frame
    void Update()
    {
        float maxDist = 0f;
        Vector3 total = Vector3.zero;
        foreach (var character in Manager.Inst.GetCharacters())
        {
            total += character.transform.position;
            foreach (var p in Manager.Inst.GetCharacters())
            {
                float d = (p.transform.position - character.transform.position).magnitude;
                if (d > maxDist) maxDist = d;
            }
        }
        Vector3 target = total *= (1f / (float)Manager.Inst.GetCharacters().Count);
        transform.position = target + offset;
        //transform.forward = target - transform.position; maybe want this later
        self.orthographicSize = Mathf.Clamp(maxDist*0.5f, 0.5f, 1000f);
    }
}
