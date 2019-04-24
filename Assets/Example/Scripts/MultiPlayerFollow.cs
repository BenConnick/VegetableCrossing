using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPlayerFollow : MonoBehaviour
{
    PlayerController[] players;
    Vector3 _cameraOffset = new Vector3(0, 1.414f, 1.414f);
    float minDist = 4f;
    const float SmoothFactor = 10f;

    // Start is called before the first frame update
    void Start()
    {
        players = FindObjectsOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        Vector3 average = Vector3.zero;

        float maxDist = 0f;
        foreach (var item in players)
        {
            foreach (var p in players)
            {
                float d = (p.transform.position - item.transform.position).magnitude;
                if (d > maxDist) maxDist = d;
            }
            average += item.transform.position;
        }
        average *= (1f / (float)players.Length);

        Vector3 newPos = average + _cameraOffset * Mathf.Pow(Mathf.Max(minDist, maxDist), 0.75f);

        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);

        transform.LookAt(average);
    }
}
