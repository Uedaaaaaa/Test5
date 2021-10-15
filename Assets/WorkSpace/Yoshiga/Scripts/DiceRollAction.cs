using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRollAction : MonoBehaviour
{
    //ダイスの回転フラグ T:回転　F:止まる
    private bool DiceRollFlg = false;
    [Header("ダイスの回転速度")]
    [SerializeField]
    private float DiceRollSpeed;
    [Header("ダイスの面 : オブジェクト")]
    [SerializeField]
    private GameObject[] DiceSides = new GameObject[6];
    [Header("ダイスの面のマテリアル")]
    [SerializeField]
    private Material[] DiceMats = new Material[6];
    //ダイスの面のrenderer
    private MeshRenderer[] DiceSideRenderers = new MeshRenderer[6];
    //ダイスの目を保存するための変数
    private int DiceNo;

    //回転しているダイスを止めるためのフラグ
    private bool DiceDecisionFlg = false;
  
    [Header("ダイスの跳ねる速度")]
    [SerializeField]
    private float JumpSpeed;
    //ジャンプの時のSin
    private float JumpSin;
    private bool JumpFlg;
    //ジャンプした時のy
    private float JumpPosY;

    [Header("ダイスが消えるまでの秒数")]
    [SerializeField]
    private float DestroyTime;
    //ジャンプして消えるまでのカウントダウンが始まるためのフラグ
    private bool DestroyFlg;

    [Header("もくもくエフェクト : オブジェクト")]
    [SerializeField]
    private GameObject CloudEffect;
    [Header("紙吹雪エフェクト : オブジェクト")]
    [SerializeField]
    private GameObject ConfettiEffect;

    // Start is called before the first frame update
    void Start()
    {
        JumpPosY = this.gameObject.transform.position.y;

        for (int i = 0; i < 6;++i)
        {
            DiceSideRenderers[i] = DiceSides[i].GetComponent<MeshRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("BtnA"))
        {
            //ダイスロールを開始
            if (DiceRollFlg == true)
            {
                DiceDecisionFlg = true;
            }
            else //ダイスロールを止める
            {
                DiceRollFlg = true;
            }
        }
    }

    private void FixedUpdate()
    {        
        //ダイスが跳ねている時にSinを増やす処理
        if(JumpFlg == true)
        {
            JumpSin += Time.deltaTime * JumpSpeed;

            if(Mathf.Sin(JumpSin) < 0)
            {
                JumpFlg = false;
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
                for(int i = 0;i < 6; ++i)
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
