using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using KanKikuchi.AudioManager;

//イベント内容
[System.Serializable]
public class EventData
{
    [Tooltip("SE")] public string SEPath;
    [Tooltip("イベントのキャラ画像")] public Sprite SpriteEventChara;
    [TextArea(1, 3), Tooltip("テキスト内容")] public string Message;
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
    [Tooltip("やる気0ならこのテキストに飛ぶ")] public bool NoYaruki;
    [Tooltip("トリックオアトリート成功時")] public bool TrueTrick;
    [Tooltip("トリックオアトリート失敗時")] public bool FalseTrick;
}
[System.Serializable]
//プラス、マイナスイベント
public class SpuareEvent
{
    [SerializeField, Tooltip("イベントの名前")] string EventName;

    [Tooltip("Sizeにテキストの数を入力")] public List<EventData> eventData;
}
[System.Serializable]
//クイズイベント(プラスマイナスマスに回答をプラス)
public class QuizEvent
{
    [SerializeField, Tooltip("イベントの名前")] string EventName;
    [Tooltip("クイズの選択肢　一番上に答えを入力")]public string[] Answer = new string[3] { "","",""};

    [Tooltip("Sizeにテキストの数を入力")] public List<QuizEventData> eventData;

}
[System.Serializable]
//ハロウィンイベント
public class HalloweenEvent
{
    [SerializeField, Tooltip("イベントの名前")] string EventName;
    [Tooltip("補正値")] public int Correction;

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
    [Space(20)]

    [SerializeField,TextArea(1, 3)] string[] RuleText;

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
    private bool FeedInFlg = false;
    private bool FeedOutFlg = true;
    private bool isRule = true;
    private bool isResult = false;

    private bool NanimoSinai;
    private bool NoYaruki;
    private bool TrueTrick;
    private bool FalseTrick;
    private bool isInput;

    private int CharaNo;
    private int UseYaruki;
    private int SuccessRate;
    private int SuccessRand;
    private int i;
    private GameManager manager;
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

        manager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
        for (int i = 0; i < quizEvent.Count; i++)
        {
            BestAnswer[i] = quizEvent[i].Answer[0];
        }
        Sel = 0;
        UseYaruki = 1;
        NextTextFlg = false;
        isInput = true;
        //最初は非表示
        HideUI();
        //キャラとテキストスペースは表示
        imgEventChara.gameObject.SetActive(true);
        imgTextSpace.gameObject.SetActive(true);

        red = Feed.color.r;
        green = Feed.color.g;
        blue = Feed.color.b;
        alfa = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Feed.color = new Color(red, green, blue, alfa);
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
                    ShowUI();
                    txtPlayerName.text = "プレイヤー" + CharaNo.ToString();
                    txtCandy.text = manager.characters[CharaNo - 1].Candy.ToString();
                    txtYaruki.text = manager.characters[CharaNo - 1].Yaruki.ToString();
                }
                else
                {
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
                FeedOutFlg = false;
                //フェードアウトが完了したら文字が流れ始める
                if (PlusFlg)
                {
                    txtMessage.gameObject.SetActive(true);
                    txtTextName.gameObject.SetActive(true);
                    StartCoroutine("Novel", plusEvent[EventRand].eventData[EventCount].Message);
                    PlusFlg = true;
                }
                else if (MinusFlg)
                {
                    txtMessage.gameObject.SetActive(true);
                    txtTextName.gameObject.SetActive(true);
                    StartCoroutine("Novel", minusEvent[EventRand].eventData[EventCount].Message);
                    MinusFlg = true;
                }
                else if (QuizFlg)
                {
                    txtMessage.gameObject.SetActive(true);
                    txtTextName.gameObject.SetActive(true);
                    StartCoroutine("Novel", quizEvent[EventRand].eventData[EventCount].Message);
                    QuizFlg = true;
                }
                else if (HalloweenFlg)
                {
                    //manager.characters[CharaNo].Yaruki = 0;
                    txtMessage.gameObject.SetActive(true);
                    txtTextName.gameObject.SetActive(true);
                    StartCoroutine("Novel", halloweenEvent[EventRand].eventData[EventCount].Message);
                    HalloweenFlg = true;
                }
                else if(isRule)
                {
                    BGMManager.Instance.Play(BGMPath.RULE_BGM);
                    txtTextName.text = "カボチャ";
                    txtMessage.gameObject.SetActive(true);
                    txtTextName.gameObject.SetActive(true);
                    StartCoroutine("Novel",RuleText[0]);
                }
                else
                {
                    EndEvent();
                }

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
        //デバッグ用
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlusEvent(1);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            MinusEvent(2);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            QuizEvent(3);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            HalloweenEvent(3);
        }
        //ルール説明中
        if(isRule)
        {
            if (!FeedInFlg && !FeedOutFlg && isInput&&Input.GetKeyDown(KeyCode.Return) || !FeedInFlg && !FeedOutFlg && Input.GetButtonDown("BtnB"))
            {
                if(NextTextFlg)
                {
                    if (i == 6)
                    {
                        i = 7;
                        isInput = false;
                        SEManager.Instance.Play(SEPath.PUSH_B);
                        manager.SpawnDice();
                        imgTextSpace.gameObject.SetActive(false);
                        txtMessage.gameObject.SetActive(false);
                        imgEventChara.gameObject.SetActive(false);
                        Debug.Log("aaa");
                    }
                    else
                    {
                        SEManager.Instance.Play(SEPath.PUSH_B);
                        NextTextFlg = false;
                        i++;
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
            //プラスイベント処理
            if (PlusFlg)
        {
            if (!FeedInFlg && !FeedOutFlg && Input.GetKeyDown(KeyCode.Return) || !FeedInFlg && !FeedOutFlg && Input.GetButtonDown("BtnB"))
            {
                if (NextTextFlg)
                {
                    NextTextFlg = false;
                    //次のテキストがない
                    if (EventCount + 1 == plusEvent[EventRand].eventData.Count)
                    {
                        //イベント終了
                        txtMessage.gameObject.SetActive(false);
                        txtTextName.gameObject.SetActive(false);
                        PlusFlg = false;
                        //フェード開始してイベ終了
                        FeedInFlg = true;
                    }
                    else
                    {
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
            if (!FeedInFlg && !FeedOutFlg && Input.GetKeyDown(KeyCode.Return) || !FeedInFlg && !FeedOutFlg && Input.GetButtonDown("BtnB"))
            {
                if (NextTextFlg)
                {
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
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (Sel > 0)
                    {
                        Sel--;
                    }
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (Sel < 2)
                    {
                        Sel++;
                    }
                }
            }
            if (!FeedInFlg && !FeedOutFlg && Input.GetKeyDown(KeyCode.Return) || !FeedInFlg && !FeedOutFlg && Input.GetButtonDown("BtnB"))
            {
                if (NextTextFlg)
                {
                    NextTextFlg = false;
                    //次のテキストがない
                    if (EventCount + 1 == quizEvent[EventRand].eventData.Count)
                    {
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
                    else
                    {
                        EventCount++;
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
        if (HalloweenFlg)
        {
            imgHalloweenSel.transform.localPosition = new Vector3(50.0f, -380 + (Sel * -90.0f), 0.0f);
            //選択の矢印が出てるとき
            if (imgHalloweenSel.gameObject.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (Sel > 0)
                    {
                        Sel--;
                    }
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (Sel < 1)
                    {
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
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (UseYaruki < manager.characters[CharaNo - 1].Yaruki)
                    {
                        UseYaruki++;
                    }
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (UseYaruki > 1)
                    {
                        UseYaruki--;
                    }
                }

            }
            if (!FeedInFlg && !FeedOutFlg && Input.GetKeyDown(KeyCode.Return) || !FeedInFlg && !FeedOutFlg && Input.GetButtonDown("BtnB"))
            {
                if (NextTextFlg)
                {
                    NextTextFlg = false;
                    //次のテキストがない
                    if (EventCount + 1 == halloweenEvent[EventRand].eventData.Count)
                    {
                        //イベント終了
                        txtMessage.gameObject.SetActive(false);
                        txtTextName.gameObject.SetActive(false);
                        HalloweenFlg = false;
                        //フェード開始してイベ終了
                        FeedInFlg = true;
                    }
                    //やる気が0なら
                    else if (manager.characters[CharaNo - 1].Yaruki == 0 && !NoYaruki)
                    {
                        Debug.Log("Yaruki");
                        NoYaruki = true;
                        while (!halloweenEvent[EventRand].eventData[EventCount].NoYaruki)
                        {
                            EventCount++;
                        }
                        SetNextText(null, null, halloweenEvent[EventRand].eventData);
                    }
                    //最初の選択肢表示したいタイミングに来た時
                    else if (halloweenEvent[EventRand].eventData[EventCount].SetSelect == true && !imgHalloweenSel.gameObject.activeSelf)
                    {

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

                        imgHalloweenSel.gameObject.SetActive(false);
                        txtTrick.gameObject.SetActive(false);
                        txtNanimoSinai.gameObject.SetActive(false);
                        SetNextText(null, null, halloweenEvent[EventRand].eventData);
                    }
                    //何もしないを選んだ時
                    else if (Sel == 1 && imgHalloweenSel.gameObject.activeSelf)
                    {

                        EventCount++;

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
                    else
                    {
                        EventCount++;

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
                        else if (NoYaruki && !halloweenEvent[EventRand].eventData[EventCount].NoYaruki)
                        {
                            NoYaruki = false;
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
                else
                {
                    txtMessage.text = halloweenEvent[EventRand].eventData[EventCount].Message;
                    NextTextFlg = true;
                    StopCoroutine("Novel");
                }
            }
        }

    }
    public void PlusEvent(int MyNo)
    {
        if (!PlusFlg)
        {
            CharaNo = MyNo + 1;
            EventRand = Random.Range(0, plusEvent.Count);
            imgEventChara.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            PlusFlg = true;
            FeedInFlg = true;
        }
    }
    public void MinusEvent(int MyNo)
    {
        if (!MinusFlg)
        {
            CharaNo = MyNo + 1;
            EventRand = Random.Range(0, minusEvent.Count);
            imgEventChara.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            MinusFlg = true;
            FeedInFlg = true;
        }
    }
    public void QuizEvent(int MyNo)
    {
        if (!QuizFlg)
        {
            CharaNo = MyNo + 1;
            EventRand = Random.Range(0, quizEvent.Count);
            imgEventChara.transform.localPosition = new Vector3(500.0f, 0.0f, 0.0f);
            QuizFlg = true;
            FeedInFlg = true;
        }
    }
    public void HalloweenEvent(int MyNo)
    {
        if (!HalloweenFlg)
        {
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
    //UIを表示
    void ShowUI()
    {
        imgEventChara.gameObject.SetActive(true);
        imgTextSpace.gameObject.SetActive(true);
        imgCharaUI.gameObject.SetActive(true);
        txtPlayerName.gameObject.SetActive(true);
        txtCandy.gameObject.SetActive(true);
        txtYaruki.gameObject.SetActive(true);

        //txtPlayerName.text = "プレイヤー" + manager.characters.
        if (PlusFlg)
        {
            if (plusEvent[EventRand].eventData[EventCount].SpriteEventChara != null)
            {
                imgEventChara.sprite = plusEvent[EventRand].eventData[EventCount].SpriteEventChara;
            }
            //名前変更
            txtTextName.text = plusEvent[EventRand].eventData[EventCount].TextName;
        }
        else if (MinusFlg)
        {
            if (minusEvent[EventRand].eventData[EventCount].SpriteEventChara != null)
            {
                imgEventChara.sprite = minusEvent[EventRand].eventData[EventCount].SpriteEventChara;
            }
            //名前変更
            txtTextName.text = minusEvent[EventRand].eventData[EventCount].TextName;
        }
        else if (QuizFlg)
        {
            if (quizEvent[EventRand].eventData[EventCount].SpriteEventChara != null)
            {
                imgEventChara.sprite = quizEvent[EventRand].eventData[EventCount].SpriteEventChara;
            }
            //名前変更
            txtTextName.text = quizEvent[EventRand].eventData[EventCount].TextName;
        }
        else if (HalloweenFlg)
        {
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
    public void FeedIn()
    {
        //if(alfa <= 1.0f) 
        alfa += FeedSpeed * Time.deltaTime;
    }
    public void FeedOut()
    {
        //if (alfa >= 1.0f) 
        alfa -= FeedSpeed * Time.deltaTime;
    }

}
