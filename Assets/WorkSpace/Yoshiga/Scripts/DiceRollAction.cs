using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;

public class DiceRollAction : MonoBehaviour
{    
    private bool DiceRollFlg = false;       //ダイスの回転フラグ T:回転　F:止まる
    [Header("ダイスの回転速度")]
    [SerializeField] private float DiceRollSpeed;
    [Header("ダイスの面 : オブジェクト")]
    [SerializeField] private GameObject[] DiceSides = new GameObject[6];
    [Header("ダイスの面のマテリアル")]
    [SerializeField] private Material[] DiceMats = new Material[6];    
    private MeshRenderer[] DiceSideRenderers = new MeshRenderer[6];  //ダイスの面のrenderer    
    private int DiceNo; //ダイスの目を保存するための変数
    private bool DiceDecisionFlg = false; //回転しているダイスを止めるためのフラグ
    [Header("ダイスの跳ねる速度")]
    [SerializeField] private float JumpSpeed;
    //ジャンプの時の変数
    private float JumpSin;
    private bool JumpFlg;    
    private float JumpPosY;　//ジャンプした時のy振幅
    [Header("ダイスが消えるまでの秒数")]
    [SerializeField] private float DestroyTime;  
    private bool DestroyFlg; //ジャンプして消えるまでのカウントダウンが始まるためのフラグ
    [Header("もくもくエフェクト : オブジェクト")]
    [SerializeField] private GameObject CloudEffect;
    [Header("紙吹雪エフェクト : オブジェクト")]
    [SerializeField] private GameObject ConfettiEffect;
    private GameManager manager;    //GameManagerのScript    
    private int ransuchousei;   //乱数調整用変数

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
        JumpPosY = this.gameObject.transform.position.y;
        for (int i = 0; i < 6;++i)
        {
            DiceSideRenderers[i] = DiceSides[i].GetComponent<MeshRenderer>();
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        //乱数調整用
        ransuchousei = Random.Range(1, 7);

        if (Input.GetButtonDown("BtnB") && JumpFlg == false)
        {
            //ダイスロールを開始
            if (DiceRollFlg == false)
            {
                SEManager.Instance.Play(SEPath.DICE_SPINE, 1, 0, 1, true);
                DiceRollFlg = true;               
                manager.CharacerUI.DiceStopUISet();
                manager.CharacerUI.PlayerTurnUIDestroy();
            }
            else //ダイスロールを止める
            {
                SEManager.Instance.Stop(SEPath.DICE_SPINE);
                SEManager.Instance.Play(SEPath.DICE_STOP);
                DiceDecisionFlg = true;
                manager.CharacerUI.DiceStopUIDestroy();
            }
        }
    }

    private void FixedUpdate()
    {        
        //ダイスが跳ねている時にSinを増やす処理
        if(JumpFlg == true && DestroyFlg == false)
        {
            JumpSin += Time.deltaTime * JumpSpeed;

            if(Mathf.Sin(JumpSin) < 0)
            {
                JumpSin = 0;
                DestroyFlg = true;                         
            }
        }

        //設定した秒数の後にダイスを消す
        if(DestroyFlg == true)
        {
            DestroyTime -= Time.deltaTime;
            if(DestroyTime < 0)
            {
                Instantiate(CloudEffect, this.gameObject.transform.position, gameObject.transform.rotation);
                Instantiate(ConfettiEffect, this.gameObject.transform.position, gameObject.transform.rotation);
                Destroy(this.gameObject);
            }
        }

        //ダイスロールが実行されている時
        if (DiceRollFlg == true)
        {
            //ダイスの回転処理
            this.transform.Rotate(new Vector3(1.0f * DiceRollSpeed, 0, 0), Space.Self);
            this.transform.Rotate(new Vector3(0, 1.0f * DiceRollSpeed, 0), Space.World);

            //ダイスの回転を止めるときに行う処理
            if (DiceDecisionFlg == true)
            {
                JumpFlg = true;
                JumpPosY = this.gameObject.transform.position.y;
                DiceNo = Random.Range(1, 7);
                if(manager.Ordering == true)
                {
                    while (DiceNo == manager.OrderjudgeNo[0] 
                        || DiceNo == manager.OrderjudgeNo[1] 
                        || DiceNo == manager.OrderjudgeNo[2] 
                        || DiceNo == manager.OrderjudgeNo[3])
                    {
                        DiceNo = Random.Range(1, 7);
                    }
                }               
                manager.SetDiceNo(DiceNo);
                if (manager.gameStatus == GameSTS.Play)
                {
                    manager.CanMove();
                }
                for (int i = 0;i < 6; ++i)
                {
                    DiceSideRenderers[i].material = DiceMats[DiceNo - 1];
                }                
                this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                DiceRollFlg = false;
                DiceDecisionFlg = false;
            }
        }

        this.gameObject.transform.position = new Vector3(this.transform.position.x, JumpPosY + Mathf.Sin(JumpSin), this.transform.position.z);
    }
}
