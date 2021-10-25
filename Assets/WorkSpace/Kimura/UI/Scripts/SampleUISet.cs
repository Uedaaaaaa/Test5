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

    //プレイヤーターンUIの表示フラグ
    //trueなら表示中
    private bool PlayerTurnUIMoveFlg = false;

    //実際に表示するダイスUI
    [SerializeField] Image DiceImage;

    //ダイスUIのスプライト
    [SerializeField] Sprite[] DiceNumSprite = new Sprite[6];
    [Header("ダイスUIの位置")]
    [SerializeField] Vector3 DiceImagePos = new Vector3(0,4,0);
    // Start is called before the first frame update
    void Start()
    {
        //UIの表示をオフにする
        ImagePlayerTurnUI.gameObject.SetActive(false);
        DiceImage.gameObject.SetActive(false);
    }
    public void PlayerTurnUISet(int PlayerNum)
    {
        if (!PlayerTurnUIMoveFlg)
        {
            ImagePlayerTurnUI.gameObject.SetActive(true);
            ImagePlayerTurnUI.sprite = PlayerTurnUI[PlayerNum].sprite;
            PlayerTurnUIMoveFlg = true;
            ImagePlayerTurnUI.transform.position += new Vector3(22, 0, 0);
        }
    }
    public void DiceUISet(int DiceNum)
    {
        DiceImage.transform.position = this.gameObject.transform.position + DiceImagePos;
        DiceImage.gameObject.SetActive(true);
        DiceImage.sprite = DiceNumSprite[DiceNum - 1];
    }
    public void DiceUIDestroy()
    {
        DiceImage.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            PlayerTurnUISet(Random.Range(0,4));
        }
        if(Input.GetKeyDown(KeyCode.B))
        {
            DiceUISet(3);
        }
        if(PlayerTurnUIMoveFlg)
        {
            ImagePlayerTurnUI.transform.position -= new Vector3(0.1f * TurnUIMoveSpeed, 0, 0);
            if(ImagePlayerTurnUI.transform.position.x <= this.gameObject.transform.position.x - 22.0f)
            {
                PlayerTurnUIMoveFlg = false;
                ImagePlayerTurnUI.transform.position = this.gameObject.transform.position;
                ImagePlayerTurnUI.gameObject.SetActive(false);
            }
        }
    }
}
