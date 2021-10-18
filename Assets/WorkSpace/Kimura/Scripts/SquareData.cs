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
    public int MapID = 0;//マップ生成時にセットされる自身のID
    public Move MyMove = Move.None;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void SetID(int ID,Move move)
    {
        MapID = ID;
        MyMove = move;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
