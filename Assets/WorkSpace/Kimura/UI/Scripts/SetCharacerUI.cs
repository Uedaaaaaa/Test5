using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    //ダイスを止めると書かれたUI
    [SerializeField] Image DiceStopUI;

    //実際に表示するダイスの数字UI
    [SerializeField] Image DiceImage;

    //ダイスの数字UIのスプライト
    [SerializeField] Sprite[] DiceNumSprite = new Sprite[6];
    [Header("ダイスUIの位置")]
    [SerializeField] Vector3 DiceImagePos = new Vector3(0, 4, 0);

    //プレイヤーのステータスUI
    [SerializeField] Image[] PlayerStatusUI = new Image[4];
    //プレイヤーのやる気、キャンディの数
    [SerializeField] Text[] Candytxt = new Text[4];
    [SerializeField] Text[] Yarukitxt = new Text[4];

    //ゲームマネージャー
    GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        //UIの表示をオフにする
        ImagePlayerTurnUI.gameObject.SetActive(false);
        PlayerTurnBbuttonUI.gameObject.SetActive(false);
        DiceStopUI.gameObject.SetActive(false);
        DiceImage.gameObject.SetActive(false);
        for (int i = 0; i < PlayerStatusUI.Length; i++)
        {
            PlayerStatusUI[i].gameObject.SetActive(false);
            Candytxt[i].text = "";
            Yarukitxt[i].text = "";
        }

        //ゲームマネージャーを獲得
        manager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
        //プレイヤーのキャンディ、やる気を獲得する
        for (int i = 0; i < manager.characters.Length; i++)
        {
            PlayerStatusUI[i].gameObject.SetActive(true);
            Candytxt[i].text = manager.characters[i].Candy.ToString().PadLeft(2, '0');
            Yarukitxt[i].text = manager.characters[i].Yaruki.ToString().PadLeft(2, '0');
        }
    }

    //ターンUIの表示非表示
    public void PlayerTurnUISet(int PlayerNum)
    {
        ImagePlayerTurnUI.gameObject.SetActive(true);
        ImagePlayerTurnUI.sprite = PlayerTurnUI[PlayerNum].sprite;

        PlayerTurnBbuttonUI.gameObject.SetActive(true);
    }
    public void PlayerTurnUIDestroy()
    {
        ImagePlayerTurnUI.gameObject.SetActive(false);
        PlayerTurnBbuttonUI.gameObject.SetActive(false);
    }

    //ダイスを止めるUIの表示非表示
    public void DiceStopUISet()
    {
        DiceStopUI.gameObject.SetActive(true);
    }
    public void DiceStopUIDestroy()
    {
        DiceStopUI.gameObject.SetActive(false);
    }

    //ダイスの数のUIの表示非表示
    public void DiceNumUISet(int DiceNum)
    {
        DiceImage.transform.position = this.gameObject.transform.position + DiceImagePos;
        DiceImage.gameObject.SetActive(true);
        DiceImage.sprite = DiceNumSprite[DiceNum - 1];
    }
    public void DiceNumUIDestroy()
    {
        DiceImage.gameObject.SetActive(false);
    }

    //プレイヤーのステータスUIの更新
    public void PlayerStatusChange()
    {
        for (int i = 0; i < manager.characters.Length; i++)
        {
            Candytxt[i].text = manager.characters[i].Candy.ToString();
            Yarukitxt[i].text = manager.characters[i].Yaruki.ToString();
        }
    }
}
