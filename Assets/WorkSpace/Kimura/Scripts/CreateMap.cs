using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    public GameObject HalloweenPrehub;
    public GameObject MinusPrehub;
    public GameObject PlusPrehub;
    public GameObject QuizPrehub;
    int[][] MapData = new int[8][];
    Quaternion Left = Quaternion.Euler(0, 270, 0);
    Quaternion Right = Quaternion.Euler(0, 90, 0);
    Quaternion None = Quaternion.Euler(0, 0, 0);
    [System.NonSerialized]
    public int SquareID = 0;
    GameObject[] MapObject = new GameObject[64];
    GameObject SetSquare;
    SquareData squareData ;
    [System.NonSerialized]
    public Move move = Move.None;
    // Start is called before the first frame update
    void Start()
    {
        //Nothing:0
        //Halloween:1
        //Minus:2
        //Plus:3
        //Quis:4
        MapData[0] = new[] { 3, 3, 4, 2, 3, 3, 2, 1 };
        MapData[1] = new[] { 1, 0, 0, 0, 0, 0, 0, 3 };
        MapData[2] = new[] { 3, 0, 0, 0, 0, 0, 0, 4 };
        MapData[3] = new[] { 2, 0, 0, 0, 0, 0, 0, 3 };
        MapData[4] = new[] { 3, 0, 0, 0, 0, 0, 0, 1 };
        MapData[5] = new[] { 3, 0, 0, 0, 0, 0, 0, 4 };
        MapData[6] = new[] { 4, 0, 0, 0, 0, 0, 0, 2 };
        MapData[7] = new[] { 2, 1, 3, 4, 3, 2, 3, 3 };

        for (int i = MapData.Length-1; i >= 0; i--)
        {
            for(int j = MapData[i].Length-1; j >= 0; j--)
            {
                //オブジェクトの向きをセット
                Quaternion Set;
                SetSquare = null;
                move = Move.None;
                if(j == 0)
                {
                    Set = Left;
                    if(i == 0)
                    {
                        move = Move.Right;
                    }
                    else if(i == MapData.Length -1)
                    {
                        move = Move.Up;
                    }
                }
                else if(j == MapData[i].Length-1)
                {
                    Set = Right;
                    if(i == 0)
                    {
                        move = Move.Down;
                    }
                    if(i == MapData.Length -1)
                    {
                        move = Move.Left;
                    }
                }
                else
                {
                    Set = None;
                }

                //オブジェクトのインスタンスを生成
                if(MapData[i][j] != 0)
                {
                    switch (MapData[i][j])
                    {
                        case 1:
                            SetSquare = HalloweenPrehub;
                            //squareData = HalloweenPrehub.GetComponent<SquareData>();
                            break;
                        case 2:
                            SetSquare = MinusPrehub;
                            //squareData = MinusPrehub.GetComponent<SquareData>();
                            break;
                        case 3:
                            SetSquare = PlusPrehub;
                            //squareData = PlusPrehub.GetComponent<SquareData>();
                            break;
                        case 4:
                            SetSquare = QuizPrehub;
                            //squareData = QuizPrehub.GetComponent<SquareData>();
                            break;
                    }
                    MapObject[SquareID] = Instantiate(SetSquare,
                                new Vector3(-20 * (MapData[i].Length - 1 - j), 0, 20 * (MapData.Length - 1 - i)), Set);
                    MapObject[SquareID].transform.parent = this.transform;
                    squareData = MapObject[SquareID].GetComponent<SquareData>();
                    squareData.SetID(SquareID,move);
                    //squareData[SquareID].MapID = SquareID;
                    //squareData[SquareID].MyMove = move;
                    //((MapData[i].Length - 1) - j) + ((MapData.Length - 1) - i);
                }
                else
                {
                    MapObject[SquareID] = null;
                }
                //squareData.MapID = SquareID + 1;
                //squareData.MyMove = Move.None;
                SquareID++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
