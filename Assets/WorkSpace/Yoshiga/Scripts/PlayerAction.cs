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
    
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
        MyNo = int.Parse(gameObject.name.Substring(0, 1));
        MapScript = GameObject.FindGameObjectWithTag("Map").GetComponent<CreateMap>();
        StartDashVelocity = new Vector3(-transform.position.x,0.0f,-transform.position.z).normalized * manager.CharacterSpeed;
    }

    public void SetMoveFlg(bool Flg)
    {
        MoveFlg = Flg;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Plus")
        {
            StopMass = MassType.Plus;
            manager.characters[manager.OrderArray[manager.NowPlayerNo]].MyDiceNo--;
        }
        else if(other.gameObject.tag == "Minus")
        {
            StopMass = MassType.Minus;
        }
        else if(other.gameObject.tag == "Halloween")
        {
            StopMass = MassType.Halloween;
        }
        else if(other.gameObject.tag == "Quiz")
        {
            StopMass = MassType.Quiz;
        }
    }

    private void FixedUpdate()
    {
        if(MyNo == manager.OrderArray[manager.NowPlayerNo] && StartFlg == false)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
