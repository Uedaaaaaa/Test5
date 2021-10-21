using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーが進める方向
public enum Move
{
    None,
    Left,
    Right,
    Up,
    Down,
}

//マス目のクラス
public class Square
{
    //自身の所持するID
    public int MyID;
    //プレイヤーに伝える次に進める方向
    public Move MyMove;
    //初期値設定
    public Square()
    {
        this.MyID = 0;
        this.MyMove = Move.None;
    }
    //自身のポジション
    public Vector3 MyPos;
}
public class CreateMap : MonoBehaviour
{
    //インスタンス作成時に参照するオブジェクト
    public GameObject HalloweenPrefab;
    public GameObject MinusPrefab;
    public GameObject PlusPrefab;
    public GameObject QuizPrefab;

    //マップ作成用二次配列
    int[][] MapData = new int[8][];

    //マップ作成配列
    //int[] MapData = new int[26];

    //オブジェクトの向き
    Quaternion Left = Quaternion.Euler(0, 270, 0);
    Quaternion Right = Quaternion.Euler(0, 90, 0);
    Quaternion None = Quaternion.Euler(0, 0, 0);

    //それぞれのマスに入れるマスのID
    [System.NonSerialized]
    public int SquareID = 1;

    //マップとして作成するオブジェクト
    [System.NonSerialized]
    public GameObject[] MapObject = new GameObject[64];

    //オブジェクトを作成するポジション
    public Vector3 SetPos;

    //作成するインスタンスの格納用オブジェクト
    GameObject SetSquare;

    //マス目クラス
    public Square[] squares = new Square[64];

    //プレイヤーに次に進む方向を指示
    [System.NonSerialized]
    public Move move = Move.None;

    //インスペクタ上でIDと向きが正しいかデバッグ用
    //SquareData squareData;
        
    // Start is called before the first frame update
    void Start()
    {
        SetPos = this.gameObject.transform.position;

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
                //オブジェクトの向きと
                //マス目クラスに格納するプレイヤーの進む向きをセット
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
                        //インスタンス作成するオブジェクトをプレハブから獲得
                        case 1:
                            SetSquare = HalloweenPrefab;
                            SetSquare.gameObject.name = SquareID.ToString();
                            break;
                        case 2:
                            SetSquare = MinusPrefab;
                            SetSquare.gameObject.name = SquareID.ToString();
                            break;
                        case 3:
                            SetSquare = PlusPrefab;
                            SetSquare.gameObject.name = SquareID.ToString();
                            break;
                        case 4:
                            SetSquare = QuizPrefab;
                            SetSquare.gameObject.name = SquareID.ToString();
                            break;
                    }
                    if(SquareID == 1)
                    {
                        SetSquare.gameObject.name += "Start";
                    }

                    //インスタンスオブジェクトを作成し、子構造にする
                    MapObject[SquareID-1] = Instantiate(SetSquare,
                                new Vector3(-20 * (MapData[i].Length - 1 - j), 0, 20 * (MapData.Length - 1 - i)), Set);
                    MapObject[SquareID-1].transform.parent = this.transform;

                    //マス目クラスを作成
                    squares[SquareID-1] = new Square();
                    squares[SquareID-1].MyID = SquareID;
                    squares[SquareID-1].MyMove = move;

                    //MyIDとMyMoveをインスペクタ上で見るためのデバッグ用処理
                    //squareData = MapObject[SquareID].GetComponent<SquareData>();
                    //squareData.SetID(squares[SquareID].MyID, squares[SquareID].MyMove);
                }
                else
                {
                    MapObject[SquareID] = null;
                }
                SquareID++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
