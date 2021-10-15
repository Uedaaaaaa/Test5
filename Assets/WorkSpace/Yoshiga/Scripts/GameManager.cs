using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}

public class GameManager : MonoBehaviour
{
    //キャラクター配列
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
    private bool Ordering;
    //順番を保存しておくための変数
    private GameObject[] OrderArray = new GameObject[4];


    // Start is called before the first frame update
    void Start()
    {
        Ordering = true;
        NowPlayerNo = 0;
        //キャラクタークラスを作成し、Rigidbodyを格納
        for (int i = 0; i < characters.Length; ++i)
        {
            characters[i] = new Character();
            OrderArray[i] = CharacterObj[i];
            characters[i].MyNo = i + 1;
            characters[i].MyRB = CharacterObj[i].GetComponent<Rigidbody>();
        }
    }

    //ダイスの目を保存する処理
    public void SetDiceNo(int No)
    {
        if(Ordering == true)
        {
            characters[NowPlayerNo].MyDiceNo = No;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
