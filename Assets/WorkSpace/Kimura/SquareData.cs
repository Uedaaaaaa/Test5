using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;

public enum Move
{
    None,
    Left,
    Right,
    Up,
    Down,
}

public class SquareData : MonoBehaviour
{
    public int MapID;//マップ生成時にセットされる自身のID
    public Move MyMove;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
