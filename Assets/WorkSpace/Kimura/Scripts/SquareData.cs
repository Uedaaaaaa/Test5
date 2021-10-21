using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;


public class SquareData : MonoBehaviour
{
    public int MapID = 0;//マップ生成時にセットされる自身のID
    public Move MyMove = Move.None;
    public Vector3 MyPos;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void SetID(int ID,Move move ,Vector3 pos)
    {
        MapID = ID;
        MyMove = move;
        MyPos = pos;
    }
    public void OnTriggerStay(Collider other)
    {
        Debug.Log("Hit");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
