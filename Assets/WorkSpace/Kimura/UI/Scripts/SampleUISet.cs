using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SampleUISet : MonoBehaviour
{
    public Image[] PlayerTurnUI = new Image[4];
    GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        for (int i = 0; i < PlayerTurnUI.Length -1;i++)
        {
            PlayerTurnUI[i].enabled = false;
        }
    }
    public void SetPlayerTurnUI(int PlayerNum)
    {
        PlayerTurnUI[PlayerNum].enabled = true;
        PlayerTurnUI[PlayerNum].gameObject.transform.position = (camera.transform.position);
        PlayerTurnUI[PlayerNum].gameObject.transform.position += new Vector3(10,0,0); 
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            SetPlayerTurnUI(0);
        }
        if(Input.GetKeyDown(KeyCode.B))
        {

        }
    }
}
