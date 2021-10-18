using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//キャラクタークラス
public class Character
{
    //キャラクターナンバー
    public int MyNo;
    //キャラクターのRigidbody
    public Rigidbody MyRB;
    //キャラクターが持っている飴の個数
    public int Candy;
    //キャラクターのやる気
    public int Yaruki;
    //現在のターンに自身が出したダイスの目
    public int MyDiceNo;
   
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
    [Header("サイコロ : オブジェクト")]
    [SerializeField]
    private GameObject DiceObj;
    //キャラクタークラス配列
    [HideInInspector]
    public Character[] characters = new Character[4];
    [Header("キャラクター : オブジェクト")]
    [SerializeField]
    private GameObject[] CharacterObj = new GameObject[4];
    [Header("ターン数 : 全員がサイコロを振って1ターン")]
    public int GameTurn;
    //現在誰のターンかの変数
    [HideInInspector]
    public int NowPlayerNo;
    //順番決めをしているかのフラグ
    [HideInInspector]
    public bool Ordering;
    //順番を保存しておくための変数
    private int[] OrderArray = new int[4];
    //ダイスを振ったかどうかの確認フラグ
    private bool FinishDiceFlg = false;
    //順番決めで出たダイスの目を保存する変数
    [HideInInspector]
    public int[] OrderjudgeNo = new int[4];
    
    // Start is called before the first frame update
    void Start()
    {
        Ordering = true;
        NowPlayerNo = 0;
        //キャラクタークラスを作成し、Rigidbodyを格納
        for (int i = 0; i < characters.Length; ++i)
        {
            characters[i] = new Character();
            OrderArray[i] = i;
            characters[i].MyNo = i + 1;
            characters[i].MyRB = CharacterObj[i].GetComponent<Rigidbody>();
        }
        SpawnDice();
    }

    //ダイスの目を保存する処理
    public void SetDiceNo(int No)
    {        
        characters[NowPlayerNo].MyDiceNo = No;

        //順番決めの時
        if(Ordering == true)
        {
            OrderjudgeNo[NowPlayerNo] = No;
            OrderArray[NowPlayerNo] = No;
        }
              
        FinishDiceFlg = true;
    }

    //サイコロ生成処理
    private void SpawnDice()
    {          
        Instantiate(DiceObj, new Vector3(CharacterObj[OrderArray[NowPlayerNo]].transform.position.x, CharacterObj[OrderArray[NowPlayerNo]].transform.position.y + 10, CharacterObj[OrderArray[NowPlayerNo]].transform.position.z), CharacterObj[OrderArray[NowPlayerNo]].transform.rotation);       
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

            //for (int i = 0; i < 4; ++i)
            //{
            //    Debug.Log("変更前 = " + OrderArray[i]);             
            //}
            //for (int i = 0; i < 4; ++i)
            //{
            //    Debug.Log(i + "のダイスNo = " + characters[i].MyDiceNo);
            //}

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

            //for (int i = 0; i < 4; ++i)
            //{
            //    Debug.Log("変更後の"+ i + " = " +OrderArray[i]);
            //}

            Ordering = false;
        }

        //ダイスを出現させる処理
        if(FinishDiceFlg == true)
        {
            Invoke("SpawnDice", 2);
            if (NowPlayerNo < 3)
            {
                NowPlayerNo++;
            }
            else
            {
                NowPlayerNo = 0;
                GameTurn--;
            }
            FinishDiceFlg = false;
        }
    }
}
