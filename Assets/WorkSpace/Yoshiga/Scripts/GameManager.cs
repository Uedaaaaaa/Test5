using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//マスの名前の列挙体
public enum MassType
{
    Plus,
    Minus,
    Halloween,
    Quiz
}

//ゲームステータスの列挙体
public enum GameSTS
{
    OrderJudge,
    Play,
    Ranking
}

//キャラクタークラス
public class Character
{
    public int MyNo;        //キャラクターの番号                    
    public int Candy;       //キャラクターが持っている飴の個数    
    public int Yaruki;      //キャラクターのやる気   
    public int MyDiceNo;    //現在のターンに自身が出したダイスの目

    //キャラクターの初期値設定の関数
    public Character()
    {
        this.Candy = 0;
        this.Yaruki = 5;
        MyDiceNo = 0;
    }
}

public class GameManager : MonoBehaviour
{
    [HideInInspector]public GameSTS GameStatus;    //ゲームステータス
    [Header("サイコロ : オブジェクト")]
    [SerializeField] private GameObject DiceObj;    
    [HideInInspector] public Character[] characters = new Character[4];  //キャラクタークラス配列
    [Header("キャラクターの速さ")]
    public float CharacterSpeed;
    [Header("キャラクター : オブジェクト")]
    [SerializeField] private GameObject[] CharacterObj = new GameObject[4];
    [Header("ターン数 : 全員がサイコロを振って1ターン")]
    public int GameTurn;    
    [HideInInspector] public int NowPlayerNo;                   //現在のターンで何番目の人のターンか
    [HideInInspector] public bool Ordering;                     //順番決めをしているかのフラグ
    [HideInInspector] public int[] OrderArray = new int[4];     //順番を保存しておくための変数  
    private bool FinishDiceFlg = false;                         //ダイスを振ったかどうかの確認フラグ    
    [HideInInspector] public int[] OrderjudgeNo = new int[4];   //順番決めで出たダイスの目を保存する変数
    [Header("プレイヤー固定カメラのプレイヤーからの距離")]
    [SerializeField] private Vector3 PlayerCameraPos;
    [Header("カメラの傾き : X軸")]
    [SerializeField] private float CameraXaxis;
    [Header("プレイヤー固定カメラの向きたい高さ")]
    [SerializeField] private float CameraLookY;
    private PlayerAction[] PlayerScript = new PlayerAction[4];   //プレイヤーごとのPlayerScript


    // Start is called before the first frame update
    void Start()
    {
        Ordering = true;
        GameStatus = GameSTS.OrderJudge;
        NowPlayerNo = 0;
        //キャラクタークラスを作成し、Rigidbodyを格納
        for (int i = 0; i < characters.Length; ++i)
        {
            characters[i] = new Character();
            OrderArray[i] = i;
            characters[i].MyNo = i + 1;
            PlayerScript[i] = CharacterObj[i].GetComponent<PlayerAction>();
        }
        SpawnDice();
    }

    //プレイヤーが進んだ時に進める回数を減らす処理
    public void MinusDiceNo()
    {
        characters[OrderArray[NowPlayerNo]].MyDiceNo--;
    }

    //ダイスの目を保存する処理
    public void SetDiceNo(int No)
    {        
        characters[OrderArray[NowPlayerNo]].MyDiceNo = No;

        //順番決めの時
        if(Ordering == true)
        {
            OrderjudgeNo[NowPlayerNo] = No;
            OrderArray[NowPlayerNo] = No;
        }
        else
        {
            PlayerScript[OrderArray[NowPlayerNo]].SetMoveFlg(true);
        }

        FinishDiceFlg = true;       
    }

    //サイコロ生成処理
    private void SpawnDice()
    {
        Instantiate(DiceObj, new Vector3(CharacterObj[OrderArray[NowPlayerNo]].transform.position.x,
                                         CharacterObj[OrderArray[NowPlayerNo]].transform.position.y + 10,
                                         CharacterObj[OrderArray[NowPlayerNo]].transform.position.z),
                        Quaternion.Euler(CharacterObj[OrderArray[NowPlayerNo]].transform.rotation.x,
                                         0.0f,
                                         CharacterObj[OrderArray[NowPlayerNo]].transform.rotation.z));                                               
    }

    private void FixedUpdate()
    {
        //カメラの位置を更新
        if(Ordering == false)
        {
            Vector3 NewPos = Vector3.Lerp(
                                transform.position, //現状のカメラ位置
                                CharacterObj[OrderArray[NowPlayerNo]].transform.position + PlayerCameraPos, //行きたいカメラ位置
                                Time.fixedDeltaTime * 3.0f); //その差の割合（0～1）

            //カメラの位置と角度の調整
            transform.position = NewPos;
            //transform.LookAt(new Vector3(CharacterObj[OrderArray[NowPlayerNo]].transform.position.x,
            //                             CharacterObj[OrderArray[NowPlayerNo]].transform.position.y + CameraLookY,
            //                             CharacterObj[OrderArray[NowPlayerNo]].transform.position.z));
        }
    }

    // Update is called once per frame
    void Update()
    {
        //順番決めのダイスを全員が振り終わった時
        if(Ordering == true && NowPlayerNo == 3 && characters[3].MyDiceNo != 0)
        {
            //順番決めの配列をソート
            Array.Sort(OrderArray);
            Array.Reverse(OrderArray);

            for (int i = 0; i < 4; ++i)
            {                
                for (int j = 0;j < 4; ++j)
                {
                    if(OrderArray[i] == characters[j].MyDiceNo)
                    {
                        OrderArray[i] = j;
                        break;
                    }
                }
            }

            //順番決めのターン終了
            Ordering = false;
            GameStatus = GameSTS.Play;
            transform.rotation = Quaternion.Euler(CameraXaxis, 0, 0);
        }

        //ダイスを出現させるための処理
        if(FinishDiceFlg == true)
        {
            Invoke("SpawnDice", 2);
            if (NowPlayerNo < 3)
            {
                NowPlayerNo++;
            }
            else
            {
                //全員が終わったらゲームの残りターンを減らす
                NowPlayerNo = 0;
                GameTurn--;
            }
            FinishDiceFlg = false;
        }
    }
}
