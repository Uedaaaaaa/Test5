using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using KanKikuchi.AudioManager;

namespace KanKikuchi.AudioManager
{
    public class TitlePlayer : MonoBehaviour
    {
        private Rigidbody MyRB;             //自身のRigidbody
        private Animator MyAnim;
        private bool isRotate;
        private bool isMove;
        private float RotateY;
        private float RotateManager;
        private float BeforeAxis;
        private bool FeedInFlg;
        private bool FeedOutFlg;

        [SerializeField] float RotateSpeed;
        [SerializeField] float MoveSpeed;
        [SerializeField] float FeedSpeed;

        //UI関連
        [SerializeField] Image imgTitle;
        [SerializeField] Image imgTitleSel;
        [SerializeField] Image Feed;
        float alfa;    //A値を操作するための変数
        float red, green, blue;    //RGBを操作するための変数

        private int Sel;
        // Start is called before the first frame update
        void Start()
        {
            BGMManager.Instance.Play(BGMPath.TITLE_BGM);
            FeedInFlg = false;
            FeedOutFlg = true;
            isMove = true;
            Sel = 0;
            RotateY = -90.0f;
            MyRB = this.gameObject.GetComponent<Rigidbody>();
            MyAnim = this.gameObject.GetComponent<Animator>();
            red = Feed.color.r;
            green = Feed.color.g;
            blue = Feed.color.b;
            alfa = 1.0f;

        }
        void Update()
        {
            Feed.color = new Color(red, green, blue, alfa);
            //フェードイン
            if (FeedInFlg)
            {
                FeedIn();
                if (alfa >= 1.0f)
                {
                    SceneManager.LoadScene("Main");
                }
            }
            if (FeedOutFlg)
            {
                FeedOut();
                if (alfa <= 0.1f)
                {
                    FeedOutFlg = false;
                }
            }
            //選択肢
            imgTitleSel.transform.localPosition = new Vector3(0.0f, Sel * -160.0f, 0.0f);
            float v = Input.GetAxis("Vertical");
            //↑
            if (!FeedOutFlg && v > 0 && BeforeAxis == 0.0f)
            {
                if (Sel > 0)
                {
                    SEManager.Instance.Play(SEPath.CURSOR_CAHAGE);
                    Sel--;
                }
            }
            //↓
            if (!FeedOutFlg && v < 0 && BeforeAxis == 0.0f)
            {
                if (Sel < 1)
                {
                    SEManager.Instance.Play(SEPath.CURSOR_CAHAGE);
                    Sel++;
                }
            }
            BeforeAxis = v;

            if (!FeedOutFlg && isMove && Input.GetKeyDown(KeyCode.Return))
            {
                if (Sel == 0)
                {
                    SEManager.Instance.Play(SEPath.LETS_HALLOWIN);
                    MyAnim.SetTrigger("Start");
                    isMove = false;
                    //2秒後にFeed
                    StartCoroutine("StartFeedIn");
                }
                else
                {
                    //ゲーム終了
                }
            }
            //回転
            if (isRotate)
            {
                RotateY += RotateSpeed * Time.deltaTime;
                RotateManager += RotateSpeed * Time.deltaTime;
                this.gameObject.transform.rotation = Quaternion.Euler(0.0f, RotateY, 0.0f);
                if (RotateManager >= 90.0f)
                {
                    RotateManager = 0;
                    isRotate = false;
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Rotate")
            {
                isRotate = true;
            }
        }
        // Update is called once per frame
        void FixedUpdate()
        {
            if (isMove)
            {
                MyRB.AddForce(transform.forward * MoveSpeed, ForceMode.Force);
            }
        }
        void FeedIn()
        {
            //if(alfa <= 1.0f) 
            alfa += FeedSpeed * Time.deltaTime;
        }
        void FeedOut()
        {
            //if (alfa >= 1.0f) 
            alfa -= FeedSpeed * Time.deltaTime;
        }
        IEnumerator StartFeedIn()
        {
            yield return new WaitForSeconds(1.0f);
            BGMManager.Instance.FadeOut(2.0f);
            FeedInFlg = true;
        }
    }
}
