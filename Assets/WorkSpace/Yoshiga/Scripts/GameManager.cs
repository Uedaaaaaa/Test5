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
    public int myNo;        //キャラクターの番号                    
    public int candy;       //キャラクターが持っている飴の個数    
    public int yaruki;      //キャラクターのやる気   
    public int myDiceNo;    //現在のターンに自身が出したダイスの目
    public bool eventFlg;   //キャラクターがマスのイベントを行っているかのフラグ
    public int rank;        //最終ランキング
    public int waitNo;      //待機場所の番号

    //キャラクターの初期値設定の関数
    public Character()
    {
        this.candy = 0;
        this.yaruki = 5;
        this.myDiceNo = 0;
        this.eventFlg = false;
        this.rank = 1;
    }
}

public class GameManager : MonoBehaviour
{
    [HideInInspector]public GameSTS gameStatus;    //ゲームステータス
    [Header("サイコロ : オブジェクト")]
    [SerializeField] private GameObject DiceObj;    
    [HideInInspector] public Character[] characters = new Character[4];  //キャラクタークラス配列(charactors[0]が1P)
    [Header("キャラクターの速さ")]
    public float CharacterSpeed;
    [Header("キャラクター : オブジェクト")]
    [SerializeField] private GameObject[] CharacterObj = new GameObject[4];
    [Header("ターン数 : 全員がサイコロを振って1ターン")]
    public int GameTurn;    
    [HideInInspector] public int NowPlayerNo;                   //現在のターンで何番目の人のターンかの変数
    [HideInInspector] public int[] OrderArray = new int[4];     //順番を保存しておくための変数  
    private bool FinishDiceFlg = false;                         //ダイスを振ったかどうかの確認フラグ    
    [HideInInspector] public int[] OrderjudgeNo = new int[4];   //順番決めで出たダイスの目を保存する変数
    [Header("プレイヤー固定カメラのプレイヤーからの距離")]
    [SerializeField] private Vector3 PlayerCameraPos;
    [Header("カメラの傾き : X軸")]
    [SerializeField] private float CameraXaxis;
    private PlayerAction[] playerScript = new PlayerAction[4];   //プレイヤーごとのPlayerScript
    [Header("CharactorUICanvas : オブジェクト")]
    public SetCharacerUI CharacerUI;
    private bool statusUIActive;    //キャラクターのステータスUIが表示されているかのフラグ
    [Header("EventController : オブジェクト")]
    [SerializeField] private GameObject eventController;
    private SpuareAction eventScript;
    private bool sortFlg = false;
    private int waitNo2 = 0;
    private CreateMap mapScript;    //マップの生成script

    // Start is called before the first frame update
    void Start()
    {
        statusUIActive = false;
        gameStatus = GameSTS.OrderJudge;
        NowPlayerNo = 0;
        //キャラクタークラスを作成し、Rigidbodyを格納
        for (int i = 0; i < characters.Length; ++i)
        {
            characters[i] = new Character();
            OrderArray[i] = i;
            characters[i].myNo = i + 1;
            playerScript[i] = CharacterObj[i].GetComponent<PlayerAction>();
        }
        eventScript = eventController.GetComponent<SpuareAction>();
        mapScript = GameObject.FindGameObjectWithTag("Map").GetComponent<CreateMap>();
    }

    public void CanMove()
    {
        playerScript[OrderArray[NowPlayerNo]].SetMoveFlg(true);
    }

    //キャラクターがマスに止まってイベントを行う時の処理
    public void PlayEvent(MassType EventType)
    {
        characters[OrderArray[NowPlayerNo]].eventFlg = true;

        switch(EventType)
        {
            case MassType.Halloween:
                eventScript.HalloweenEvent(OrderArray[NowPlayerNo]);
                break;
            case MassType.Plus:
                eventScript.PlusEvent(OrderArray[NowPlayerNo]);
                break;
            case MassType.Minus:
                eventScript.MinusEvent(OrderArray[NowPlayerNo]);
                break;
            case MassType.Quiz:
                eventScript.QuizEvent(OrderArray[NowPlayerNo]);
                break;
        }
    }

    // キャラクターの位置を調整
    public void ChangePos()
    {
        int Samesquare = 0;

        //同じマスに何人いるのか確認する処理
        for (int i = 0; i < characters.Length; ++i)
        {
            if (playerScript[OrderArray[NowPlayerNo]].nowMassNo == playerScript[OrderArray[i]].nowMassNo)
            {
                Samesquare++;
            }
        }

        for(int i = 0; i <= Samesquare; ++i)
        {
            if (mapScript.squares[playerScript[OrderArray[NowPlayerNo]].nowMassNo - 1].waitPosFlg[i] == false)
            {
                //　待機場所に移動
                playerScript[OrderArray[NowPlayerNo]].SetWaitPos(i);
                mapScript.SetWaitPosFlg(playerScript[OrderArray[NowPlayerNo]].nowMassNo - 1, i, true);
                characters[OrderArray[NowPlayerNo]].waitNo = i;
                return;
            }
        }
    }

    // 自身がどこの待機場所にいるのか
    public void SetWaitNo(int No)
    {
        characters[OrderArray[NowPlayerNo]].waitNo = No;
    }

    //キャラクターがイベントを終えたときに呼ばれる関数
    public void EndEvent()
    {
        characters[OrderArray[NowPlayerNo]].eventFlg = false;
        ChangeNowPlayerNo();
        if(gameStatus == GameSTS.Play)
        {
            SpawnDice();
        }    
    }

    //現在のターンで何番目の人のターンかの変数を変える
    public void ChangeNowPlayerNo()
    {
        if (NowPlayerNo < 3)
        {
            NowPlayerNo++;
        }
        else if(NowPlayerNo >= 3)
        {
            //全員が終わったらゲームの残りターンを減らす
            GameTurn--;
            //もしターンが0以下になった時ゲームを終わる
            if(GameTurn <= 0)
            {
                gameStatus = GameSTS.Ranking;
                CharacerUI.GameEndUISet();
                return;
            }
            NowPlayerNo = 0;
        }
    }

    //ゲーム終了関数
    public void GameFinish()
    {
        for(int i = 0; i < 4; ++i)
        {
            CharacterObj[i].transform.position = new Vector3((i * 7.0f) - 10.0f, 0.0f, -20.0f);
        }

        this.gameObject.transform.position = new Vector3(0.5f, 5.0f, -35.0f);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        int LowestRank = 1; // 最低順位が何位か 
        for (int i = 0; i < 4; ++i)
        {
            for(int j = 0; j < 4; ++j)
            {
                if (characters[i].candy < characters[j].candy && i != j)
                {
                    characters[i].rank++;
                    // 最低順位を更新
                    if(characters[i].rank > LowestRank)
                    {
                        LowestRank = characters[i].rank;
                    }
                }
            }  
        }

        if(LowestRank != 1)
        {
            for (int i = 0; i < 4; ++i)
            {
                // 自身のランクが最低順位と同じなら四位にする
                if (characters[i].rank == LowestRank)
                {
                    characters[i].rank = 4;
                }
            }
        }
    }

    //プレイヤーが進んだ時に進める回数を減らす処理
    public void MinusDiceNo()
    {
        characters[OrderArray[NowPlayerNo]].myDiceNo--;
    }

    //ダイスの目を保存する処理
    public void SetDiceNo(int No)
    {        
        characters[OrderArray[NowPlayerNo]].myDiceNo = No;

        //順番決めの時
        if(gameStatus == GameSTS.OrderJudge)
        {
            OrderjudgeNo[NowPlayerNo] = No;
            OrderArray[NowPlayerNo] = No;
        }
       
        FinishDiceFlg = true;       
    }

    //サイコロ生成処理
    public void SpawnDice()
    {
        if(gameStatus == GameSTS.Play)
        {
            playerScript[OrderArray[NowPlayerNo]].SetStartPos();
            if(statusUIActive == false)
            {
                CharacerUI.PlayerStatusUISet();
                statusUIActive = true;
            }

            CharacerUI.PlayerTurnUISet(OrderArray[NowPlayerNo]);
        }
        else
        {
            CharacerUI.DiceStartUISet();
        }

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
        if(gameStatus == GameSTS.Play)
        {
            Vector3 NewPos = Vector3.Lerp(
                                transform.position, //現状のカメラ位置
                                CharacterObj[OrderArray[NowPlayerNo]].transform.position + PlayerCameraPos, //行きたいカメラ位置
                                Time.fixedDeltaTime * 3.0f); //その差の割合（0～1）

            //カメラの位置と角度の調整
            transform.position = NewPos;
            
            //カメラがプレイヤーについていくようにする
            if(playerScript[OrderArray[NowPlayerNo]].moveFlg == true)
            {
                transform.position = new Vector3(CharacterObj[OrderArray[NowPlayerNo]].transform.position.x, 
                                                 NewPos.y, 
                                                 CharacterObj[OrderArray[NowPlayerNo]].transform.position.z + PlayerCameraPos.z);
            }
        }
    }

    public void GameStart()
    {
        gameStatus = GameSTS.Play;
        transform.rotation = Quaternion.Euler(CameraXaxis, 0, 0);
        SpawnDice();
    }

    // Update is called once per frame
    void Update()
    {
        //順番決めのダイスを全員が振り終わった時
        if(gameStatus == GameSTS.OrderJudge && NowPlayerNo == 3 && characters[3].myDiceNo != 0)
        {
            //順番決めの配列をソート
            Array.Sort(OrderArray);
            Array.Reverse(OrderArray);
            sortFlg = true;

            for (int i = 0; i < 4; ++i)
            {                
                for (int j = 0;j < 4; ++j)
                {
                    if(OrderArray[i] == characters[j].myDiceNo)
                    {
                        OrderArray[i] = j;
                        break;
                    }
                }
            }

            //順番決めのターン終了
            eventScript.ChangeEndDice();
            NowPlayerNo = 0;
        }

        //ダイスを出現させるための処理
        if (FinishDiceFlg == true && sortFlg == false)
        {
            if(gameStatus == GameSTS.OrderJudge)
            {
                Invoke("SpawnDice", 2);
                ChangeNowPlayerNo();
            }
            FinishDiceFlg = false;
        }
    }
}
