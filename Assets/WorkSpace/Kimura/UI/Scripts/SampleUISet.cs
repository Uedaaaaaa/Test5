using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SampleUISet : MonoBehaviour
{
    public Image[] PlayerTurnUI = new Image[4];
    Image SetPlayerTurnUI;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < PlayerTurnUI.Length;i++)
        {

        }
    }
    void PlayerTrunUIDestroy()
    {
        if (SetPlayerTurnUI != null)
        {
            Destroy(SetPlayerTurnUI);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            SetPlayerTurnUI = Instantiate(PlayerTurnUI[0]);
        }
        if(Input.GetKeyDown(KeyCode.B))
        {
            SetPlayerTurnUI = Instantiate(PlayerTurnUI[1]);
        }
    }
}
