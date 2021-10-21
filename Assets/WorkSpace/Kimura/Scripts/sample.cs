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
    CreateMap square;
    GameObject map;
    public int DebugID;
    public Move DebugMove;
    // Start is called before the first frame update
    void Start()
    {
        map = GameObject.FindGameObjectWithTag("Map");
        square = map.GetComponent<CreateMap>();
    }

    // Update is called once per frame
    void Update()
    {
        DebugID = square.squares[7].MyID;
        DebugMove = square.squares[7].MyMove;
    }
}
