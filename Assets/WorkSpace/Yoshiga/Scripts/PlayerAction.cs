using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;

public class PlayerAction : MonoBehaviour
{    
    private GameManager manager;    //ゲームマネージャー
    private int myNo;               //自身の番号
    private CreateMap mapScript;    //マップの生成script
    private bool startFlg = true;  //スタート地点に向かうためのフラグ
    private Vector3 startDashVelocity;  //スタートダッシュの時の速度
    [HideInInspector]public bool moveFlg = false;       //動いていいかのフラグ
    [HideInInspector]public MassType stopMass;       //止まったマスのタイプ
    private Rigidbody myRB;             //自身のRigidbody
    [HideInInspector] public int nowMassNo = 0;    //今いるマスの番号
    private int massHeadNo;     //今いるマスの頭の数字(そのマスの番号が何桁の数字か)
    private Animator myAnim;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
        myNo = int.Parse(gameObject.name.Substring(0, 1)) - 1;
        mapScript = GameObject.FindGameObjectWithTag("Map").GetComponent<CreateMap>();
        startDashVelocity = new Vector3(-transform.position.x,0.0f,-transform.position.z).normalized * manager.CharacterSpeed;
        myRB = this.gameObject.GetComponent<Rigidbody>();
        myAnim = this.gameObject.GetComponent<Animator>();
    }

    //MoveFlgを変更する処理
    public void SetMoveFlg(bool Flg)
    {
        moveFlg = Flg;
        myAnim.SetBool("Move", Flg);
    }

    private void OnTriggerEnter(Collider other)
    {
        // SEを鳴らす
        SEManager.Instance.Stop(SEPath.DICE_COUNT_MINUS);
        SEManager.Instance.Play(SEPath.DICE_COUNT_MINUS,1.2f);

        if(myNo == manager.OrderArray[manager.NowPlayerNo])
        {
            //プレイヤーが１マス進んだ時の処理
            if (other.gameObject.tag == "Plus")
            {
                stopMass = MassType.Plus;
            }
            else if (other.gameObject.tag == "Minus")
            {
                stopMass = MassType.Minus;
            }
            else if (other.gameObject.tag == "Halloween")
            {
                stopMass = MassType.Halloween;
            }
            else if (other.gameObject.tag == "Quiz")
            {
                stopMass = MassType.Quiz;
            }

            manager.MinusDiceNo();
            massHeadNo = int.Parse(other.gameObject.name.Substring(0, 1));
            if (massHeadNo == 1)
            {
                nowMassNo = int.Parse(other.gameObject.name.Substring(1, 1));
            }
            else if (massHeadNo == 2)
            {
                nowMassNo = int.Parse(other.gameObject.name.Substring(1, 2));
            }

            if (nowMassNo == 1)
            {
                startFlg = false;
            }

            //止まりたいマスについた時
            if (manager.characters[manager.OrderArray[manager.NowPlayerNo]].myDiceNo == 0)
            {
                manager.CharacerUI.DiceNumUIDestroy();
                SetMoveFlg(false);
                myRB.velocity = Vector3.zero;
                this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                manager.PlayEvent(stopMass);
            }
        }       
    }

    public void SetStartPos()
    {
        if(startFlg == false)
        {
            this.gameObject.transform.position = mapScript.squares[nowMassNo - 1].MyPos;
        }
        
    }

    private void FixedUpdate()
    {
        //キャラクターが動いていい時の処理
        if(moveFlg == true && myNo == manager.OrderArray[manager.NowPlayerNo] && manager.gameStatus == GameSTS.Play)
        {
            manager.CharacerUI.DiceNumUISet(manager.characters[manager.OrderArray[manager.NowPlayerNo]].myDiceNo);

            //キャラクターを動かす処理
            if (startFlg == true)
            {
                myRB.velocity = startDashVelocity;
                this.gameObject.transform.LookAt(new Vector3(0, 0, 0));
            }
            else
            {
                switch (mapScript.squares[nowMassNo - 1].MyMove)
                {
                    case Move.Down:
                        myRB.velocity = new Vector3(0, 0, -1) * manager.CharacterSpeed;
                        this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                        break;
                    case Move.Up:
                        myRB.velocity = new Vector3(0, 0, 1) * manager.CharacterSpeed;
                        this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                        break;
                    case Move.Left:
                        myRB.velocity = new Vector3(-1, 0, 0) * manager.CharacterSpeed;
                        this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 270.0f, 0.0f);
                        break;
                    case Move.Right:
                        myRB.velocity = new Vector3(1, 0, 0) * manager.CharacterSpeed;
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
