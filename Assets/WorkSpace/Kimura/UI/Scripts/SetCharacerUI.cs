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
    [SerializeField] Image DiceStartUI;

    //ダイスを止めると書かれたUI
    [SerializeField] Image DiceStopUI;

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

    //スクエアアクション
    SpuareAction feed;
    private GameObject feedobj;
    // Start is called before the first frame update
    void Start()
    {
        //ダイスの数字UIの高さを変更
        DiceImagePos.y = 100;
        //プレイヤー以外のUI表示をオフにする
        ImagePlayerTurnUI.enabled = false;
        PlayerTurnBbuttonUI.enabled = false;
        DiceStopUI.enabled = false;
        DiceImage.enabled = false;
        GameSetUI.enabled = false;
        DiceStartUI.enabled = false;
        //プレイヤー関係のUIの非表示
        for (int i = 0; i < PlayerStatusUI.Length; i++)
        {
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
        DiceStartUI.enabled = true;
    }
    public void DiceStartUIDestroy()
    {
        DiceStopUI.enabled = false;
    }

    //ダイスを止めるUIの表示非表示
    public void DiceStopUISet()
    {
        //UIの位置を変更する
        if(manager.gameStatus == GameSTS.OrderJudge)
        {
            switch(manager.NowPlayerNo)
            {
                case 0:
                    DiceStopUI.gameObject.transform.position = new Vector3(-1175,0,0);
                    break;
                case 1:
                    DiceStopUI.gameObject.transform.position = new Vector3(-740, 0, 0);
                    break;
                case 2:
                    DiceStopUI.rectTransform.position = new Vector3(-310, 0, 0);
                    break;
                case 3:
                    DiceStopUI.rectTransform.position = new Vector3(100, 0, 0);
                    break;
            }
        }
        else
        {
            DiceStopUI.rectTransform.position = new Vector3(0, 0, 0);
        }
        //ダイスを止めるUIの表示
        DiceStopUI.enabled = true;
    }
    public void DiceStopUIDestroy()
    {
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
        PlayerTurnBbuttonUI.enabled = true;
        SEManager.Instance.Play(SEPath.GAME_END);
    }
    public void GameEndUIDestroy()
    {
        feedobj = GameObject.Find("EventController");
        feed = feedobj.GetComponent<SpuareAction>();
        feed.FeedInFlg =true;
        feed.isResult = true;
        GameSetUI.enabled = false;
        PlayerTurnBbuttonUI.enabled = false;
    }
}
