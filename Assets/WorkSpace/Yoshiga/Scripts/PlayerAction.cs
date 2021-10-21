using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{    
    private GameManager manager;    //ゲームマネージャー
    private int MyNo;               //自身の番号
    private CreateMap MapScript;    //マップの生成script
    private bool StartFlg = false;  //スタート地点に向かうためのフラグ
    private Vector3 StartDashVelocity;  //スタートダッシュの時の速度
    private bool MoveFlg = false;       //動いていいかのフラグ
    [HideInInspector]public MassType StopMass;       //止まったマスのタイプ
    private Rigidbody MyRB;             //自身のRigidbody
    
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
        }
        else if(other.gameObject.tag == "Minus")
        {
            StopMass = MassType.Minus;
            manager.MinusDiceNo();
        }
        else if(other.gameObject.tag == "Halloween")
        {
            StopMass = MassType.Halloween;
            manager.MinusDiceNo();
        }
        else if(other.gameObject.tag == "Quiz")
        {
            StopMass = MassType.Quiz;
            manager.MinusDiceNo();
        }
    }

    private void FixedUpdate()
    {
        //順番決めが終わっていてMoveflgがtrueの時
        if(MyNo == manager.OrderArray[manager.NowPlayerNo] && StartFlg == false && manager.GameStatus == GameSTS.Play)
        {
            MyRB.velocity = StartDashVelocity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
