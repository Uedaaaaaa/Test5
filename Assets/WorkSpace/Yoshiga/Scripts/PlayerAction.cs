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
    [HideInInspector]public int NowMassNo = 0;    //今いるマスの番号
    
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
        MyNo = int.Parse(gameObject.name.Substring(0, 1)) - 1;
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
        if(MyNo == manager.OrderArray[manager.NowPlayerNo])
        {
            //プレイヤーが１マス進んだ時の処理
            if (other.gameObject.tag == "Plus")
            {
                StopMass = MassType.Plus;
                manager.MinusDiceNo();
                NowMassNo = int.Parse(other.gameObject.name.Substring(0, 1));
            }
            else if (other.gameObject.tag == "Minus")
            {
                StopMass = MassType.Minus;
                manager.MinusDiceNo();
                NowMassNo = int.Parse(other.gameObject.name.Substring(0, 1));
            }
            else if (other.gameObject.tag == "Halloween")
            {
                StopMass = MassType.Halloween;
                manager.MinusDiceNo();
                NowMassNo = int.Parse(other.gameObject.name.Substring(0, 1));
            }
            else if (other.gameObject.tag == "Quiz")
            {
                StopMass = MassType.Quiz;
                manager.MinusDiceNo();
                NowMassNo = int.Parse(other.gameObject.name.Substring(0, 1));
            }

            if (NowMassNo == 1)
            {
                StartFlg = false;
            }

            //止まりたいマスについた時
            if (manager.characters[manager.OrderArray[manager.NowPlayerNo]].MyDiceNo == 0)
            {
                manager.CharacerUI.DiceNumUIDestroy();
                MoveFlg = false;
                MyRB.velocity = Vector3.zero;
                this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                manager.PlayEvent(StopMass);
            }
        }       
    }

    public void SetStartPos()
    {
        if(StartFlg == false)
        {
            this.gameObject.transform.position = MapScript.squares[NowMassNo - 1].MyPos;
        }
        
    }

    private void FixedUpdate()
    {
        //キャラクターが動いていい時の処理
        if(MoveFlg == true && MyNo == manager.OrderArray[manager.NowPlayerNo] && manager.gameStatus == GameSTS.Play)
        {
            manager.CharacerUI.DiceNumUISet(manager.characters[manager.OrderArray[manager.NowPlayerNo]].MyDiceNo);

            //キャラクターを動かす処理
            if (StartFlg == true)
            {
                MyRB.velocity = StartDashVelocity;
                this.gameObject.transform.LookAt(new Vector3(0, 0, 0));
            }
            else
            {
                switch (MapScript.squares[NowMassNo - 1].MyMove)
                {
                    case Move.Down:
                        MyRB.velocity = new Vector3(0, 0, -1) * manager.CharacterSpeed;
                        this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                        break;
                    case Move.Up:
                        MyRB.velocity = new Vector3(0, 0, 1) * manager.CharacterSpeed;
                        this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                        break;
                    case Move.Left:
                        MyRB.velocity = new Vector3(-1, 0, 0) * manager.CharacterSpeed;
                        this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 270.0f, 0.0f);
                        break;
                    case Move.Right:
                        MyRB.velocity = new Vector3(1, 0, 0) * manager.CharacterSpeed;
                        this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
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
