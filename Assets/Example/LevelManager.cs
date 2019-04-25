using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Inst;
    // Start is called before the first frame update
    void Start()
    {
        if (Inst != null)
        {
            Debug.LogError("Cannot instantiate multiple Level Managers");
            Destroy(gameObject);
            return;
        }
        Inst = this;
        Manager.Inst.PerFrameUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        Manager.Inst.PerFrameUpdate();
    }
}
