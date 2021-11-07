using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KanKikuchi.AudioManager;
public class SetCharacerUI : MonoBehaviour
{
    //実際に表示するプレイヤーターンUI
    [SerializeField] Image ImagePlayerTurnUI;
    //[Header("プレイヤーのターンUIの移動速度")]
    //[SerializeField] float TurnUIMoveSpeed = 1.0f;

    //プレイヤーターンUIのスプライト獲得
    [SerializeField] Image[] PlayerTurnUI = new Image[4];

    //プレイヤーターンUI表示と同時にセットするBボタンUI
    [SerializeField] Image PlayerTurnBbuttonUI;

    //ダイスを振るを書かれたUI
    [SerializeField] Image[] DiceStartUI = new Image[4];

    //ダイスを止めると書かれたUI
    [SerializeField] Image DiceStopUI;
    [SerializeField] Image[] DiceStopUI_Order = new Image[4];
    //実際に表示するダイスの数字UI
    [SerializeField] Image DiceImage;

    //ダイスの数字UIのスプライト
    [SerializeField] Sprite[] DiceNumSprite = new Sprite[6];
    [Header("ダイスUIの位置")]
    [SerializeField] Vector3 DiceImagePos = new Vector3(0, 100, 0);

    //プレイヤーのステータスUI
    [SerializeField] Image[] PlayerStatusUI = new Image[4];
    //プレイヤーのやる気、キャンディの数
    [SerializeField] Text[] Candytxt = new Text[4];
    [SerializeField] Text[] Yarukitxt = new Text[4];

    //プレイヤーの名前テキストUI
    [SerializeField] Text[] PlayerNametxt = new Text[4];
    //ゲーム終了UI
    [SerializeField] Image GameSetUI;

    //ゲームマネージャー
    GameManager manager;

    //ゲーム終了を獲得するフラグ
    private bool GameSetflg;

    //スクエアアクション
    SpuareAction feed;
    private GameObject feedobj;
    // Start is called before the first frame update
    void Start()
    {
        GameSetflg = false;
        //ダイスの数字UIの高さを変更
        //DiceImagePos.y = 145;
        //プレイヤー以外のUI表示をオフにする
        ImagePlayerTurnUI.enabled = false;
        PlayerTurnBbuttonUI.enabled = false;
        DiceStopUI.enabled = false;
        DiceImage.enabled = false;
        GameSetUI.enabled = false;
        //プレイヤー関係のUIの非表示
        for (int i = 0; i < PlayerStatusUI.Length; i++)
        {
            DiceStopUI_Order[i].enabled = false;
            DiceStartUI[i].enabled = false;
            PlayerStatusUI[i].enabled = false;
            Candytxt[i].text = "";
            Yarukitxt[i].text = "";
            PlayerNametxt[i].text = "";
        }
        //ゲームマネージャーを獲得
        manager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
    }

    //ターンUIの表示非表示
    public void PlayerTurnUISet(int PlayerNum)
    {
        ImagePlayerTurnUI.enabled = true;
        ImagePlayerTurnUI.sprite = PlayerTurnUI[PlayerNum].sprite;
        SEManager.Instance.Play(SEPath.TURN_CHANGE);
        PlayerTurnBbuttonUI.enabled = true;
    }
    public void PlayerTurnUIDestroy()
    {
        SEManager.Instance.Play(SEPath.PUSH_B);
        ImagePlayerTurnUI.enabled = false;
        PlayerTurnBbuttonUI.enabled = false;
    }

    //ダイスを振るUIの表示非表示
    public void DiceStartUISet()
    {
        //ダイスを振るのUIの表示
        DiceStartUI[manager.NowPlayerNo].enabled = true;
    }
    public void DiceStartUIDestroy()
    {
        for(int i = 0; i< DiceStartUI.Length; i++)
        {
            DiceStartUI[i].enabled = false;
        }
    }

    //ダイスを止めるUIの表示非表示
    public void DiceStopUISet()
    {
        //UIの位置を変更する
        if(manager.gameStatus == GameSTS.OrderJudge)
        {
            DiceStopUI_Order[manager.NowPlayerNo].enabled = true;
        }
        else
        {
            //ダイスを止めるUIの表示
            DiceStopUI.enabled = true;
        }
    }
    public void DiceStopUIDestroy()
    {
        for(int i = 0; i < DiceStopUI_Order.Length;i++)
        {
            DiceStopUI_Order[i].enabled = false;
        }
        DiceStopUI.enabled = false;
    }

    //ダイスの数のUIの表示非表示
    public void DiceNumUISet(int DiceNum)
    {
        DiceImage.transform.position = this.gameObject.transform.position + DiceImagePos;
        DiceImage.enabled = true;
        DiceImage.sprite = DiceNumSprite[DiceNum - 1];
    }
    public void DiceNumUIDestroy()
    {
        SEManager.Instance.Play(SEPath.SCENE_CHANGE);
        DiceImage.enabled = false;
    }

    //プレイヤーステータスUIの表示
    public void PlayerStatusUISet()
    {
        //プレイヤーのキャンディ、やる気を獲得する
        for (int i = 0; i < PlayerStatusUI.Length; i++)
        {
            PlayerStatusUI[i].enabled = true;
            PlayerNametxt[i].text = "プレイヤー" + (i + 1).ToString();
        }
        PlayerStatusChange();
    }
    public void PlayerStatusUIDestroy()
    {
        for (int i = 0; i < PlayerStatusUI.Length; i++)
        {
            PlayerStatusUI[i].enabled = false;
            Candytxt[i].text = "";
            Yarukitxt[i].text = "";
            PlayerNametxt[i].text = "";
        }
    }
    //プレイヤーのステータスUIの更新
    public void PlayerStatusChange()
    {
        for (int i = 0; i < manager.characters.Length; i++)
        {
            Candytxt[i].text = manager.characters[i].candy.ToString();
            Yarukitxt[i].text = manager.characters[i].yaruki.ToString();
        }
    }

    //ゲーム終了UIの表示非表示
    public void GameEndUISet()
    {
        GameSetUI.enabled = true;
        GameSetflg = true;
        SEManager.Instance.Play(SEPath.GAME_END);
    }
    public void GameEndUIDestroy()
    {
        feedobj = GameObject.Find("EventController");
        feed = feedobj.GetComponent<SpuareAction>();
        feed.FeedInFlg =true;
        feed.isResult = true;
        GameSetUI.enabled = false;
    }

    private void Update()
    {
        if(manager.gameStatus == GameSTS.Ranking)
        {
            if(Input.GetButtonDown("BtnB"))
            {
                if(GameSetflg)
                {
                    GameEndUIDestroy();
                    GameSetflg = false;
                }
            }
        }
    }
}
