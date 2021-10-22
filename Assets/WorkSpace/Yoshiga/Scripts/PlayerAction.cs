using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{    
    private GameManager manager;    //ゲームマネージャー
    private int MyNo;               //自身の番号
    private CreateMap MapScript;    //マップの生成script
    private bool StartFlg = true;  //スタート地点に向かうためのフラグ
    private Vector3 StartDashVelocity;  //スタートダッシュの時の速度
    private bool MoveFlg = false;       //動いていいかのフラグ
    [HideInInspector]public MassType StopMass;       //止まったマスのタイプ
    private Rigidbody MyRB;             //自身のRigidbody
    private int NowMassNo;              //今いるマスの番号
    
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
        MyNo = int.Parse(gameObject.name.Substring(0, 1));
        MapScript = GameObject.FindGameObjectWithTag("Map").GetComponent<CreateMap>();
        StartDashVelocity = new Vector3(-transform.position.x,0.0f,-transform.position.z).normalized * manager.CharacterSpeed;
        MyRB = this.gameObject.GetComponent<Rigidbody>();
    }

    //MoveFlgを変更する処理
    public void SetMoveFlg(bool Flg)
    {
        MoveFlg = Flg;
    }

    private void OnTriggerEnter(Collider other)
    {
        //プレイヤーが１マス進んだ時の処理
        if(other.gameObject.tag == "Plus")
        {
            StopMass = MassType.Plus;
            manager.MinusDiceNo();
            NowMassNo = int.Parse(other.gameObject.name.Substring(0, 1));
        }
        else if(other.gameObject.tag == "Minus")
        {
            StopMass = MassType.Minus;
            manager.MinusDiceNo();
            NowMassNo = int.Parse(other.gameObject.name.Substring(0, 1));
        }
        else if(other.gameObject.tag == "Halloween")
        {
            StopMass = MassType.Halloween;
            manager.MinusDiceNo();
            NowMassNo = int.Parse(other.gameObject.name.Substring(0, 1));
        }
        else if(other.gameObject.tag == "Quiz")
        {
            StopMass = MassType.Quiz;
            manager.MinusDiceNo();
            NowMassNo = int.Parse(other.gameObject.name.Substring(0, 1));
        }

        //止まりたいマスについた時
        if(manager.characters[manager.OrderArray[manager.NowPlayerNo]].MyDiceNo == 0)
        {
            MoveFlg = false;
            MyRB.velocity = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        //キャラクターが動いていい時の処理
        if(MoveFlg && MyNo == manager.OrderArray[manager.NowPlayerNo] && manager.GameStatus == GameSTS.Play)
        {
            //キャラクターを動かす処理
            if (StartFlg == true)
            {
                MyRB.velocity = StartDashVelocity;
            }
            else
            {
                switch (MapScript.squares[NowMassNo].MyMove)
                {
                    case Move.Down:
                        break;
                    case Move.Left:
                        break;
                    case Move.Right:
                        break;
                    case Move.None:
                        break;
                }

            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
