using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//キャラクタークラス
public class Character
{
    //キャラクター本体
    private GameObject MyObj;
    //キャラクターのRigidbody
    private Rigidbody MyRB;
    //キャラクターが持っている飴の個数
    public int Candy;
    //キャラクターのやる気
    public int Yaruki;

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

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0;i < characters.Length; ++i)
        {
<<<<<<< Updated upstream
            characters[i].
=======

>>>>>>> Stashed changes
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
