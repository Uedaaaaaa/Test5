using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpuareAction : MonoBehaviour
{
    [SerializeField] float NovelSpeed;
    [SerializeField] Image imgEventChara;
    [SerializeField] Image imgTextSpace;
    [SerializeField] Image imgBbtn;
    [SerializeField] Text txtMessage;
    [SerializeField] Text txtPlayerName;
    [System.Serializable]
    public class Event
    {
        [SerializeField] string EventName;
        [System.Serializable]
        public class EventData
        {
            public Sprite SpriteEventChara;
            [TextArea(1,3)]public string Message;
            public string PlayerName;
        }
        [Header("Sizeにテキストの数を入力")]
        public List<EventData> eventData;
    }
    [Header("Sizeにイベントの個数を入力")]
    [SerializeField] List<Event> plusEvent = new List<Event>();
    [SerializeField] List<Event> minusEvent = new List<Event>();

    private int EventRand;//どのイベントを行うかの乱数
    private int EventCount;//何番目のテキストか
    private bool PlusFlg;
    private bool MinusFlg;
    private bool NextTextFlg;

    // Start is called before the first frame update
    void Start()
    {
        PlusFlg = false;
        MinusFlg = false;
        NextTextFlg = false;
        //最初は非表示
        imgBbtn.gameObject.SetActive(false);
        imgEventChara.gameObject.SetActive(false);
        imgTextSpace.gameObject.SetActive(false);
        txtMessage.gameObject.SetActive(false);
        txtPlayerName.gameObject.SetActive(false);

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

        if (PlusFlg)
        {
            PlusEvent();
        }
        if(MinusFlg)
        {
            MinusEvent();
        }
    }
    void PlusEvent()
    {
        if (!PlusFlg)
        {
            DoEvent(plusEvent);
            PlusFlg = true;
        }
        if(Input.GetKeyDown(KeyCode.Return))
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
                    StartCoroutine("Novel", plusEvent);
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
            DoEvent(minusEvent);
            MinusFlg = true;
        }
        if (Input.GetKeyDown(KeyCode.Return))
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
                    StartCoroutine("Novel", minusEvent);
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
    void EndEvent()
    {
        imgEventChara.gameObject.SetActive(false);
        imgTextSpace.gameObject.SetActive(false);
        txtMessage.gameObject.SetActive(false);
        txtPlayerName.gameObject.SetActive(false);

        PlusFlg = false;
        MinusFlg = false;
        EventCount = 0;
    }
    //1文字ずつ表示する処理
    IEnumerator Novel(List<Event> eventName)
    {
        int messageCount = 0; //現在表示中の文字数
        txtMessage.text = ""; //テキストのリセット
        while (eventName[EventRand].eventData[EventCount].Message.Length > messageCount)//文字をすべて表示していない場合ループ
        {
            txtMessage.text += eventName[EventRand].eventData[EventCount].Message[messageCount];//一文字追加
            messageCount++;//現在の文字数
            yield return new WaitForSeconds(NovelSpeed);//任意の時間待つ
        }
        NextTextFlg = true;
    }
    //イベント最初の処理
    void DoEvent(List<Event> eventName)
    {
        StartCoroutine("Novel", eventName);
        imgEventChara.sprite = eventName[EventRand].eventData[EventCount].SpriteEventChara;
        txtPlayerName.text = eventName[EventRand].eventData[EventCount].PlayerName;
        imgEventChara.gameObject.SetActive(true);
        imgTextSpace.gameObject.SetActive(true);
        txtMessage.gameObject.SetActive(true);
        txtPlayerName.gameObject.SetActive(true);
        EventRand = Random.Range(0, eventName.Count - 1);
    }
}
