using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexa_move : MonoBehaviour
{
    float time;
    void Start()
    {
        time = 0;
    }

    void Update()
    {
        time += Time.deltaTime;
        float z = Mathf.PerlinNoise(time, 0);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, z);
    }
}
