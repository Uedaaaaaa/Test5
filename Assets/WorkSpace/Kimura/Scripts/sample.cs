using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;

public enum move
{
    none,
    left,
    right,
    up,
    down,
}

public class sample : MonoBehaviour
{
    public move mymove = move.none;
    // Start is called before the first frame update
    void Start()
    {
        SEManager.Instance.Play(SEPath.SYSTEM20);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
