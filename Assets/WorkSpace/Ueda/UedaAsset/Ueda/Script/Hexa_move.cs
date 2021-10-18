using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexa_move : MonoBehaviour
{
    float time = 2f;
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 10 + Mathf.Sin(Time.frameCount * time));
    }
}
