using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using KanKikuchi.AudioManager;
using UnityEngine.SceneManagement;

//イベント内容
[System.Serializable]
public class EventData
{
    [Tooltip("SE")] public string SEPath;
    [Tooltip("イベントのキャラ画像")] public Sprite SpriteEventChara;
    [TextArea(1, 4), Tooltip("テキスト内容")] public string Message;
    [Tooltip("名前テキスト")] public string TextName;
}
//イベント内容にクイズが出てくるタイミングフラグ、正解テキストかどうかの判断フラグを追加
[System.Serializable]
public class QuizEventData : EventData
{
    [Tooltip("クイズを出すタイミングでチェック")] public bool QuizSet;
    [Tooltip("不正解のテキストに全てチェック")] public bool AnswerText;
}
[System.Serializable]
public class HalloweenEventData : EventData
{
    [Tooltip("何もしないかお菓子くださいの選択肢表示")] public bool SetSelect;
    [Tooltip("やるき選択の選択肢表示")] public bool SetYaruki;
    [Tooltip("何もしないかった")] public bool NanimoSinai;
    [Tooltip("トリックオアトリート成功時")] public bool TrueTrick;
    [Tooltip("トリックオアトリート失敗時")] public bool FalseTrick;
}
[System.Serializable]
//プラス、マイナスイベント
public class SpuareEvent
{
    [SerializeField, Tooltip("イベントの名前")] string EventName;
    [SerializeField, Tooltip("やる気の増減")] public int ChangeYaruki;

    [Tooltip("Sizeにテキストの数を入力")] public List<EventData> eventData;
}
[System.Serializable]
//クイズイベント(プラスマイナスマスに回答をプラス)
public class QuizEvent
{
    [SerializeField, Tooltip("イベントの名前")] string EventName;
    [SerializeField, Tooltip("やる気の増減")] public int ChangeYaruki;
    [Tooltip("クイズの選択肢　一番上に答えを入力")]public string[] Answer = new string[3] { "","",""};

    [Tooltip("Sizeにテキストの数を入力")] public List<QuizEventData> eventData;

}
[System.Serializable]
//ハロウィンイベント
public class HalloweenEvent
{
    [SerializeField, Tooltip("イベントの名前")] string EventName;
    [Tooltip("補正値")] public int Correction;
    [SerializeField, Tooltip("やる気の増減")] public int ChangeCandy;

    [Tooltip("Sizeにテキストの数を入力")] public List<HalloweenEventData> eventData;
}

public class SpuareAction : MonoBehaviour
{
    GameObject Canvas;
    float alfa;    //A値を操作するための変数
    float red, green, blue;    //RGBを操作するための変数

    [SerializeField] float NovelSpeed;
    [SerializeField] float FeedSpeed = 0.02f;  //透明化の速さ
    [SerializeField] Sprite[] PlayerUI;
    [SerializeField] Sprite[] Back;
    [SerializeField] Sprite Pumpkin;
    [SerializeField] Sprite Up;
    [SerializeField] Sprite Down;

    [Space(20)]
    [SerializeField] Image Feed;
    [SerializeField] Image imgEventChara;
    [SerializeField] Image imgTextSpace;
    [SerializeField] Image imgBbtn;
    [SerializeField] Image imgSel;
    [SerializeField] Text txtMessage;
    [SerializeField] Text txtTextName;
    [SerializeField] Image[] imgQuizSpace;
    [SerializeField] Text[] txtAnswer;
    [SerializeField] Image imgCharaUI;
    [SerializeField] Text txtPlayerName;
    [SerializeField] Text txtCandy;
    [SerializeField] Text txtYaruki;
    [SerializeField] Text txtTrick;
    [SerializeField] Text txtNanimoSinai;
    [SerializeField] Image imgHalloweenSel;
    [SerializeField] Image imgYaruki;
    [SerializeField] Image imgBack;
    [SerializeField] Image imgUpDown;
    [Space(20)]

    [SerializeField,TextArea(1, 3)] string[] RuleText;
    [SerializeField, TextArea(1, 3)] string[] ResultText;

    [Space(50)]

    [SerializeField, Tooltip("Sizeにイベントの種類の個数を入力")] List<SpuareEvent> plusEvent = new List<SpuareEvent>();
    [Space(30)]
    [SerializeField, Tooltip("Sizeにイベントの種類の個数を入力")] List<SpuareEvent> minusEvent = new List<SpuareEvent>();
    [Space(30)]
    [SerializeField, Tooltip("Sizeにイベントの種類の個数を入力")] List<QuizEvent> quizEvent = new List<QuizEvent>();
    [Space(30)]
    [SerializeField, Tooltip("Sizeにイベントの種類の個数を入力")] List<HalloweenEvent> halloweenEvent = new List<HalloweenEvent>();

    private int EventRand;//どのイベントを行うかの乱数
    private int EventCount;//何番目のテキストか
    private bool PlusFlg;
    private bool MinusFlg;
    private bool QuizFlg;
    private bool HalloweenFlg;
    private bool NextTextFlg;
    private string[] BestAnswer = new string[50];
    private int Sel;
    private bool NowQuizFlg;
    private bool isCorrect;
    [HideInInspector]
    public bool FeedInFlg = false;
    private bool FeedOutFlg = true;
    private bool isRule = true;
    [HideInInspector]
    public bool isResult = false;
    private bool FirstFeed = true;
    private float BeforeAxis;

    private bool NanimoSinai;
    private bool NoYaruki;
    private bool TrueTrick;
    private bool FalseTrick;
    private bool isInput;
    private bool EndDice;
    private bool DoSort;

    private int CharaNo;
    private int UseYaruki;
    private int SuccessRate;
    private int SuccessRand;
    List<Character> list = new List<Character>();

    //同じイベントがかぶらないようにする
    List<int> PlusList = new List<int>();
    List<int> MinusList = new List<int>();
    List<int> QuizList = new List<int>();

    private int i;

    private GameManager manager;
    public SetCharacerUI CharacerUI;

    // Start is called before the first frame update
    void Start()
    {
        Canvas = GameObject.Find("Canvas");
        Feed = Canvas.transform.Find("Feed").GetComponent<Image>();
        imgEventChara = Canvas.transform.Find("imgEventChara").GetComponent<Image>();
        imgTextSpace = Canvas.transform.Find("imgTextSpace").GetComponent<Image>();
        imgBbtn = Canvas.transform.Find("imgBbtn").GetComponent<Image>();
        imgSel = Canvas.transform.Find("imgSel").GetComponent<Image>();
        txtMessage = Canvas.transform.Find("txtMessage").GetComponent<Text>();
        txtTextName = Canvas.transform.Find("txtTextName").GetComponent<Text>();
        imgQuizSpace[0] = Canvas.transform.Find("imgQuizSpace1").GetComponent<Image>();
        imgQuizSpace[1] = Canvas.transform.Find("imgQuizSpace2").GetComponent<Image>();
        imgQuizSpace[2] = Canvas.transform.Find("imgQuizSpace3").GetComponent<Image>();
        txtAnswer[0] = Canvas.transform.Find("txtAnswer1").GetComponent<Text>();
        txtAnswer[1] = Canvas.transform.Find("txtAnswer2").GetComponent<Text>();
        txtAnswer[2] = Canvas.transform.Find("txtAnswer3").GetComponent<Text>();
        imgCharaUI = Canvas.transform.Find("imgCharaUI").GetComponent<Image>();
        txtPlayerName = Canvas.transform.Find("txtPlayerName").GetComponent<Text>();
        txtCandy = Canvas.transform.Find("txtCandy").GetComponent<Text>();
        txtYaruki = Canvas.transform.Find("txtYaruki").GetComponent<Text>();
        imgHalloweenSel = Canvas.transform.Find("imgHalloweenSel").GetComponent<Image>();
        imgYaruki = Canvas.transform.Find("imgYaruki").GetComponent<Image>();
        txtTrick = Canvas.transform.Find("txtTrick").GetComponent<Text>();
        txtNanimoSinai = Canvas.transform.Find("txtNanimoSinai").GetComponent<Text>();
        imgBack = Canvas.transform.Find("imgBack").GetComponent<Image>();
        imgUpDown = Canvas.transform.Find("imgUpDown").GetComponent<Image>();

        manager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
        for (int i = 0; i < quizEvent.Count; i++)
        {
            //答えを入れておく
            BestAnswer[i] = quizEvent[i].Answer[0];
        }

        //各リストに要素数分0から追加
        AddEventRandList();

        Sel = 0;
        UseYaruki = 1;
        NextTextFlg = false;
        isInput = true;
        EndDice = false;
        //最初は非表示
        HideUI();
        //キャラとテキストスペースは表示
        imgEventChara.gameObject.SetActive(true);
        imgTextSpace.gameObject.SetActive(true);

        red = Feed.color.r;
        green = Feed.color.g;
        blue = Feed.color.b;
        alfa = 1.0f;

        BGMManager.Instance.Play(BGMPath.RULE_BGM);

    }
    public void ChangeEndDice()
    {
        EndDice = true;
    }
    // Update is called once per frame
    void Update()
    {
        float v = Input.GetAxis("Vertical");

        Feed.color = new Color(red, green, blue, alfa);
        if(PlusList.Count == 0 || MinusList.Count == 0 || QuizList.Count == 0)
        {
            AddEventRandList();
        }
        //フェードイン
        if (FeedInFlg)
        {
            FeedIn();
            if (alfa >= 1.0f)
            {
                FeedInFlg = false;
                //Feedがイベント開始時ならUIを表示
                if (PlusFlg || MinusFlg || QuizFlg || HalloweenFlg)
                {
                    for (int i = 0; i < PlayerUI.Length; i++)
                    {
                        if (i == CharaNo - 1)
                        {
                            imgCharaUI.sprite = PlayerUI[i];
                        }
                    }
                    CharacerUI.PlayerStatusUIDestroy();
                    ShowUI();
                    if (HalloweenFlg&&manager.characters[CharaNo-1].yaruki == 0)
                    {
                        imgEventChara.gameObject.SetActive(false);
                    }
                }
                //ルール説明終了時
                else if (isRule)
                {
                    HideUI();
                }
                else if (isResult && i == 11)
                {
                    SceneManager.LoadScene("Title");
                }

                else if (isResult)
                {
                    imgEventChara.sprite = Pumpkin;
                    imgEventChara.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                    CharacerUI.PlayerStatusUIDestroy();
                    imgEventChara.gameObject.SetActive(true);
                    imgTextSpace.gameObject.SetActive(true);
                    manager.GameFinish();
                    DoSort = true;
                }


                //イベント終了時
                else
                {
                    manager.ChangePos();
                    CharacerUI.PlayerStatusUISet();
                    HideUI();
                }
                //FeedOut開始
                FeedOutFlg = true;
            }
        }
        if (FeedOutFlg)
        {
            FeedOut();
            if (alfa <= 0.1f)
            {
                //フェードアウトが完了したら文字が流れ始める
                if (PlusFlg)
                {
                    txtMessage.gameObject.SetActive(true);
                    txtTextName.gameObject.SetActive(true);
                    StartCoroutine("Novel", plusEvent[EventRand].eventData[EventCount].Message);
                    if(plusEvent[EventRand].eventData[EventCount].SEPath != null)
                    {
                        SEManager.Instance.Play("SE/SE/" + plusEvent[EventRand].eventData[EventCount].SEPath);
                    }
                }
                else if (MinusFlg)
                {
                    txtMessage.gameObject.SetActive(true);
                    txtTextName.gameObject.SetActive(true);
                    StartCoroutine("Novel", minusEvent[EventRand].eventData[EventCount].Message);
                    if (minusEvent[EventRand].eventData[EventCount].SEPath != null)
                    {
                        SEManager.Instance.Play("SE/SE/" + minusEvent[EventRand].eventData[EventCount].SEPath);
                    }

                }
                else if (QuizFlg)
                {
                    txtMessage.gameObject.SetActive(true);
                    txtTextName.gameObject.SetActive(true);
                    StartCoroutine("Novel", quizEvent[EventRand].eventData[EventCount].Message);
                    if (quizEvent[EventRand].eventData[EventCount].SEPath != null)
                    {
                        SEManager.Instance.Play("SE/SE/" + quizEvent[EventRand].eventData[EventCount].SEPath);
                    }

                }
                else if (HalloweenFlg)
                {
                    txtTextName.text = "プレイヤー" + CharaNo.ToString();

                    txtMessage.gameObject.SetActive(true);
                    if(manager.characters[CharaNo -1].yaruki == 0)
                    {
                        StartCoroutine("Novel", "尋ねてみたが留守のようだ。\nやる気を貯めてまた来よう！");
                        NoYaruki = true;
                    }
                    else
                    {
                        txtTextName.gameObject.SetActive(true);
                        StartCoroutine("Novel", halloweenEvent[EventRand].eventData[EventCount].Message);
                        if (halloweenEvent[EventRand].eventData[EventCount].SEPath != null)
                        {
                            SEManager.Instance.Play("SE/SE/" + halloweenEvent[EventRand].eventData[EventCount].SEPath);
                        }

                    }
                }
                else if (isRule && !EndDice && i > 0)
                {
                    isRule = false;
                    i = 0;
                    BGMManager.Instance.Play(BGMPath.MAIN_BGM);
                    manager.GameStart();
                }
                else if(isRule)
                {
                    FirstFeed = false;
                    txtTextName.text = "カボチャ";
                    txtMessage.gameObject.SetActive(true);
                    txtTextName.gameObject.SetActive(true);
                    StartCoroutine("Novel",RuleText[0]);
                }
                else if (isResult)
                {
                    BGMManager.Instance.Play(BGMPath.END_BGM);
                    txtTextName.text = "カボチャ";
                    txtMessage.gameObject.SetActive(true);
                    txtTextName.gameObject.SetActive(true);
                    StartCoroutine("Novel", ResultText[0]);
                }
                else
                {
                    EndEvent();
                }
                FeedOutFlg = false;

            }
        }

        if (NextTextFlg)
        {
            imgBbtn.gameObject.SetActive(true);
        }
        else
        {
            imgBbtn.gameObject.SetActive(false);
        }
        //ルール説明中
        if(isRule)
        {

            //順番決め終了検知してテキスト表示
            if (EndDice && manager.gameStatus == GameSTS.OrderJudge)
            {
                isInput = true;
                EndDice = false;
                imgTextSpace.gameObject.SetActive(true);
                imgEventChara.gameObject.SetActive(true);
                txtMessage.gameObject.SetActive(true);
                txtTextName.gameObject.SetActive(true);
                StartCoroutine("Novel", RuleText[i]);
            }
            if (isInput&&!FeedInFlg && !FeedOutFlg && Input.GetButtonDown("BtnA"))
            {
                if(NextTextFlg)
                {
                    SEManager.Instance.Play(SEPath.PUSH_B);
                    NextTextFlg = false;
                    if (i == 6)
                    {
                        i = 7;
                        isInput = false;
                        manager.SpawnDice();
                        imgTextSpace.gameObject.SetActive(false);
                        imgEventChara.gameObject.SetActive(false);
                        txtMessage.gameObject.SetActive(false);
                        txtTextName.gameObject.SetActive(false);
                    }
                    else if (i == 10)
                    {
                        BGMManager.Instance.Stop();
                        SEManager.Instance.Play(SEPath.LETS_HALLOWIN);
                        ////デバッグ
                        //isResult = true;
                        //isRule = false;
                        //i = 0;

                        FeedInFlg = true;
                    }
                    else
                    {
                        i++;
                        if(i == 9)
                        {
                            int No1 = manager.OrderArray[0] + 1;
                            int No2 = manager.OrderArray[1] + 1;
                            int No3 = manager.OrderArray[2] + 1;
                            int No4 = manager.OrderArray[3] + 1;
                            RuleText[i] = "1番 プレイヤー" + No1.ToString() + "さん。2番 プレイヤー" + No2.ToString() + "さん。\n3番 プレイヤー" + No3.ToString() + "さん。4番 プレイヤー" + No4.ToString() + "さん。";
                        }
                        StartCoroutine("Novel", RuleText[i]);
                    }
                } 
                else
                {
                    txtMessage.text = RuleText[i];
                    NextTextFlg = true;
                    StopCoroutine("Novel");
                }
            }
        }
        //結果発表
        if (isResult)
        {

            if (DoSort)
            {
                Debug.Log("DoSort");
                DoSort = false;
                for(int i = 0;i < 4;i++)
                {
                    list.Add(manager.characters[i]);
                }
                //int[] Rank
                list.Sort((a, b) => a.rank - b.rank);
            }
            if (!FeedInFlg && !FeedOutFlg && Input.GetButtonDown("BtnA"))
            {
                if (NextTextFlg)
                {
                    SEManager.Instance.Play(SEPath.PUSH_B);
                    NextTextFlg = false;
                    //順位発表
                    if (i >= 2&&i < 8)
                    {
                        i = 3;
                        //1244
                        if (list[0].rank == 1 && list[1].rank == 2 && list[2].rank == 4 && list[3].rank == 4)
                        {
                            if (ResultText[3][0] == '4')
                            {
                                ResultText[3] = "2位　プレイヤー" + list[1].myNo.ToString() + "さん！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.RANK4TO2);
                            }
                            else if (ResultText[3][0] == '2')
                            {
                                ResultText[3] = "1位　プレイヤー" + list[0].myNo.ToString() + "さん！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.RANK4TO2);

                            }
                            else if (ResultText[3][0] == '1')
                            {
                                ResultText[3] = "ということで優勝はプレイヤー" + list[0].myNo.ToString() + "さんになります！\nおめでとうございます！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.YOUR_CHAMPION,1.2f);

                            }
                            else if (ResultText[3][0] == 'と')
                            {
                                i = 8;
                                StartCoroutine("Novel", ResultText[i]);
                            }
                            else
                            {
                                ResultText[3] = "4位　プレイヤー" + list[2].myNo.ToString() + "さんとプレイヤー" + list[3].myNo.ToString() + "さん！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.RANK4TO2);

                            }
                        }
                        //1224
                        else if (list[0].rank == 1 && list[1].rank == 2 && list[2].rank == 2 && list[3].rank == 4)
                        {
                            if (ResultText[3][0] == '4')
                            {
                                ResultText[3] = "2位　プレイヤー" + list[1].myNo.ToString() + "さんとプレイヤー" + list[2].myNo.ToString() + "さん！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.RANK4TO2);

                            }
                            else if (ResultText[3][0] == '2')
                            {
                                ResultText[3] = "1位　プレイヤー" + list[0].myNo.ToString() + "さん！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.RANK4TO2);

                            }
                            else if (ResultText[3][0] == '1')
                            {
                                ResultText[3] = "ということで優勝はプレイヤー" + list[0].myNo.ToString() + "さんになります！\nおめでとうございます！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.YOUR_CHAMPION, 1.2f);

                            }
                            else if (ResultText[3][0] == 'と')
                            {
                                i = 8;
                                StartCoroutine("Novel", ResultText[i]);
                            }
                            else
                            {
                                ResultText[3] = "4位　プレイヤー" + list[3].myNo.ToString() + "さん！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.RANK4TO2);

                            }

                        }
                        //1134
                        else if (list[0].rank == 1 && list[1].rank == 1 && list[2].rank == 3 && list[3].rank == 4)
                        {
                            if (ResultText[3][0] == '4')
                            {
                                ResultText[3] = "3位　プレイヤー" + list[2].myNo.ToString() + "さん！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.RANK4TO2);

                            }
                            else if (ResultText[3][0] == '3')
                            {
                                ResultText[3] = "1位　プレイヤー" + list[0].myNo.ToString() + "さんとプレイヤー" + list[1].myNo.ToString() + "さん！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.RANK4TO2);

                            }
                            else if (ResultText[3][0] == '1')
                            {
                                ResultText[3] = "ということで優勝はプレイヤー" + list[0].myNo.ToString() + "さんと\nプレイヤー" + list[1].myNo.ToString() + "なります！おめでとうございます！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.YOUR_CHAMPION, 1.2f);

                            }
                            else if (ResultText[3][0] == 'と')
                            {
                                i = 8;
                                StartCoroutine("Novel", ResultText[i]);
                            }
                            else
                            {
                                ResultText[3] = "4位　プレイヤー" + list[3].myNo.ToString() + "さん！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.RANK4TO2);

                            }

                        }
                        //1444
                        else if (list[0].rank == 1 && list[1].rank == 4 && list[2].rank == 4 && list[3].rank == 4)
                        {
                            if (ResultText[3][0] == '4')
                            {
                                ResultText[3] = "1位　プレイヤー" + list[0].myNo.ToString() + "さん！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.RANK4TO2);

                            }
                            else if (ResultText[3][0] == '1')
                            {
                                ResultText[3] = "ということで優勝はプレイヤー" + list[0].myNo.ToString() + "さんになります！\nおめでとうございます！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.YOUR_CHAMPION, 1.2f);

                            }
                            else if (ResultText[3][0] == 'と')
                            {
                                i = 8;
                                StartCoroutine("Novel", ResultText[i]);
                            }
                            else
                            {
                                ResultText[3] = "4位　プレイヤー" + list[3].myNo.ToString() + "さんとプレイヤー" + list[2].myNo.ToString() + "さんと\nプレイヤー" + list[1].myNo.ToString() + "さん！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.RANK4TO2);

                            }

                        }
                        //1114
                        else if (list[0].rank == 1 && list[1].rank == 1 && list[2].rank == 1 && list[3].rank == 4)
                        {
                            if (ResultText[3][0] == '4')
                            {
                                ResultText[3] = "1位　プレイヤー" + list[0].myNo.ToString() + "さんとプレイヤー" + list[1].myNo.ToString() + "さんと\nプレイヤー" + list[2].myNo.ToString() + "さん！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.RANK4TO2);

                            }
                            else if (ResultText[3][0] == '1')
                            {
                                ResultText[3] = "ということで優勝はプレイヤー" + list[0].myNo.ToString() + "さんと\nプレイヤー" + list[1].myNo.ToString() + "さんとプレイヤー" + list[2].myNo.ToString() + "さんになります！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.YOUR_CHAMPION, 1.2f);

                            }
                            else if (ResultText[3][0] == 'と')
                            {
                                ResultText[3] = "おめでとうございます！";
                                StartCoroutine("Novel", ResultText[i]);
                            }
                            else if (ResultText[3][0] == 'お')
                            {
                                i = 8;
                                StartCoroutine("Novel", ResultText[i]);
                            }

                            else
                            {
                                ResultText[3] = "4位　プレイヤー" + list[3].myNo.ToString() + "さん！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.RANK4TO2);

                            }

                        }
                        //1144
                        else if (list[0].rank == 1 && list[1].rank == 1 && list[2].rank == 4 && list[3].rank == 4)
                        {
                            if (ResultText[3][0] == '4')
                            {
                                ResultText[3] = "1位　プレイヤー" + list[0].myNo.ToString() + "さんとプレイヤー" + list[1].myNo.ToString() + "さん！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.RANK4TO2);

                            }
                            else if (ResultText[3][0] == '1')
                            {
                                ResultText[3] = "ということで優勝はプレイヤー" + list[0].myNo.ToString() + "さんとプレイヤー" + list[1].myNo.ToString() + "さんになります！おめでとうございます！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.YOUR_CHAMPION, 1.2f);

                            }
                            else if (ResultText[3][0] == 'と')
                            {
                                i = 8;
                                StartCoroutine("Novel", ResultText[i]);
                            }
                            else
                            {
                                ResultText[3] = "4位　プレイヤー" + list[2].myNo.ToString() + "さんとプレイヤー" + list[3].myNo.ToString() + "さん！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.RANK4TO2);

                            }

                        }
                        //1111
                        else if (list[0].rank == 1 && list[1].rank == 1 && list[2].rank == 1 && list[3].rank == 1)
                        {
                            if (ResultText[3][0] == '1')
                            {
                                ResultText[3] = "とてもおもしろい結果になりましたね！\n全員同率で１位とは驚きました！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.YOUR_CHAMPION, 1.2f);

                            }
                            else if (ResultText[3][0] == 'と')
                            {
                                ResultText[3] = "またのゲームでも面白い展開を期待しています！";
                                StartCoroutine("Novel", ResultText[3]);
                            }
                            else if (ResultText[3][0] == 'ま')
                            {
                                i = 8;
                                StartCoroutine("Novel", ResultText[i]);
                            }
                            else
                            {
                                ResultText[3] = "1位　プレイヤー" + list[0].myNo.ToString() + "さんとプレイヤー" + list[1].myNo.ToString() + "さんと\nプレイヤー" + list[2].myNo.ToString() + "さんとプレイヤー" + list[3].myNo.ToString() + "さん！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.RANK4TO2);

                            }

                        }
                        else
                        {
                            if (ResultText[3][0] == '4')
                            {
                                ResultText[3] = "3位　プレイヤー" + list[2].myNo.ToString() + "さん！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.RANK4TO2);

                            }
                            else if (ResultText[3][0] == '3')
                            {
                                ResultText[3] = "2位　プレイヤー" + list[1].myNo.ToString() + "さん！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.RANK4TO2);

                            }
                            else if (ResultText[3][0] == '2')
                            {
                                ResultText[3] = "1位　プレイヤー" + list[0].myNo.ToString() + "さん！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.RANK4TO2);

                            }
                            else if (ResultText[3][0] == '1')
                            {
                                ResultText[3] = "ということで優勝はプレイヤー" + list[0].myNo.ToString() + "さんになります！\nおめでとうございます！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.YOUR_CHAMPION, 1.2f);

                            }
                            else if (ResultText[3][0] == 'と')
                            {
                                i = 8;
                                StartCoroutine("Novel", ResultText[i]);
                            }
                            else
                            {
                                ResultText[3] = "4位　プレイヤー" + list[3].myNo.ToString() + "さん！";
                                StartCoroutine("Novel", ResultText[3]);
                                SEManager.Instance.Play(SEPath.RANK4TO2);

                            }

                        }

                    }
                    else if(i == 11)
                    {
                        FeedInFlg = true;
                        BGMManager.Instance.FadeOut(2.0f);
                    }
                    else
                    {
                        i++;
                        StartCoroutine("Novel", ResultText[i]);
                    }
                }
                else
                {
                    txtMessage.text = ResultText[i];
                    NextTextFlg = true;
                    StopCoroutine("Novel");
                }
            }
        }
        //プラスイベント処理
        if (PlusFlg)
        {
            if (isInput&&!FeedInFlg && !FeedOutFlg && Input.GetButtonDown("BtnA"))
            {
                if (NextTextFlg)
                {
                    NextTextFlg = false;
                    //次のテキストがない
                    if (EventCount + 1 == plusEvent[EventRand].eventData.Count)
                    {
                        //イベント終了
                        SEManager.Instance.Play(SEPath.PUSH_B);
                        txtMessage.gameObject.SetActive(false);
                        txtTextName.gameObject.SetActive(false);
                        PlusFlg = false;
                        //フェード開始してイベ終了
                        FeedInFlg = true;
                    }
                    //次がやるき上昇テキスト = やる気テキストが最後なので2個先のテキストがない
                    else if (EventCount + 2 == plusEvent[EventRand].eventData.Count)
                    {
                        EventCount++;
                        SEManager.Instance.Play(SEPath.YARUKI_UP);
                        manager.characters[CharaNo - 1].yaruki += plusEvent[EventRand].ChangeYaruki;
                        if(manager.characters[CharaNo -1].yaruki > 10)
                        {
                            manager.characters[CharaNo - 1].yaruki = 10;
                        }
                        txtYaruki.text = manager.characters[CharaNo - 1].yaruki.ToString();
                        StartCoroutine("UpDown");
                        SetNextText(null, plusEvent[EventRand].eventData, null);
                    }
                    else
                    {
                        SEManager.Instance.Play(SEPath.PUSH_B);
                        EventCount++;
                        SetNextText(null, plusEvent[EventRand].eventData, null);
                    }
                }
                else
                {
                    txtMessage.text = plusEvent[EventRand].eventData[EventCount].Message;
                    NextTextFlg = true;
                    StopCoroutine("Novel");
                }
            }
        }
        //マイナス
        if (MinusFlg)
        {
            if (isInput && !FeedInFlg && !FeedOutFlg && Input.GetButtonDown("BtnA"))
            {
                if (NextTextFlg)
                {
                    SEManager.Instance.Play(SEPath.PUSH_B);
                    NextTextFlg = false;
                    //次のテキストがない
                    if (EventCount + 1 == minusEvent[EventRand].eventData.Count)
                    {
                        //イベント終了
                        txtMessage.gameObject.SetActive(false);
                        txtTextName.gameObject.SetActive(false);
                        MinusFlg = false;
                        //フェード開始してイベ終了
                        FeedInFlg = true;

                    }
                    //次がやるき減少テキスト = やる気テキストが最後なので2個先のテキストがない
                    else if (EventCount + 2 == minusEvent[EventRand].eventData.Count)
                    {
                        EventCount++;
                        SEManager.Instance.Play(SEPath.YARUKI_MINUS);
                        manager.characters[CharaNo - 1].yaruki -= minusEvent[EventRand].ChangeYaruki;
                        if (manager.characters[CharaNo - 1].yaruki < 0)
                        {
                            manager.characters[CharaNo - 1].yaruki = 0;
                        }

                        txtYaruki.text = manager.characters[CharaNo - 1].yaruki.ToString();
                        StartCoroutine("UpDown");

                        SetNextText(null, minusEvent[EventRand].eventData, null);
                    }
                    else
                    {
                        EventCount++;
                        SetNextText(null, minusEvent[EventRand].eventData, null);
                    }
                }
                else
                {
                    txtMessage.text = minusEvent[EventRand].eventData[EventCount].Message;
                    NextTextFlg = true;
                    StopCoroutine("Novel");
                }
            }
        }
        //クイズ
        if (QuizFlg)
        {
            imgSel.transform.localPosition = new Vector3(-860.0f, 290 + (Sel * -200.0f), 0.0f);
            //選択の矢印が出てるとき
            if (imgSel.gameObject.activeSelf)
            {
                if (v > 0 && BeforeAxis == 0.0f)
                {
                    if (Sel > 0)
                    {
                        SEManager.Instance.Play(SEPath.CURSOR_CAHAGE);
                        Sel--;
                    }
                }
                if (v < 0 && BeforeAxis == 0.0f)
                {
                    if (Sel < 2)
                    {
                        SEManager.Instance.Play(SEPath.CURSOR_CAHAGE);
                        Sel++;
                    }
                }
            }
            if (isInput && !FeedInFlg && !FeedOutFlg && Input.GetButtonDown("BtnA"))
            {
                if (NextTextFlg)
                {
                    NextTextFlg = false;
                    //次のテキストがない
                    if (EventCount + 1 == quizEvent[EventRand].eventData.Count)
                    {
                        SEManager.Instance.Play(SEPath.PUSH_B);

                        //イベント終了
                        txtMessage.gameObject.SetActive(false);
                        txtTextName.gameObject.SetActive(false);
                        QuizFlg = false;
                        //フェード開始してイベ終了
                        FeedInFlg = true;
                    }
                    //クイズあるなら出す
                    else if (quizEvent[EventRand].eventData[EventCount].QuizSet == true && !NowQuizFlg)
                    {
                        SEManager.Instance.Play(SEPath.PUSH_B);

                        //配列をシャッフル
                        quizEvent[EventRand].Answer = quizEvent[EventRand].Answer.OrderBy(i => System.Guid.NewGuid()).ToArray();
                        //選択肢表示
                        for (int i = 0; i < 3; i++)
                        {
                            txtAnswer[i].gameObject.SetActive(true);
                            imgQuizSpace[i].gameObject.SetActive(true);
                            txtAnswer[i].text = quizEvent[EventRand].Answer[i];
                        }
                        imgSel.gameObject.SetActive(true);
                        NextTextFlg = true;
                        //クイズ中
                        NowQuizFlg = true;
                    }
                    //正解なら
                    else if (quizEvent[EventRand].Answer[Sel] == BestAnswer[EventRand] && NowQuizFlg)
                    {
                        SEManager.Instance.Play(SEPath.QUIZ_BINGO);

                        for (int i = 0; i < 3; i++)
                        {
                            if (i == Sel)
                            {
                                continue;
                            }
                            txtAnswer[i].gameObject.SetActive(false);
                            imgQuizSpace[i].gameObject.SetActive(false);
                        }
                        isCorrect = true;
                        NowQuizFlg = false;
                        imgSel.gameObject.SetActive(false);
                        EventCount++;
                        SetNextText(quizEvent[EventRand].eventData, null, null);

                    }
                    //不正解なら
                    else if (quizEvent[EventRand].Answer[Sel] != BestAnswer[EventRand] && NowQuizFlg)
                    {
                        SEManager.Instance.Play(SEPath.QUIZ_BAD);

                        for (int i = 0; i < 3; i++)
                        {
                            if (i == Sel)
                            {
                                continue;
                            }
                            txtAnswer[i].gameObject.SetActive(false);
                            imgQuizSpace[i].gameObject.SetActive(false);
                        }
                        //不正解テキストまでスキップ
                        while (!quizEvent[EventRand].eventData[EventCount].AnswerText)
                        {
                            EventCount++;
                        }
                        isCorrect = false;
                        imgSel.gameObject.SetActive(false);
                        NowQuizFlg = false;
                        SetNextText(quizEvent[EventRand].eventData, null, null);
                    }
                    //次がやるき上昇テキストなら
                    else if(quizEvent[EventRand].eventData[EventCount + 1].Message[3] == '褒')
                    {
                        EventCount++;
                        SEManager.Instance.Play(SEPath.YARUKI_UP);
                        manager.characters[CharaNo - 1].yaruki += quizEvent[EventRand].ChangeYaruki;
                        if (manager.characters[CharaNo - 1].yaruki > 10)
                        {
                            manager.characters[CharaNo - 1].yaruki = 10;
                        }
                        txtYaruki.text = manager.characters[CharaNo - 1].yaruki.ToString();
                        StartCoroutine("UpDown");

                        SetNextText(quizEvent[EventRand].eventData, null, null);
                    }
                    else
                    {
                        EventCount++;
                        if(quizEvent[EventRand].eventData[EventCount].QuizSet == true)
                        {
                            SEManager.Instance.Play(SEPath.QUIZ);
                        }
                        else
                        {
                            SEManager.Instance.Play(SEPath.PUSH_B);
                        }

                        //正解テキストが終了
                        if (quizEvent[EventRand].eventData[EventCount].AnswerText && isCorrect)
                        {
                            //イベント終了
                            txtMessage.gameObject.SetActive(false);
                            txtTextName.gameObject.SetActive(false);
                            QuizFlg = false;
                            //フェード開始してイベ終了
                            FeedInFlg = true;
                        }
                        else
                        {
                            SetNextText(quizEvent[EventRand].eventData, null, null);
                        }
                    }
                }
                else
                {
                    txtMessage.text = quizEvent[EventRand].eventData[EventCount].Message;
                    NextTextFlg = true;
                    StopCoroutine("Novel");
                }
            }
        }
        //ハロウィン
        if (HalloweenFlg)
        {
            imgHalloweenSel.transform.localPosition = new Vector3(100.0f, -380 + (Sel * -90.0f), 0.0f);
            //選択の矢印が出てるとき
            if (imgHalloweenSel.gameObject.activeSelf)
            {
                if (v > 0 && BeforeAxis == 0.0f)
                {
                    if (Sel > 0)
                    {
                        SEManager.Instance.Play(SEPath.CURSOR_CAHAGE);
                        Sel--;
                    }
                }
                if (v < 0 && BeforeAxis == 0.0f)
                {
                    if (Sel < 1)
                    {
                        SEManager.Instance.Play(SEPath.CURSOR_CAHAGE);
                        Sel++;
                    }
                }
            }
            //やる気使う量設定とその時の文字列
            if (imgYaruki.gameObject.activeSelf)
            {
                txtTrick.text = "  ×" + UseYaruki.ToString();
                txtNanimoSinai.text = "成功率" + SuccessRate + "%";
                SuccessRate = 10 * UseYaruki + halloweenEvent[EventRand].Correction;
                if (SuccessRate >= 100)
                {
                    SuccessRate = 100;
                }
                if (v > 0 && BeforeAxis == 0.0f)
                {
                    if (UseYaruki < manager.characters[CharaNo - 1].yaruki)
                    {
                        SEManager.Instance.Play(SEPath.CURSOR_CAHAGE);
                        UseYaruki++;
                    }
                }
                if (v < 0 && BeforeAxis == 0.0f)
                {
                    if (UseYaruki > 1)
                    {
                        SEManager.Instance.Play(SEPath.CURSOR_CAHAGE);
                        UseYaruki--;
                    }
                }

            }
            if (isInput && !FeedInFlg && !FeedOutFlg && Input.GetButtonDown("BtnA"))
            {
                if (NextTextFlg)
                {
                    NextTextFlg = false;
                    //次のテキストがない
                    if (EventCount + 1 == halloweenEvent[EventRand].eventData.Count)
                    {
                        SEManager.Instance.Play(SEPath.PUSH_B);
                        //イベント終了
                        txtMessage.gameObject.SetActive(false);
                        txtTextName.gameObject.SetActive(false);
                        HalloweenFlg = false;
                        //フェード開始してイベ終了
                        FeedInFlg = true;
                    }
                    //やる気が0なら
                    else if (NoYaruki)
                    {
                        SEManager.Instance.Play(SEPath.PUSH_B);
                        //イベント終了
                        txtMessage.gameObject.SetActive(false);
                        txtTextName.gameObject.SetActive(false);
                        HalloweenFlg = false;
                        NoYaruki = false;
                        //フェード開始してイベ終了
                        FeedInFlg = true;
                    }
                    //最初の選択肢表示したいタイミングに来た時
                    else if (halloweenEvent[EventRand].eventData[EventCount].SetSelect == true && !imgHalloweenSel.gameObject.activeSelf)
                    {
                        SEManager.Instance.Play(SEPath.PUSH_B);
                        //選択肢表示
                        txtTrick.text = "Trick or Treat!";
                        txtNanimoSinai.text = "何もせずに帰る。";
                        imgHalloweenSel.gameObject.SetActive(true);
                        txtTrick.gameObject.SetActive(true);
                        txtNanimoSinai.gameObject.SetActive(true);
                        NextTextFlg = true;

                    }
                    //トリックオアトリートがおされたとき
                    else if (Sel == 0 && imgHalloweenSel.gameObject.activeSelf)
                    {

                        EventCount++;
                        SEManager.Instance.Play(SEPath.PUSH_B);

                        imgHalloweenSel.gameObject.SetActive(false);
                        txtTrick.gameObject.SetActive(false);
                        txtNanimoSinai.gameObject.SetActive(false);
                        SetNextText(null, null, halloweenEvent[EventRand].eventData);
                    }
                    //何もしないを選んだ時
                    else if (Sel == 1 && imgHalloweenSel.gameObject.activeSelf)
                    {

                        EventCount++;
                        SEManager.Instance.Play(SEPath.PUSH_B);

                        imgHalloweenSel.gameObject.SetActive(false);
                        txtTrick.gameObject.SetActive(false);
                        txtNanimoSinai.gameObject.SetActive(false);

                        NanimoSinai = true;
                        //何もしないテキストまでスキップ
                        while (!halloweenEvent[EventRand].eventData[EventCount].NanimoSinai)
                        {
                            EventCount++;
                        }

                        SetNextText(null, null, halloweenEvent[EventRand].eventData);
                    }
                    //トリックオアトリートを選択した後のテキストが終わった時
                    else if (halloweenEvent[EventRand].eventData[EventCount].SetYaruki == true && !imgYaruki.gameObject.activeSelf)
                    {
                        SEManager.Instance.Play(SEPath.PUSH_B);
                        txtTrick.text = "  ×" + UseYaruki.ToString();
                        SuccessRate = 10 * UseYaruki + halloweenEvent[EventRand].Correction;
                        txtNanimoSinai.text = "成功率" + SuccessRate + "%";

                        imgYaruki.gameObject.SetActive(true);
                        txtTrick.gameObject.SetActive(true);
                        txtNanimoSinai.gameObject.SetActive(true);
                        NextTextFlg = true;

                        SuccessRand = Random.Range(0, 100);
                        Debug.Log(SuccessRand);
                    }
                    //成功
                    else if (SuccessRand <= SuccessRate && imgYaruki.gameObject.activeSelf)
                    {
                        SEManager.Instance.Play(SEPath.CANDY_GET);
                        manager.characters[CharaNo - 1].yaruki -= UseYaruki;
                        if(manager.characters[CharaNo-1].yaruki < 0)
                        {
                            manager.characters[CharaNo - 1].yaruki = 0;
                        }
                        txtYaruki.text = manager.characters[CharaNo - 1].yaruki.ToString();

                        TrueTrick = true;
                        imgYaruki.gameObject.SetActive(false);
                        txtTrick.gameObject.SetActive(false);
                        txtNanimoSinai.gameObject.SetActive(false);

                        while (!halloweenEvent[EventRand].eventData[EventCount].TrueTrick)
                        {
                            EventCount++;
                        }
                        SetNextText(null, null, halloweenEvent[EventRand].eventData);
                    }
                    //失敗
                    else if (SuccessRand > SuccessRate && imgYaruki.gameObject.activeSelf)
                    {
                        SEManager.Instance.Play(SEPath.CANDY_NO_GET);
                        manager.characters[CharaNo - 1].yaruki -= UseYaruki;
                        if (manager.characters[CharaNo - 1].yaruki < 0)
                        {
                            manager.characters[CharaNo - 1].yaruki = 0;
                        }
                        txtYaruki.text = manager.characters[CharaNo - 1].yaruki.ToString();

                        FalseTrick = true;
                        imgYaruki.gameObject.SetActive(false);
                        txtTrick.gameObject.SetActive(false);
                        txtNanimoSinai.gameObject.SetActive(false);

                        while (!halloweenEvent[EventRand].eventData[EventCount].FalseTrick)
                        {
                            EventCount++;
                        }
                        SetNextText(null, null, halloweenEvent[EventRand].eventData);
                    }
                    //次がお菓子増量テキストなら
                    else if(halloweenEvent[EventRand].eventData[EventCount + 1].Message[1] == '菓')
                    {
                        EventCount++;
                        SEManager.Instance.Play(SEPath.CANDY_PLUS);
                        manager.characters[CharaNo - 1].candy += halloweenEvent[EventRand].ChangeCandy;
                        txtCandy.text = manager.characters[CharaNo - 1].candy.ToString();
                        StartCoroutine("UpDown");

                        SetNextText(null, null, halloweenEvent[EventRand].eventData);
                    }
                    else
                    {
                        EventCount++;
                        SEManager.Instance.Play(SEPath.PUSH_B);

                        //何もしないテキストが送られている中、何もしないフラグが入ってないテキストに到達したとき
                        if (NanimoSinai && !halloweenEvent[EventRand].eventData[EventCount].NanimoSinai)
                        {
                            NanimoSinai = false;
                            txtMessage.gameObject.SetActive(false);
                            txtTextName.gameObject.SetActive(false);
                            HalloweenFlg = false;
                            //フェード開始してイベ終了
                            FeedInFlg = true;
                        }
                        else if (TrueTrick && !halloweenEvent[EventRand].eventData[EventCount].TrueTrick)
                        {
                            TrueTrick = false;
                            txtMessage.gameObject.SetActive(false);
                            txtTextName.gameObject.SetActive(false);
                            HalloweenFlg = false;
                            //フェード開始してイベ終了
                            FeedInFlg = true;
                        }
                        else if (FalseTrick && !halloweenEvent[EventRand].eventData[EventCount].FalseTrick)
                        {
                            FalseTrick = false;
                            txtMessage.gameObject.SetActive(false);
                            txtTextName.gameObject.SetActive(false);
                            HalloweenFlg = false;
                            //フェード開始してイベ終了
                            FeedInFlg = true;
                        }
                        else
                        {
                            SetNextText(null, null, halloweenEvent[EventRand].eventData);
                        }
                    }
                }
                else if(NoYaruki == true)
                {
                    txtMessage.text = "尋ねてみたが留守のようだ。\nやる気を貯めてまた来よう！";
                    NextTextFlg = true;
                    StopCoroutine("Novel");
                }
                else
                {
                    txtMessage.text = halloweenEvent[EventRand].eventData[EventCount].Message;
                    NextTextFlg = true;
                    StopCoroutine("Novel");
                }
            }
        }
        BeforeAxis = v;

    }
    public void PlusEvent(int MyNo)
    {
        if (!PlusFlg)
        {
            BGMManager.Instance.Stop();
            CharaNo = MyNo + 1;
            //PlusListからランダムに要素を取得
            EventRand = PlusList[Random.Range(0, PlusList.Count)];
            //取得した番号は以降出てこない
            PlusList.Remove(EventRand);

            imgEventChara.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            PlusFlg = true;
            FeedInFlg = true;
        }
    }
    public void MinusEvent(int MyNo)
    {
        if (!MinusFlg)
        {
            BGMManager.Instance.Stop();
            CharaNo = MyNo + 1;
            //MinusListからランダムに要素を取得
            EventRand = MinusList[Random.Range(0, MinusList.Count)];
            //取得した番号は以降出てこない
            MinusList.Remove(EventRand);
            imgEventChara.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            MinusFlg = true;
            FeedInFlg = true;
        }
    }
    public void QuizEvent(int MyNo)
    {
        if (!QuizFlg)
        {
            BGMManager.Instance.Stop();
            CharaNo = MyNo + 1;
            //PlusListからランダムに要素を取得
            EventRand = QuizList[Random.Range(0, QuizList.Count)];
            //取得した番号は以降出てこない
            QuizList.Remove(EventRand);
            imgEventChara.transform.localPosition = new Vector3(600.0f, 0.0f, 0.0f);
            QuizFlg = true;
            FeedInFlg = true;
        }
    }
    public void HalloweenEvent(int MyNo)
    {
        if (!HalloweenFlg)
        {
            BGMManager.Instance.Stop();
            CharaNo = MyNo + 1;
            EventRand = Random.Range(0, halloweenEvent.Count);
            imgEventChara.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            HalloweenFlg = true;
            FeedInFlg = true;
        }
    }

    void EndEvent()
    {
        //プレイヤー側の終了関数
        manager.EndEvent();
        BGMManager.Instance.Stop();
        BGMManager.Instance.Play(BGMPath.MAIN_BGM);
        PlusFlg = false;
        MinusFlg = false;
        QuizFlg = false;
        HalloweenFlg = false;
        EventCount = 0;
        Sel = 0;
        UseYaruki = 1;

        NanimoSinai = false;
        NoYaruki = false;
        TrueTrick = false;
        FalseTrick = false;
    }
    //1文字ずつ表示する処理
    IEnumerator Novel(string NowMessage)
    {
        int messageCount = 0; //現在表示中の文字数
        txtMessage.text = ""; //テキストのリセット
        while (NowMessage.Length > messageCount)//文字をすべて表示していない場合ループ
        {
            txtMessage.text += NowMessage[messageCount];//一文字追加
            messageCount++;//現在の文字数
            yield return new WaitForSeconds(NovelSpeed);//任意の時間待つ
        }
        NextTextFlg = true;
    }
    IEnumerator UpDown()
    {
        float R = imgUpDown.color.r;
        float G = imgUpDown.color.g;
        float B = imgUpDown.color.b;

        float A = 1.0f;
        float Y = 0.0f;
        if (PlusFlg || QuizFlg || HalloweenFlg)
        {
            imgUpDown.gameObject.SetActive(true);
            imgUpDown.sprite = Up;
            while (A != 0.0f)
            {
                imgUpDown.color = new Color(R, G, B, A);
                imgUpDown.transform.localPosition = new Vector3(-500.0f, 400.0f + Y, 0.0f);
                A -= 0.03f;
                Y += 2.0f;
                if (A < 0.0f)
                {
                    imgUpDown.gameObject.SetActive(false);
                    A = 0.0f;
                }
                yield return new WaitForSeconds(0.001f * Time.deltaTime);//任意の時間待つ
            }
        }
        else if (MinusFlg)
        {
            imgUpDown.sprite = Down;
            imgUpDown.gameObject.SetActive(true);
            while (A != 0.0f)
            {
                imgUpDown.color = new Color(R, G, B, A);
                imgUpDown.transform.localPosition = new Vector3(-500.0f, 500.0f + Y, 0.0f);
                A -= 0.03f;
                Y -= 2.0f;
                if (A < 0.0f)
                {
                    imgUpDown.gameObject.SetActive(false);
                    A = 0.0f;
                }
                yield return new WaitForSeconds(0.001f * Time.deltaTime);//任意の時間待つ
            }
        }
        Debug.Log("Updown終了");
    }
    //UIを表示
    void ShowUI()
    {
        imgEventChara.gameObject.SetActive(true);
        imgTextSpace.gameObject.SetActive(true);
        imgCharaUI.gameObject.SetActive(true);
        txtPlayerName.gameObject.SetActive(true);
        txtCandy.gameObject.SetActive(true);
        txtYaruki.gameObject.SetActive(true);
        imgBack.gameObject.SetActive(true);

        txtPlayerName.text = "プレイヤー" + CharaNo.ToString();
        txtCandy.text = manager.characters[CharaNo - 1].candy.ToString();
        txtYaruki.text = manager.characters[CharaNo - 1].yaruki.ToString();

        if (PlusFlg)
        {
            imgBack.sprite = Back[0];
            BGMManager.Instance.Play(BGMPath.PLUS_BGM);
            SEManager.Instance.Play(SEPath.PLUS);
            if (plusEvent[EventRand].eventData[EventCount].SpriteEventChara != null)
            {
                imgEventChara.sprite = plusEvent[EventRand].eventData[EventCount].SpriteEventChara;
            }
            //名前変更
            txtTextName.text = plusEvent[EventRand].eventData[EventCount].TextName;
        }
        else if (MinusFlg)
        {
            imgBack.sprite = Back[1];
            BGMManager.Instance.Play(BGMPath.MINUS_BGM);
            SEManager.Instance.Play(SEPath.MINUS);
            if (minusEvent[EventRand].eventData[EventCount].SpriteEventChara != null)
            {
                imgEventChara.sprite = minusEvent[EventRand].eventData[EventCount].SpriteEventChara;
            }
            //名前変更
            txtTextName.text = minusEvent[EventRand].eventData[EventCount].TextName;
        }
        else if (QuizFlg)
        {
            imgBack.sprite = Back[2];
            BGMManager.Instance.Play(BGMPath.QUIZ_BGM);
            if (quizEvent[EventRand].eventData[EventCount].SpriteEventChara != null)
            {
                imgEventChara.sprite = quizEvent[EventRand].eventData[EventCount].SpriteEventChara;
            }
            //名前変更
            txtTextName.text = quizEvent[EventRand].eventData[EventCount].TextName;
        }
        else if (HalloweenFlg)
        {
            imgBack.sprite = Back[3];
            BGMManager.Instance.Play(BGMPath.HALLOWIN_BGM);

            if (halloweenEvent[EventRand].eventData[EventCount].SpriteEventChara != null)
            {
                imgEventChara.sprite = halloweenEvent[EventRand].eventData[EventCount].SpriteEventChara;
            }
            //名前変更
            txtTextName.text = halloweenEvent[EventRand].eventData[EventCount].TextName;
        }

    }
    void HideUI()
    {
        imgBbtn.gameObject.SetActive(false);
        imgEventChara.gameObject.SetActive(false);
        imgTextSpace.gameObject.SetActive(false);
        txtMessage.gameObject.SetActive(false);
        txtTextName.gameObject.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            txtAnswer[i].gameObject.SetActive(false);
            imgQuizSpace[i].gameObject.SetActive(false);
        }
        imgSel.gameObject.SetActive(false);
        imgCharaUI.gameObject.SetActive(false);
        txtPlayerName.gameObject.SetActive(false);
        txtCandy.gameObject.SetActive(false);
        txtYaruki.gameObject.SetActive(false);
        imgHalloweenSel.gameObject.SetActive(false);
        txtTrick.gameObject.SetActive(false);
        txtNanimoSinai.gameObject.SetActive(false);
        imgYaruki.gameObject.SetActive(false);
        imgBack.gameObject.SetActive(false);
        imgUpDown.gameObject.SetActive(false);
    }
    //次のテキストデータを表示
    void SetNextText(List<QuizEventData> Q, List<EventData> S, List<HalloweenEventData> H)
    {
        if (Q != null)
        {
            StartCoroutine("Novel", Q[EventCount].Message);
            //キャラ画像が設定されてるなら変更
            if (Q[EventCount].SpriteEventChara != null)
            {
                imgEventChara.sprite = Q[EventCount].SpriteEventChara;
            }
            //名前変更
            if (Q[EventCount].TextName == "プレイヤー")
            {
                txtTextName.text = Q[EventCount].TextName + CharaNo.ToString();
            }
            else
            {
                txtTextName.text = Q[EventCount].TextName;
            }
            //音があるなら鳴らす
            if (Q[EventCount].SEPath != null)
            {
                SEManager.Instance.Play("SE/SE/" + Q[EventCount].SEPath);
            }

        }
        if (S != null)
        {
            StartCoroutine("Novel", S[EventCount].Message);
            //キャラ画像が設定されてるなら変更
            if (S[EventCount].SpriteEventChara != null)
            {
                imgEventChara.sprite = S[EventCount].SpriteEventChara;
            }
            //名前変更
            if (S[EventCount].TextName == "プレイヤー")
            {
                txtTextName.text = S[EventCount].TextName + CharaNo.ToString();
            }
            else
            {
                txtTextName.text = S[EventCount].TextName;
            }
            //音があるなら鳴らす
            if (S[EventCount].SEPath != null)
            {
                SEManager.Instance.Play("SE/SE/" + S[EventCount].SEPath);
            }

        }
        if (H != null)
        {
            StartCoroutine("Novel", H[EventCount].Message);
            //キャラ画像が設定されてるなら変更
            if (H[EventCount].SpriteEventChara != null)
            {
                imgEventChara.sprite = H[EventCount].SpriteEventChara;
            }
            //名前変更
            if (H[EventCount].TextName == "プレイヤー")
            {
                txtTextName.text = H[EventCount].TextName + CharaNo.ToString();
            }
            else
            {
                txtTextName.text = H[EventCount].TextName;
            }
        }
    }

    void AddEventRandList()
    {
        if (PlusList.Count == 0)
        {
            for (int i = 0; i < plusEvent.Count; i++)
            {
                PlusList.Add(i);
            }
        }
        if (MinusList.Count == 0)
        {
            for (int i = 0; i < minusEvent.Count; i++)
            {
                MinusList.Add(i);
            }
        }
        if (QuizList.Count == 0)
        {
            for (int i = 0; i < quizEvent.Count; i++)
            {
                QuizList.Add(i);
            }
        }

    }
    public void FeedIn()
    {
        //if(alfa <= 1.0f) 
        if(isResult&&i == 11)
        {
            alfa += 0.5f * Time.deltaTime;
        }
        else
        {
            alfa += FeedSpeed * Time.deltaTime;
        }
    }
    public void FeedOut()
    {
        //if (alfa >= 1.0f) 
        if (FirstFeed)
        {
            alfa -= 0.6f * Time.deltaTime;
        }
        else
        {
            alfa -= FeedSpeed * Time.deltaTime;
        }
    }

}
