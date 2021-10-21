using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

//イベント内容
[System.Serializable]
public class EventData
{
    [Tooltip("イベントのキャラ画像")] public Sprite SpriteEventChara;
    [TextArea(1, 3), Tooltip("テキスト内容")] public string Message;
    [Tooltip("名前テキスト")] public string PlayerName;
}
//イベント内容にクイズが出てくるタイミングフラグ、正解テキストかどうかの判断フラグを追加
[System.Serializable]
public class QuizEventData : EventData
{
    [Tooltip("クイズを出すタイミングでチェック")] public bool QuizSet;
    [Tooltip("不正解のテキストに全てチェック")] public bool AnswerText;
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

public class SpuareAction : MonoBehaviour
{
    [SerializeField] float NovelSpeed;
    [SerializeField] Image imgEventChara;
    [SerializeField] Image imgTextSpace;
    [SerializeField] Image imgBbtn;
    [SerializeField] Image imgSel;
    [SerializeField] Text txtMessage;
    [SerializeField] Text txtPlayerName;
    [SerializeField] Image[] imgQuizSpace;
    [SerializeField] Text[] txtAnswer;

    [Space(50)]

    [SerializeField, Tooltip("Sizeにイベントの種類の個数を入力")] List<SpuareEvent> plusEvent = new List<SpuareEvent>();
    [Space(30)]
    [SerializeField, Tooltip("Sizeにイベントの種類の個数を入力")] List<SpuareEvent> minusEvent = new List<SpuareEvent>();
    [Space(30)]
    [SerializeField, Tooltip("Sizeにイベントの種類の個数を入力")] List<QuizEvent> quizEvent = new List<QuizEvent>();
    [Space(30)]
    [SerializeField, Tooltip("Sizeにイベントの種類の個数を入力")] List<SpuareEvent> halloweenEvent = new List<SpuareEvent>();

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
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0;i<quizEvent.Count;i++)
        {
            BestAnswer[i] = quizEvent[i].Answer[0];
        }
        Sel = 0;
        NextTextFlg = false;
        //最初は非表示
        imgBbtn.gameObject.SetActive(false);
        imgEventChara.gameObject.SetActive(false);
        imgTextSpace.gameObject.SetActive(false);
        imgSel.gameObject.SetActive(false);
        txtMessage.gameObject.SetActive(false);
        txtPlayerName.gameObject.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            txtAnswer[i].gameObject.SetActive(false);
            imgQuizSpace[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(NextTextFlg)
        {
            imgBbtn.gameObject.SetActive(true);
        }
        else
        {
            imgBbtn.gameObject.SetActive(false);
        }
        //デバッグ用
        if(Input.GetKeyDown(KeyCode.P))
        {
            PlusEvent();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            MinusEvent();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            QuizEvent();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            HalloweenEvent();
        }

        if (PlusFlg)
        {
            PlusEvent();
        }
        if(MinusFlg)
        {
            MinusEvent();
        }
        if(QuizFlg)
        {
            QuizEvent();
        }
        if (HalloweenFlg)
        {
            HalloweenEvent();
        }

    }
    void PlusEvent()
    {
        if (!PlusFlg)
        {
            EventRand = Random.Range(0, plusEvent.Count);
            StartCoroutine("Novel", plusEvent[EventRand].eventData[EventCount].Message);
            imgEventChara.sprite = plusEvent[EventRand].eventData[EventCount].SpriteEventChara;
            txtPlayerName.text = plusEvent[EventRand].eventData[EventCount].PlayerName;
            imgEventChara.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            SetUI();
            PlusFlg = true;
        }
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("BtnB"))
        {
            if(NextTextFlg)
            {
                NextTextFlg = false;
                //次のテキストがない
                if(EventCount + 1 == plusEvent[EventRand].eventData.Count)
                {
                    //イベント終了
                    EndEvent();
                }
                else
                {
                    EventCount++;
                    StartCoroutine("Novel", plusEvent[EventRand].eventData[EventCount].Message);
                    //キャラ画像が設定されてるなら変更
                    if (plusEvent[EventRand].eventData[EventCount].SpriteEventChara != null)
                    {
                        imgEventChara.sprite = plusEvent[EventRand].eventData[EventCount].SpriteEventChara;
                    }
                    //名前変更
                    txtPlayerName.text = plusEvent[EventRand].eventData[EventCount].PlayerName;
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
    void MinusEvent()
    {
        if (!MinusFlg)
        {
            EventRand = Random.Range(0, minusEvent.Count);
            StartCoroutine("Novel", minusEvent[EventRand].eventData[EventCount].Message);
            imgEventChara.sprite = minusEvent[EventRand].eventData[EventCount].SpriteEventChara;
            txtPlayerName.text = minusEvent[EventRand].eventData[EventCount].PlayerName;
            imgEventChara.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            SetUI();
            MinusFlg = true;
        }
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("BtnB"))
        {
            if (NextTextFlg)
            {
                NextTextFlg = false;
                //次のテキストがない
                if (EventCount + 1 == minusEvent[EventRand].eventData.Count)
                {
                    //イベント終了
                    EndEvent();

                }
                else
                {
                    EventCount++;
                    StartCoroutine("Novel", minusEvent[EventRand].eventData[EventCount].Message);
                    //キャラ画像が設定されてるなら変更
                    if (minusEvent[EventRand].eventData[EventCount].SpriteEventChara != null)
                    {
                        imgEventChara.sprite = minusEvent[EventRand].eventData[EventCount].SpriteEventChara;
                    }
                    //名前変更
                    txtPlayerName.text = minusEvent[EventRand].eventData[EventCount].PlayerName;
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
    void QuizEvent()
    {
        if (!QuizFlg)
        {
            EventRand = Random.Range(0, quizEvent.Count);
            StartCoroutine("Novel", quizEvent[EventRand].eventData[EventCount].Message);
            imgEventChara.sprite = quizEvent[EventRand].eventData[EventCount].SpriteEventChara;
            txtPlayerName.text = quizEvent[EventRand].eventData[EventCount].PlayerName;
            imgEventChara.transform.localPosition = new Vector3(500.0f, 0.0f, 0.0f);
            SetUI();
            QuizFlg = true;
        }
        //選択の矢印が出てるとき
        if(imgSel.gameObject.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (Sel > 0)
                {
                    Sel--;
                    imgSel.transform.localPosition += new Vector3(0.0f, 200.0f, 0.0f);
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (Sel < 2)
                {
                    Sel++;
                    imgSel.transform.localPosition += new Vector3(0.0f, -200.0f, 0.0f);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Return)||Input.GetButtonDown("BtnB"))
        {
            if (NextTextFlg)
            {
                NextTextFlg = false;
                //次のテキストがない
                if (EventCount + 1 == quizEvent[EventRand].eventData.Count)
                {
                    //イベント終了
                    EndEvent();
                }
                //クイズあるなら出す
                else if (quizEvent[EventRand].eventData[EventCount].QuizSet == true && !NowQuizFlg)
                {
                    //配列をシャッフル
                    quizEvent[EventRand].Answer = quizEvent[EventRand].Answer.OrderBy(i => System.Guid.NewGuid()).ToArray();
                    for (int i = 0; i < 3; i++)
                    {
                        txtAnswer[i].gameObject.SetActive(true);
                        imgQuizSpace[i].gameObject.SetActive(true);
                        txtAnswer[i].text = quizEvent[EventRand].Answer[i];
                    }
                    imgSel.gameObject.SetActive(true);
                    NextTextFlg = true;
                    NowQuizFlg = true;
                }
                //正解なら
                else if (quizEvent[EventRand].Answer[Sel] == BestAnswer[EventRand] && NowQuizFlg)
                {
                    isCorrect = true;
                    NowQuizFlg = false;
                    imgSel.gameObject.SetActive(false);
                    EventCount++;
                    StartCoroutine("Novel", quizEvent[EventRand].eventData[EventCount].Message);
                    //キャラ画像が設定されてるなら変更
                    if (quizEvent[EventRand].eventData[EventCount].SpriteEventChara != null)
                    {
                        imgEventChara.sprite = quizEvent[EventRand].eventData[EventCount].SpriteEventChara;
                    }
                    //名前変更
                    txtPlayerName.text = quizEvent[EventRand].eventData[EventCount].PlayerName;

                }
                //不正解なら
                else if (quizEvent[EventRand].Answer[Sel] != BestAnswer[EventRand] && NowQuizFlg)
                {
                    //不正解テキストまでスキップ
                    while (!quizEvent[EventRand].eventData[EventCount].AnswerText)
                    {
                        EventCount++;
                    }
                    isCorrect = false;
                    imgSel.gameObject.SetActive(false);
                    NowQuizFlg = false;
                    StartCoroutine("Novel", quizEvent[EventRand].eventData[EventCount].Message);
                    //キャラ画像が設定されてるなら変更
                    if (quizEvent[EventRand].eventData[EventCount].SpriteEventChara != null)
                    {
                        imgEventChara.sprite = quizEvent[EventRand].eventData[EventCount].SpriteEventChara;
                    }
                    //名前変更
                    txtPlayerName.text = quizEvent[EventRand].eventData[EventCount].PlayerName;
                }
                else
                {
                    EventCount++;
                    if (quizEvent[EventRand].eventData[EventCount].AnswerText && isCorrect)
                    {
                        EndEvent();
                    }
                    else
                    {
                        StartCoroutine("Novel", quizEvent[EventRand].eventData[EventCount].Message);
                        //キャラ画像が設定されてるなら変更
                        if (quizEvent[EventRand].eventData[EventCount].SpriteEventChara != null)
                        {
                            imgEventChara.sprite = quizEvent[EventRand].eventData[EventCount].SpriteEventChara;
                        }
                        //名前変更
                        txtPlayerName.text = quizEvent[EventRand].eventData[EventCount].PlayerName;
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
    void HalloweenEvent()
    {
        if (!HalloweenFlg)
        {
            EventRand = Random.Range(0, halloweenEvent.Count);
            StartCoroutine("Novel", halloweenEvent[EventRand].eventData[EventCount].Message);
            imgEventChara.sprite = halloweenEvent[EventRand].eventData[EventCount].SpriteEventChara;
            txtPlayerName.text = halloweenEvent[EventRand].eventData[EventCount].PlayerName;
            imgEventChara.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            SetUI();
            HalloweenFlg = true;
        }
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("BtnB"))
        {
            if (NextTextFlg)
            {
                NextTextFlg = false;
                //次のテキストがない
                if (EventCount + 1 == halloweenEvent[EventRand].eventData.Count)
                {
                    //イベント終了
                    EndEvent();
                }
                else
                {
                    EventCount++;
                    StartCoroutine("Novel", halloweenEvent[EventRand].eventData[EventCount].Message);
                    //キャラ画像が設定されてるなら変更
                    if (halloweenEvent[EventRand].eventData[EventCount].SpriteEventChara != null)
                    {
                        imgEventChara.sprite = halloweenEvent[EventRand].eventData[EventCount].SpriteEventChara;
                    }
                    //名前変更
                    txtPlayerName.text = halloweenEvent[EventRand].eventData[EventCount].PlayerName;
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

    void EndEvent()
    {
        imgEventChara.gameObject.SetActive(false);
        imgTextSpace.gameObject.SetActive(false);
        txtMessage.gameObject.SetActive(false);
        txtPlayerName.gameObject.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            txtAnswer[i].gameObject.SetActive(false);
            imgQuizSpace[i].gameObject.SetActive(false);
        }
        imgSel.gameObject.SetActive(false);
        PlusFlg = false;
        MinusFlg = false;
        QuizFlg = false;
        HalloweenFlg = false;
        EventCount = 0;
        Sel = 0;
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
    void SetUI()
    {
        imgEventChara.gameObject.SetActive(true);
        imgTextSpace.gameObject.SetActive(true);
        txtMessage.gameObject.SetActive(true);
        txtPlayerName.gameObject.SetActive(true);
    }
}
