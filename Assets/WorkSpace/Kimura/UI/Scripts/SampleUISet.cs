using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SampleUISet : MonoBehaviour
{
    //実際に表示するプレイヤーターンUI
    [SerializeField] Image ImagePlayerTurnUI;
    [Header("プレイヤーのターンUIの移動速度")]
    [SerializeField] float TurnUIMoveSpeed = 1.0f;

    //プレイヤーターンUIのスプライト獲得
    [SerializeField] Image[] PlayerTurnUI = new Image[4];

    //プレイヤーターンUI表示と同時にセットするBボタンUI
    [SerializeField] Image PlayerTurnBbuttonUI;

    //プレイヤーターンUIの表示フラグ
    //trueなら表示中
    //private bool PlayerTurnUIMoveFlg = false;

    //ダイスを止めると書かれたUI
    [SerializeField] Image DiceStopUI;

    //実際に表示するダイスの数字UI
    [SerializeField] Image DiceImage;

    //ダイスの数字UIのスプライト
    [SerializeField] Sprite[] DiceNumSprite = new Sprite[6];
    [Header("ダイスUIの位置")]
    [SerializeField] Vector3 DiceImagePos = new Vector3(0,4,0);

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
        for(int i = 0; i < PlayerStatusUI.Length -1; i++)
        {
            PlayerStatusUI[i].gameObject.SetActive(false);
            Candytxt[i].text = "";
            Yarukitxt[i].text = "";
        }

        //ゲームマネージャーを獲得
        manager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
        //プレイヤーのキャンディ、やる気を獲得する
        for(int i = 0; i < manager.characters.Length -1; i++)
        {
            PlayerStatusUI[i].gameObject.SetActive(true);
            Candytxt[i].text = manager.characters[i].Candy.ToString();
            Yarukitxt[i].text = manager.characters[i].Yaruki.ToString();
        }
    }

    //ターンUIの表示非表示
    public void PlayerTurnUISet(int PlayerNum)
    {
        //if (!PlayerTurnUIMoveFlg)
        //{
        ImagePlayerTurnUI.gameObject.SetActive(true);
        ImagePlayerTurnUI.sprite = PlayerTurnUI[PlayerNum].sprite;

        PlayerTurnBbuttonUI.gameObject.SetActive(true);
            //PlayerTurnUIMoveFlg = true;
            //ImagePlayerTurnUI.transform.position += new Vector3(22, 0, 0);
        //}
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
        for (int i = 0; i < manager.characters.Length - 1; i++)
        {
            Candytxt[i].text = manager.characters[i].Candy.ToString();
            Yarukitxt[i].text = manager.characters[i].Yaruki.ToString();
        }
    }




    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.A))
        //{
        //    PlayerTurnUISet(Random.Range(0,4));
        //}
        //if(Input.GetKeyDown(KeyCode.B))
        //{
        //    DiceNumUISet(3);
        //}
        //if(PlayerTurnUIMoveFlg)
        //{
            //ImagePlayerTurnUI.transform.position -= new Vector3(0.1f * TurnUIMoveSpeed, 0, 0);
            //if(ImagePlayerTurnUI.transform.position.x <= this.gameObject.transform.position.x - 22.0f)
            //{
            //    PlayerTurnUIMoveFlg = false;
            //    ImagePlayerTurnUI.transform.position = this.gameObject.transform.position;
            //    ImagePlayerTurnUI.gameObject.SetActive(false);
            //}
        //}
    }
}
