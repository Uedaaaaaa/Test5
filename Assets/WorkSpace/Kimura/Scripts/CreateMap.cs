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
    public bool[] waitPosFlg = new bool[4];
    //自身の所持するID
    public int MyID;
    //プレイヤーに伝える次に進める方向
    public Move MyMove;
    //初期値設定
    public Square()
    {
        this.MyID = 0;
        this.MyMove = Move.None;
        for(int i = 0;i < waitPosFlg.Length; i++)
        {
            waitPosFlg[i] = false;
        }
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

    //マップ作成配列
    int[] MapData = new int[28];

    //オブジェクトの向き
    Quaternion Left = Quaternion.Euler(0, 270, 0);
    Quaternion Right = Quaternion.Euler(0, 90, 0);
    Quaternion None = Quaternion.Euler(0, 0, 0);

    //それぞれのマスに入れるマスのID
    [System.NonSerialized]
    public int SquareID = 1;

    //マップとして作成するオブジェクト
    [System.NonSerialized]
    public GameObject[] MapObject = new GameObject[28];

    //オブジェクトを作成するポジション
    [System.NonSerialized]
    public Vector3 SetPos;

    //作成するインスタンスの格納用オブジェクト
    GameObject SetSquare;

    //マス目クラス
    public Square[] squares = new Square[28];

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
        MapData =
            new[] { 3, 2, 1, 4, 3, 2, 4,
                1, 3, 4, 1, 2, 4, 3,
                1, 2, 3, 4, 1, 3, 2,
                1, 3, 4, 3, 1, 4, 2, 3 };
            //new[] { 3, 3, 2, 3, 4, 3, 1,
        //2, 4, 3, 3, 2, 3, 1,
        //3, 3, 4, 2, 3, 3, 2,
        //1, 3, 4, 3, 1, 4, 2, 3 };
        SetPos = new Vector3(20,-0.7f,0);
        Quaternion Set;
        for (int i = 1; i < MapData.Length; i++)
        {
            //オブジェクトの向きとポジション、
            //マス目クラスに格納するプレイヤーの進む向きをセット
            SetSquare = null;
            Set = None;
            move = Move.None;
            if(i <= 8)
            {
                SetPos.x -= 20;
            }
            if(i >= 9 && i <= 15)
            {
                SetPos.z += 20;
            }
            if(i > 15 && i <= 22)
            {
                SetPos.x += 20;
            }
            if(i > 22)
            {
                SetPos.z -= 20;
            }

            //進む方向をセットする
            if(i < 8)
            {
                move = Move.Left;
            }
            else if(i >= 8 && i < 15)
            {
                move = Move.Up;
            }
            else if(i >= 15 && i < 22)
            {
                move = Move.Right;
            }
            else if(i >= 22)
            {
                move = Move.Down;
            }
            if (i >= 8 && i < 15)
            {
                Set = Left;
            }
            else if (i == 1 || i > 22)
            {
                Set = Right;
            }

            //オブジェクトのインスタンスを生成
            switch (MapData[i - 1])
            {
                //インスタンス作成するオブジェクトをプレハブから獲得
                case 1:
                    SetSquare = HalloweenPrefab;
                    break;
                case 2:
                    SetSquare = MinusPrefab;
                    break;
                case 3:
                    SetSquare = PlusPrefab;
                    break;
                case 4:
                    SetSquare = QuizPrefab;
                    break;
            }
            //作成するオブジェクトの名前を変更
            if(i >= 10)
            {
                SetSquare.gameObject.name = "2" + i.ToString();
            }
            else
            {
                SetSquare.gameObject.name = "1" + i.ToString();
            }
            if (i == 1)
            {
                SetSquare.gameObject.name += "Start";
            }
            //オブジェクトのインスタンスを作成
            MapObject[i - 1] = Instantiate(SetSquare, SetPos, Set);
            MapObject[i - 1].transform.parent = this.transform;
            //マス目クラスを作成
            squares[i - 1] = new Square();
            //マス目クラスに今回作成したオブジェクトの情報を与える
            squares[i - 1].MyID = i;
            squares[i - 1].MyMove = move;
            squares[i - 1].MyPos = SetPos;
            //MyIDとMyMoveをインスペクタ上で見るためのデバッグ用処理
            //squareData = MapObject[i - 1].GetComponent<SquareData>();
            //squareData.SetID(squares[i - 1].MyID, squares[i - 1].MyMove, squares[i - 1].MyPos);
        }
    }

    public void SetWaitPosFlg(int num,int waitNum,bool flg)
    {
        squares[num].waitPosFlg[waitNum] = flg;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
